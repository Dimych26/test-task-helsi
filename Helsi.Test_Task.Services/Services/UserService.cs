using Amazon.Runtime.Internal.Util;
using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using AutoMapper;
using DnsClient.Internal;
using Helsi.Test_Task.Contracts;
using Helsi.Test_Task.Contracts.Services;
using Helsi.Test_Task.Core;
using Helsi.Test_Task.Dtos.User;
using Helsi.Test_Task.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Helsi.Test_Task.Services.Services;

public class UserService : IUserService
{
    public IMongoUnitOfWork _unitOfWork;
    public IMapper _mapper;
    public IValidator _validator;
    public IRequestContext _context;
    public ILogger<UserService> _logger;

    public UserService(
        IMongoUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator validator,
        IRequestContext context,
        ILogger<UserService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;
        _context = context;
        _logger = logger;
    }

    public async Task<Result<List<UserDto>>> GetAllAsync()
    {
        var users = await _unitOfWork.GetAllAsync<User>();

        return _mapper.Map<List<UserDto>>(users);
    }

    public async Task<Result<UserDto>> GetByIdAsync(string id)
    {
        var user = await _unitOfWork.GetByIdAsync<User>(id);

        return _mapper.Map<UserDto>(user);
    }

    public async Task<bool> ExistsAsync(string id)
    {
        var builder = Builders<User>.Filter;
        var filter = builder.Eq(x => x.Id, ObjectId.Parse(_context.UserId));

        return await _unitOfWork.GetCollection<User>()
            .Find(filter)
            .AnyAsync();
    }

    public async Task<Result<UserDto>> CreateAsync(UserCreateDto dto)
    {
        try
        {
            var valRes = await _validator.ValidateAsync(dto);
            if (!valRes.IsValid)
            {
                return Result.Invalid(valRes.AsErrors());
            }

            var user = _mapper.Map<User>(dto);

            await _unitOfWork.AddAsync(user);

            return _mapper.Map<UserDto>(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception has been thrown while creating the user.");

            return Result.Error();
        }
    }
}
