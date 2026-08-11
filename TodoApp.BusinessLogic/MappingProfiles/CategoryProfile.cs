using AutoMapper;
using TodoApp.DataAccess.Database.Entities;
using TodoApp.Interfaces.DTOs.Category;

namespace TodoApp.BusinessLogic.MappingProfiles;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<CreateCategoryDto, Category>();
        CreateMap<UpdateCategoryDto, Category>();
        CreateMap<Category, CategoryDto>();
    }
}