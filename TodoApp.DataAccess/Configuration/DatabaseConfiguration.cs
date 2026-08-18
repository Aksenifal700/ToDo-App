using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.DataAccess.Database;

namespace TodoApp.DataAccess.Configuration;

public static class DatabaseConfiguration
{
    public static void AddDatabase(
        IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options => 
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection")));
    }
}