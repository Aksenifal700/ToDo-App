using AutoMapper;
using TodoApp.BusinessLogic.Security;
using TodoApp.Interfaces;
using TodoApp.Interfaces.DTOs.Auth;
using TodoApp.Interfaces.Entities;
using TodoApp.Interfaces.IRepositories;
using TodoApp.Interfaces.IServices;

namespace TodoApp.BusinessLogic.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IPasswordHasher _passwordHasher;

    public AuthService(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _passwordHasher = passwordHasher;
    }

    public async Task<LoginResultDto> LoginAsync(LoginDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user is null)
            throw new Exception();

        var isPasswordValid = _passwordHasher.VerifyHash(dto.Password, user.PasswordHash);
        if (!isPasswordValid)
            throw new Exception();
        
        var jwtToken = _jwtTokenGenerator.GenerateJwtToken(new TokenGenerationDto
        {
            UserId = user.Id,
            Email = dto.Email,
        });
        
        return new LoginResultDto()
        {
            UserId = user.Id,
            Email = user.Email,
            Token = jwtToken
        };
        
    }

    public async Task<LoginResultDto> RegisterAsync(RegisterDto dto)
    {
        var emailExists = await _userRepository.ExistsByEmailAsync(dto.Email);

        if (emailExists)
            throw new Exception("");
        
        var passwordHash = _passwordHasher.HashPassword(dto.Password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = dto.Email,
            PasswordHash = passwordHash,
        };
        
        await _userRepository.AddAsync(user);

        var jwtToken = _jwtTokenGenerator.GenerateJwtToken(new TokenGenerationDto
        {
            UserId = user.Id,
            Email = dto.Email,
        });

        return new LoginResultDto
        {
            Token = jwtToken,
            UserId = user.Id,
            Email = user.Email,
        };
    }
}