using AutoMapper;
using Helsi.Test_Task.Contracts.Services;
using Helsi.Test_Task.Dtos.User;
using Helsi.Test_Task.Infrastructure.Extensions;
using Helsi.Test_Task.Infrastructure.Utils;
using Helsi.Test_Task.WebApi.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace Helsi.Test_Task.WebApi.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    public IUserService _userService;
    public IMapper _mapper;

    public UserController(
        IUserService userService,
        IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    [HttpPost("create")]
    public async Task<ActionResult<UserModel>> CreateAsync(UserCreateModel model)
    {
        var dto = _mapper.Map<UserCreateDto>(model);
        var result = await _userService.CreateAsync(dto);

        return result
            .ToResult(_mapper.Map<UserModel>)
            .ToActionResult(this);
    }

    [HttpGet("")]
    public async Task<ActionResult<IReadOnlyList<UserModel>>> GetAllAsync()
    {
        var result = await _userService.GetAllAsync();

        return result
            .ToResult(_mapper.Map<IReadOnlyList<UserModel>>)
            .ToActionResult(this);
    }
}
