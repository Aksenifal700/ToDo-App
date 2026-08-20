using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TodoApp.Interfaces;
using TodoApp.Interfaces.DTOs.Auth;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace TodoApp.DataAccess;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IConfiguration _configuration;
    private readonly TimeSpan _tokenLifetime;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
        _tokenLifetime = TimeSpan.Parse(_configuration["Jwt:TokenLifeTime"]!);
    }

    public string? GenerateJwtToken(TokenGenerationDto dto)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        string secret = _configuration["Jwt:Secret"];
        var key = Encoding.UTF8.GetBytes(secret);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, dto.Email),
            new(JwtRegisteredClaimNames.Email, dto.Email),
            new("userid", dto.UserId.ToString())
        };

        var tokenDesciptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(_tokenLifetime),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDesciptor);
        
        return tokenHandler.WriteToken(token);
    }
    
}