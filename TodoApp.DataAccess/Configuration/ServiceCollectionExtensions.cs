using Microsoft.Extensions.DependencyInjection;
using TodoApp.DataAccess.Database.Repositories;
using TodoApp.Interfaces;

namespace TodoApp.DataAccess.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ITaskItemRepository, TaskItemRepository>();
        
        return services;
    }
}