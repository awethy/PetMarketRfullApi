using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using PetMarketRfullApi.Domain.Models;
using Microsoft.AspNetCore.Identity;
using PetMarketRfullApi.Web.Extensions;
using PetMarketRfullApi.Application.Mapping;
using PetMarketRfullApi.Infrastructure.Data.Contexts;
using PetMarketRfullApi.Application.Abstractions;
using PetMarketRfullApi.Domain.Options;
using RabbitMQ.Client;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(ModelToResourceProfile));

builder.Services.AddIdentity<User, UserRole>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddUserManager<UserManager<User>>()
.AddUserStore<UserStore<User, UserRole, AppDbContext, string>>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();

builder
    .AddBearerAuthentication()
    .AddOptions()
    .AddSwagger()
    .AppData()
    .AddRedis()
    .AddApplicationServices()
    .AddAuthorization()
    .AddBackgroundService();

// Временная проверка конфигурации
var rabbitConfig = builder.Configuration.GetSection("RabbitMQ").Get<RabbitMqOptions>();
Console.WriteLine($"RabbitMQ Config: {JsonSerializer.Serialize(rabbitConfig)}");

// Проверка регистрации сервисов
Console.WriteLine($"IConnection registered: {builder.Services.Any(x => x.ServiceType == typeof(IConnection))}");
Console.WriteLine($"IRabbitMqChannelFactory registered: {builder.Services.Any(x => x.ServiceType == typeof(IRabbitMqChannelFactory))}");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();