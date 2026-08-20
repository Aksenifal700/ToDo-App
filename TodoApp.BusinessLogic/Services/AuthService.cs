using System.Security.Authentication;
using TodoApp.BusinessLogic.Exceptions;
using TodoApp.BusinessLogic.Security;
using TodoApp.Interfaces;
using TodoApp.Interfaces.DTOs.Auth;
using TodoApp.Interfaces.Entities;
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
            throw new InvalidCredentialException();

        var isValid = _passwordHasher.VerifyPasswordHash(dto.Password, user.PasswordHash, user.PasswordSalt);
        if (!isValid)
            throw new InvalidCredentialException();
        
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
            throw new EmailAlreadyExistsException("Email already exists");
        
        _passwordHasher.CreatePasswordHash(dto.Password, out byte[] passwordHash, out byte[] passwordSalt);

        var userDto = await _userRepository.CreateUserAsync(dto, passwordHash, passwordSalt);
       
        var jwtToken = _jwtTokenGenerator.GenerateJwtToken(new TokenGenerationDto
        {
            UserId = userDto.Id,
            Email = dto.Email,
        });

        return new LoginResultDto
        {
            Token = jwtToken,
            UserId = userDto.Id,
            Email = userDto.Email,
        };
    }
}