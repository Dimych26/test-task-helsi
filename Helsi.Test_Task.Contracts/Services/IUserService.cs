using Ardalis.Result;
using Helsi.Test_Task.Dtos.User;

namespace Helsi.Test_Task.Contracts.Services;

public interface IUserService
{
    Task<Result<UserDto>> GetByIdAsync(string id);

    Task<bool> ExistsAsync(string id);

    Task<Result<List<UserDto>>> GetAllAsync();

    Task<Result<UserDto>> CreateAsync(UserCreateDto dto);
}
