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

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(ModelToResourceProfile));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyConString")));

builder.Services.AddIdentity<User, UserRole>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICategoryServices, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddScoped<IPetService, PetService>();
builder.Services.AddScoped<IPetRepository, PetRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Add services to the container.
//test2

builder.Services.AddControllers()
    .AddJsonOptions(options =>  
    {
        options.JsonSerializerOptions.Converters.Add(new CustomDateConverter("dd/MM/yyyy"));
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UpdateUserPolicy", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireAssertion(context =>
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var routeData = context.Resource as RouteData;
            var requestedUserId = routeData?.Values["id"]?.ToString();

            return userId == requestedUserId || context.User.IsInRole("admin");
        });
    });
    options.AddPolicy("AdminOnly", policy => {
        policy.RequireAuthenticatedUser();
        policy.RequireRole("admin");
    });
});


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
