using AutoMapper;
using TodoApp.Interfaces.DTOs.Category;
using TodoApp.Interfaces.Entities;

namespace TodoApp.API.MappingProfiles;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<CreateCategoryDto, Category>();
        CreateMap<UpdateCategoryDto, Category>();
        CreateMap<Category, CategoryDto>();
    }
}