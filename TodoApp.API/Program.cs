using Microsoft.EntityFrameworkCore;
using TodoApp.API;
using TodoApp.API.Configurations;
using TodoApp.BusinessLogic.Configurations;
using TodoApp.BusinessLogic.IServices;
using TodoApp.DataAccess.Configuration;
using TodoApp.DataAccess.Database;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJwtConfiguration(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddServices();
builder.Services.AddRepositories();

builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();