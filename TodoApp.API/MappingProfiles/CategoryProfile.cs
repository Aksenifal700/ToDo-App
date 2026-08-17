using AutoMapper;
using TodoApp.API.Models.Request.Category;
using TodoApp.API.Models.Resposne;
using TodoApp.Interfaces.DTOs.Category;

namespace TodoApp.API.MappingProfiles;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<CreateCategoryRequest, CreateCategoryDto>();
        CreateMap<UpdateCategoryRequest, UpdateCategoryDto>();
        CreateMap<CategoryDto, CategoryResponse>();
    }
}