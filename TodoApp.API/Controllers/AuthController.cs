using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TodoApp.API.Models.Request.Auth;
using TodoApp.API.Models.Resposne;
using TodoApp.BusinessLogic.IServices;
using TodoApp.Interfaces.DTOs.Auth;

namespace TodoApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;

    public AuthController(IAuthService authService, IMapper mapper)
    {
        _authService = authService;
        _mapper = mapper;
    }

    [HttpPost("register")]
    public async Task<ActionResult<LoginResultDto>> Register([FromBody] RegisterRequest request)
    {
        var registerDto = _mapper.Map<RegisterDto>(request);
        var result = await _authService.RegisterAsync(registerDto);
        var response = _mapper.Map<LoginResultDto>(result);
        
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResultDto>> Login([FromBody] LogInRequest request)
    {
        var loginDto = _mapper.Map<LoginDto>(request);
        var result = await _authService.LoginAsync(loginDto);
        var response =  _mapper.Map<LogInResponse>(result);
        
        return Ok(response);
    }
    
}