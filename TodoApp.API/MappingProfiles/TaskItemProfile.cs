using AutoMapper;
using TodoApp.Interfaces.DTOs.TaskItem;
using TodoApp.Interfaces.Entities;

namespace TodoApp.API.MappingProfiles;

public class TaskItemProfile : Profile
{
    public TaskItemProfile()
    {
        CreateMap<CreateTaskItemDto, TaskItem>();
        CreateMap<UpdateTaskItemDto, TaskItem>();
        CreateMap<TaskItem, TaskItemDto>();
    }
}