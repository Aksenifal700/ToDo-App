using AutoMapper;
using TodoApp.Interfaces.DTOs.Auth;
using TodoApp.Interfaces.Entities;

namespace TodoApp.BusinessLogic.MappingProfiles;

public class AuthProfile : Profile
{

    public AuthProfile()
    {
        CreateMap<RegisterDto, User>();
        
        CreateMap<User, LoginResultDto>();
    }
}