using AutoMapper;
using TodoApp.API.Models.Request.Task;
using TodoApp.API.Models.Response;
using TodoApp.Interfaces.DTOs.TaskItem;
using TodoApp.Interfaces.Entities;

namespace TodoApp.API.MappingProfiles;

public class TaskItemProfile : Profile
{
    public TaskItemProfile()
    {
        CreateMap<CreateTaskRequest, CreateTaskItemDto>();
        CreateMap<UpdateTaskRequest, UpdateTaskItemDto>();
        CreateMap<TaskItemDto, TaskResponse>();
        CreateMap<CreateTaskItemDto, TaskItem>();
        CreateMap<UpdateTaskItemDto, TaskItem>();
        CreateMap<TaskItem, TaskItemDto>();
    }
}