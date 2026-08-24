using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApp.API.Helpers;
using TodoApp.API.Models.Request.Category;
using TodoApp.API.Models.Resposne;
using TodoApp.Interfaces.DTOs.Category;
using TodoApp.Interfaces.Entities;
using TodoApp.Interfaces.IServices;

namespace TodoApp.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;

    public CategoriesController(ICategoryService categoryService, IMapper mapper)
    {
        _categoryService = categoryService;
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<CategoryResponse>>> GetCategories()
    {
        var userId = User.GetUserId();
        
        var categories = await _categoryService.GetByUserIdAsync(userId);
        var response = _mapper.Map<List<CategoryResponse>>(categories);
        
        return Ok(response);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryResponse>> GetCategory(Guid id)
    {
        var userId = User.GetUserId();
        
        var category = await _categoryService.GetByIdAsync(id, userId);
        
        var response = _mapper.Map<CategoryResponse>(category);
        return Ok(response);
    }
    
    [HttpPost]
    public async Task<ActionResult<CategoryResponse>> CreateCategory([FromBody] CreateCategoryRequest request)
    {
        var userId = User.GetUserId();
        
        var dto = _mapper.Map<CreateCategoryDto>(request);

        var result = await _categoryService.CreateAsync(dto, userId);
        
        var response = _mapper.Map<CategoryResponse>(result);
        
        return Ok(response);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<CategoryResponse>> UpdateCategory(Guid id, [FromBody] UpdateCategoryRequest request)
    {
        var userId = User.GetUserId();
        
        var updateDto = _mapper.Map<UpdateCategoryDto>(request);
        
        var result = await _categoryService.UpdateAsync(id, updateDto, userId);
        var response = _mapper.Map<CategoryResponse>(result);

        return Ok(response);
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCategory(Guid id)
    {
        var userId = User.GetUserId();
        await _categoryService.DeleteAsync(id, userId);
        return NoContent();
    }
    
}