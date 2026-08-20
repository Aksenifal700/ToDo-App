using AutoMapper;
using TodoApp.API.Models.Request.Category;
using TodoApp.API.Models.Resposne;
using TodoApp.Interfaces.DTOs.Category;
using TodoApp.Interfaces.Entities;

namespace TodoApp.API.MappingProfiles;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<CreateCategoryRequest, CreateCategoryDto>();
        CreateMap<UpdateCategoryRequest, UpdateCategoryDto>();
        CreateMap<CategoryDto, CategoryResponse>();
        CreateMap<CreateCategoryDto, Category>();
        CreateMap<Category, CategoryDto>();
        CreateMap<UpdateCategoryDto, Category>();
    }
}