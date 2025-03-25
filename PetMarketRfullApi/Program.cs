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

builder.Services.AddControllers()
    .AddJsonOptions(options =>  
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();

builder
    .AddOptions()
    .AddBearerAuthentication()
    .AddSwagger()
    .AppData()
    .AddAuthorization()
    .AddApplicationServices();

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
