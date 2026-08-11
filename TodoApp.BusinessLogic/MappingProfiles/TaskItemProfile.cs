using AutoMapper;
using TodoApp.DataAccess.Database.Entities;
using TodoApp.Interfaces.DTOs.TaskItem;

namespace TodoApp.BusinessLogic.MappingProfiles;

public class TaskItemProfile : Profile
{
    public TaskItemProfile()
    {
        CreateMap<CreateTaskItemDto, TaskItem>();
        CreateMap<UpdateTaskItemDto, TaskItem>();
        CreateMap<TaskItem, TaskItemDto>();
    }
}