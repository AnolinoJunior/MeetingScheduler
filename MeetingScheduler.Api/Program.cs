using DotNetEnv;
using MeetingScheduler.Api.Data;
using Microsoft.EntityFrameworkCore;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

var connectionString =
    $"server={Environment.GetEnvironmentVariable("DB_SERVER")};" +
    $"database={Environment.GetEnvironmentVariable("DB_DATABASE")};" +
    $"user={Environment.GetEnvironmentVariable("DB_USER")};" +
    $"password={Environment.GetEnvironmentVariable("DB_PASSWORD")};";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    ));

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// CORS
app.UseCors("ReactPolicy");

app.MapControllers();

app.Run();