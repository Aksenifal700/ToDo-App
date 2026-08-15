using AutoMapper;
using TodoApp.API.Models.Request.Auth;
using TodoApp.API.Models.Resposne;
using TodoApp.Interfaces.DTOs.Auth;

namespace TodoApp.API.MappingProfiles;

public class AuthProfile : Profile
{
    public AuthProfile()
    {
        CreateMap<RegisterRequest, RegisterDto>();
        
        CreateMap<LogInRequest, LoginDto>();
        
        CreateMap<LoginResultDto, LogInResponse>();
    }
}