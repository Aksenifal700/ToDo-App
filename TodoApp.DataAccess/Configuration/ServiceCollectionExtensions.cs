using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using TodoApp.DataAccess.Caching;
using TodoApp.DataAccess.Database.Repositories;
using TodoApp.Interfaces;
using TodoApp.Interfaces.IServices;

namespace TodoApp.DataAccess.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ITaskItemRepository, TaskItemRepository>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        return services;
    }
    
    public static IServiceCollection AddRedisCaching(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConnectionMultiplexer>(sp =>
            ConnectionMultiplexer.Connect(configuration["Redis:ConnectionString"]!));

        services.AddScoped<ICacheService, RedisCacheService>();
        services.AddScoped<ICachedQueryService, CachedQueryService>();

        return services;
    }
}