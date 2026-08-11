using AutoMapper;
using TodoApp.DataAccess.Database.Entities;
using TodoApp.Interfaces.DTOs.TaskItem;
using TodoApp.Interfaces.IRepositories;
using TodoApp.Interfaces.IServices;

namespace TodoApp.BusinessLogic.Services;

public class TaskItemService : ITaskItemService
{
    private readonly ITaskItemRepository _taskitemRepository;
    private readonly IMapper _mapper;

    public TaskItemService( ITaskItemRepository taskitemRepository, IMapper mapper)
    {
        _taskitemRepository = taskitemRepository;
        _mapper = mapper;
    }

    public async Task<TaskItemDto?> GetByIdAsync(Guid id, Guid userId)
    {
        var taskitem = await _taskitemRepository.GetByIdAsync(id);
        if (taskitem is null || taskitem.UserId != userId)
            return null;
        
        return _mapper.Map<TaskItemDto>(taskitem);
    }
    
    public async Task<List<TaskItemDto>> GetByUserIdAsync(Guid userId)
    {
        var taskitems = await _taskitemRepository.GetByUserIdAsync(userId);
        return _mapper.Map<List<TaskItemDto>>(taskitems);
    }

    public async Task<(List<TaskItemDto> Items, int TotalCount)> GetPagedAsync(
        Guid userId, int pageNumber, int pageSize,
        string? searchTerm = null, Guid? categoryId = null,
        bool? isCompleted = null)
    {
        var result = await _taskitemRepository.GetPagedAsync(
            userId, 
            pageNumber, 
            pageSize, 
            searchTerm, 
            categoryId, 
            isCompleted);
        
        var taskItems = _mapper.Map<List<TaskItemDto>>(result.Items);
        
        return (taskItems, result.TotalCount);
    }

    public async Task<TaskItemDto> CreateAsync(CreateTaskItemDto dto, Guid userId)
    {
        var taskitem = _mapper.Map<TaskItem>(dto);
        
        taskitem.Id = Guid.NewGuid();
        taskitem.UserId = userId;
        taskitem.CreatedAt = DateTime.UtcNow;
        taskitem.IsCompleted = false;
        
        await _taskitemRepository.AddAsync(taskitem);
        
        return _mapper.Map<TaskItemDto>(taskitem);
    }

    public async Task UpdateAsync(Guid id, UpdateTaskItemDto dto, Guid userId)
    {
        var taskitem = await _taskitemRepository.GetByIdAsync(id);
        if(taskitem is null || taskitem.UserId != userId)
            throw new Exception("TaskItem not found");
        
        _mapper.Map(dto, taskitem);
        
        taskitem.UpdatedAt = DateTime.UtcNow;
        
        await _taskitemRepository.Update(taskitem);
    }

    public async Task DeleteAsync(Guid id, Guid userId)
    {
        var taskitem = await _taskitemRepository.GetByIdAsync(id);
        if(taskitem is null || taskitem.UserId != userId)
            throw new Exception("TaskItem not found");
        
        await _taskitemRepository.Delete(taskitem);
    }
}