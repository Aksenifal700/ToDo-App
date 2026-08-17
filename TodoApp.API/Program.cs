using TodoApp.API;
using TodoApp.API.Configurations;
using TodoApp.BusinessLogic.IServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJwtConfiguration(builder.Configuration);

builder.Services.AddApplicationDependencies(
    builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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