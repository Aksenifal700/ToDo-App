using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApp.API.Helpers;
using TodoApp.API.Models.Request.Task;
using TodoApp.API.Models.Response;
using TodoApp.Interfaces.DTOs.TaskItem;
using TodoApp.Interfaces.IServices;

namespace TodoApp.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TaskItemController : ControllerBase
{
    private readonly ITaskItemService _taskItemService;
    private readonly IMapper _mapper;

    public TaskItemController(IMapper mapper, ITaskItemService taskItemService)
    {
        _mapper = mapper;
        _taskItemService = taskItemService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<TaskResponse>>> GetTasks([FromQuery] TaskQueryRequest query)
    {
        var userId = User.GetUserId();
        
        var (items, totalCount) = await _taskItemService.GetPagedAsync(
            userId,
            query.PageNumber,
            query.PageSize,
            query.SearchTerm,
            query.CategoryId,
            query.IsCompleted);

        var response = new PagedResponse<TaskResponse>
        {
            Items = _mapper.Map<List<TaskResponse>>(items),
            TotalCount = totalCount,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskResponse>> GetTask(Guid id)
    {
        var userId = User.GetUserId();
        
        var task = await _taskItemService.GetByIdAsync(id, userId);
        if(task is null)
            return NotFound();
        
        var response = _mapper.Map<TaskResponse>(task);
        return Ok(response);
    }
    
    [HttpPost]
    public async Task<ActionResult<TaskResponse>> CreateTask([FromBody] CreateTaskRequest request)
    {
        var userId = User.GetUserId();
        var dto = _mapper.Map<CreateTaskItemDto>(request);

        var result = await _taskItemService.CreateAsync(dto, userId);
        var response = _mapper.Map<TaskResponse>(result);

        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TaskResponse>> UpdateTask(Guid id, [FromBody] UpdateTaskRequest request)
    {
        var userId = User.GetUserId();
        var dto = _mapper.Map<UpdateTaskItemDto>(request);

        var result = await _taskItemService.UpdateAsync(id, dto, userId);
        var response = _mapper.Map<TaskResponse>(result);

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(Guid id)
    {
        var userId = User.GetUserId();

        await _taskItemService.DeleteAsync(id, userId);

        return NoContent();
    }
    
}