using Microsoft.Extensions.DependencyInjection;
using TodoApp.BusinessLogic.IServices;
using TodoApp.BusinessLogic.Security;
using TodoApp.BusinessLogic.Services;

namespace TodoApp.BusinessLogic.Configurations;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ITaskItemService, TaskItemService>();
        
        return services;
    }
}