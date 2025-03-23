using PetMarketRfullApi.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using PetMarketRfullApi.Domain.Services;
using PetMarketRfullApi.Sevices;
using PetMarketRfullApi.Domain.Repositories;
using PetMarketRfullApi.Data.Repositories;
using AutoMapper;
using PetMarketRfullApi.Mapping;
using PetMarketRfullApi.Infrastructure.Converters;
using System.Security.Cryptography.Xml;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using PetMarketRfullApi.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using PetMarketRfullApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(ModelToResourceProfile));

builder.Services.AddIdentity<User, UserRole>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICategoryServices, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddScoped<IPetService, PetService>();
builder.Services.AddScoped<IPetRepository, PetRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Add services to the container.
//test2

builder.Services.AddControllers()
    .AddJsonOptions(options =>  
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

builder
    .AddSwagger()
    .AppData()
    .AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
