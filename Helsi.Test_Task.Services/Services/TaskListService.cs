using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using AutoMapper;
using Helsi.Test_Task.Contracts;
using Helsi.Test_Task.Contracts.Services;
using Helsi.Test_Task.Core;
using Helsi.Test_Task.Dtos.Options;
using Helsi.Test_Task.Dtos.TaskList;
using Helsi.Test_Task.Infrastructure.Resources;
using Helsi.Test_Task.Infrastructure.Utils;
using Helsi.Test_Task.Infrastructure.Utils.PagedList;
using Helsi.Test_Task.Models;
using Helsi.Test_Task.Services.Extensions;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Helsi.Test_Task.Services.Services;

public class TaskListService : ITaskListService
{
    public IMongoUnitOfWork _unitOfWork;
    public IMapper _mapper;
    public IRequestContext _context;
    public IValidator _validator;
    public ILogger<TaskListService> _logger;

    public TaskListService(
        IMongoUnitOfWork unitOfWork,
        IMapper mapper,
        IRequestContext context,
        IValidator validator,
        ILogger<TaskListService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _context = context;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<PagedList<TaskListShortDto>>> GetPageAsync(PaginationOptions options)
    {
        var builder = Builders<TaskList>.Filter;
        var filter = builder.Eq(x => x.Creator.Id, ObjectId.Parse(_context.UserId))
            | builder.ElemMatch(x => x.Observers, observer => observer.Id == ObjectId.Parse(_context.UserId));

        var sortBuilder = Builders<TaskList>.Sort;
        var sort = sortBuilder.Descending(x => x.Created);

        var pipeline = new EmptyPipelineDefinition<TaskList>()
            .Match(filter)
            .Sort(sort)
            .Project(x => new TaskListShortDto()
            {
                Id = x.Id.ToString(),
                Name = x.Name
            });

        return await _unitOfWork.GetCollection<TaskList>()
            .GetPagedAsync(pipeline, options.PageNumber, options.PageSize);
    }

    public async Task<Result<TaskListDto>> GetByIdAsync(string id)
    {
        var isAllow = await CanAccess(id);
        if (!isAllow)
        {
            return new InvalidResultBuilder()
                .AddError("userId", ValidationErrorCodes.NotAllow)
                .Build();
        }

        var taskList = await _unitOfWork.GetByIdAsync<TaskList>(id);

        if (taskList == null)
        {
            return Result.NotFound();
        }

        return _mapper.Map<TaskListDto>(taskList);
    }

    public async Task<bool> ExistsAsync(string id)
    {
        var builder = Builders<TaskList>.Filter;
        var filter = builder.Eq(x => x.Id, ObjectId.Parse(id));

        return await _unitOfWork.GetCollection<TaskList>()
            .Find(filter)
            .AnyAsync();
    }

    public async Task<Result<TaskListDto>> CreateAsync(TaskListCreateDto dto)
    {
        try
        {
            var valRes = await _validator.ValidateAsync(dto);
            if (!valRes.IsValid)
            {
                return Result.Invalid(valRes.AsErrors());
            }

            if (dto.ObserverIds.Any(x => x == _context.UserId))
            {
                return new InvalidResultBuilder()
                    .AddError(nameof(dto.ObserverIds), ValidationErrorCodes.NotAllow)
                    .Build();
            }

            var taskList = _mapper.Map<TaskList>(dto);
            taskList.Creator = await GetUserAsync(_context.UserId);
            taskList.Observers = await GetUsersAsync(dto.ObserverIds);
            taskList.Created = DateTime.UtcNow;

            await _unitOfWork.AddAsync(taskList);

            return _mapper.Map<TaskListDto>(taskList);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception has been thrown while creating the task list.");

            return Result.Error();
        }
    }

    public async Task<Result> DeleteAsync(string id)
    {
        try
        {
            var isAllow = await IsCreator(id);
            if (!isAllow)
            {
                return new InvalidResultBuilder()
                    .AddError("userId", ValidationErrorCodes.NotAllow)
                    .Build();
            }

            var taskList = await _unitOfWork.GetByIdAsync<TaskList>(id);

            if (taskList == null)
            {
                return Result.NotFound();
            }

            await _unitOfWork.DeleteAsync(taskList);

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception has been thrown while deleting the task list.");

            return Result.Error();
        }
    }

    public async Task<Result<TaskListDto>> UpdateAsync(TaskListUpdateDto dto)
    {
        try
        {
            var valRes = await _validator.ValidateAsync(dto);
            if (!valRes.IsValid)
            {
                return Result.Invalid(valRes.AsErrors());
            }

            var isAllow = await CanAccess(dto.Id);
            if (!isAllow)
            {
                return new InvalidResultBuilder()
                    .AddError("userId", ValidationErrorCodes.NotAllow)
                    .Build();
            }

            var taskList = await _unitOfWork.GetByIdAsync<TaskList>(dto.Id);

            if (dto.ObserverIds.Any(x => x == taskList.Creator.Id.ToString()))
            {
                return new InvalidResultBuilder()
                    .AddError(nameof(dto.ObserverIds), ValidationErrorCodes.NotAllow)
                    .Build();
            }

            taskList.Name = dto.Name;
            taskList.Observers = await GetUsersAsync(dto.ObserverIds);

            await _unitOfWork.UpdateAsync(taskList);

            return _mapper.Map<TaskListDto>(taskList);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception has been thrown while updating the task list.");

            return Result.Error();
        }
    }

    public async Task<Result<TaskListDto>> LinkUserAsync(LinkUserDto dto)
    {
        try
        {
            var valRes = await _validator.ValidateAsync(dto);
            if (!valRes.IsValid)
            {
                return Result.Invalid(valRes.AsErrors());
            }

            var isAllow = await CanAccess(dto.TaskListId);
            if (!isAllow)
            {
                return new InvalidResultBuilder()
                    .AddError("userId", ValidationErrorCodes.NotAllow)
                    .Build();
            }

            var taskList = await _unitOfWork.GetByIdAsync<TaskList>(dto.TaskListId);

            if (taskList.Observers.Any(x => x.Id == ObjectId.Parse(dto.ObserverId)))
            {
                return new InvalidResultBuilder()
                    .AddError(nameof(dto.ObserverId), ValidationErrorCodes.AlreadyObserver)
                    .Build();
            }

            if (taskList.Creator.Id == ObjectId.Parse(dto.ObserverId))
            {
                return new InvalidResultBuilder()
                    .AddError(nameof(dto.ObserverId), ValidationErrorCodes.NotAllow)
                    .Build();
            }

            var observer = await _unitOfWork.GetByIdAsync<User>(dto.ObserverId);

            taskList.Observers.Add(observer);

            await _unitOfWork.UpdateAsync(taskList);

            return _mapper.Map<TaskListDto>(taskList);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception has been thrown while link user {userId} to the task list.", dto.ObserverId);

            return Result.Error();
        }
    }

    public async Task<Result<TaskListDto>> UnlinkUserAsync(LinkUserDto dto)
    {
        try
        {
            var valRes = await _validator.ValidateAsync(dto);
            if (!valRes.IsValid)
            {
                return Result.Invalid(valRes.AsErrors());
            }

            var isAllow = await CanAccess(dto.TaskListId);
            if (!isAllow)
            {
                return new InvalidResultBuilder()
                    .AddError("userId", ValidationErrorCodes.NotAllow)
                    .Build();
            }

            var taskList = await _unitOfWork.GetByIdAsync<TaskList>(dto.TaskListId);
            var observer = taskList.Observers.FirstOrDefault(x => x.Id == ObjectId.Parse(dto.ObserverId));

            if (observer == null)
            {
                return new InvalidResultBuilder()
                   .AddError(nameof(dto.ObserverId), ValidationErrorCodes.NotAnObserver)
                   .Build();
            }

            taskList.Observers.Remove(observer);

            await _unitOfWork.UpdateAsync(taskList);

            return _mapper.Map<TaskListDto>(taskList);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception has been thrown while unlink user {userId} from the task list.", dto.ObserverId);

            return Result.Error();
        }
    }

    private async Task<bool> CanAccess(string taskListId)
    {
        var builder = Builders<TaskList>.Filter;
        var filter = (builder.Eq(x => x.Creator.Id, ObjectId.Parse(_context.UserId))
            | builder.ElemMatch(x => x.Observers, observer => observer.Id == ObjectId.Parse(_context.UserId)))
            & builder.Eq(x => x.Id, ObjectId.Parse(taskListId));

        return await _unitOfWork.GetCollection<TaskList>()
            .Find(filter)
            .AnyAsync();
    }

    private async Task<bool> IsCreator(string taskListId)
    {
        var builder = Builders<TaskList>.Filter;
        var filter = builder.Eq(x => x.Creator.Id, ObjectId.Parse(_context.UserId))
            & builder.Eq(x => x.Id, ObjectId.Parse(taskListId));

        return await _unitOfWork.GetCollection<TaskList>()
            .Find(filter)
            .AnyAsync();
    }

    private async Task<User> GetUserAsync(string userId)
    {
        return await _unitOfWork.GetByIdAsync<User>(userId);
    }

    private async Task<List<User>> GetUsersAsync(string[] userIds)
    {
        if (userIds == null || !userIds.Any() == true)
        {
            return new List<User>();
        }

        var builder = Builders<User>.Filter;
        var filter = builder.In(x => x.Id, userIds.Select(id => new ObjectId(id)));

        return await _unitOfWork.GetByFilterAsync(filter);
    }
}
