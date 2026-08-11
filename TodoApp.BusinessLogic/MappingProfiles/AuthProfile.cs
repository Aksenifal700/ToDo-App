using AutoMapper;
using TodoApp.DataAccess.Database.Entities;
using TodoApp.Interfaces.DTOs.Auth;

namespace TodoApp.BusinessLogic.MappingProfiles;

public class AuthProfile : Profile
{

    public AuthProfile()
    {
        CreateMap<RegisterDto, User>();
        
        CreateMap<User, LoginResultDto>();
    }
}