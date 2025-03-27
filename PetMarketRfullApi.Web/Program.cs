using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using PetMarketRfullApi.Domain.Models;
using Microsoft.AspNetCore.Identity;
using PetMarketRfullApi.Web.Extensions;
using Microsoft.AspNetCore.Builder;
using PetMarketRfullApi.Application.Mapping;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PetMarketRfullApi.Infrastructure.Data.Contexts;

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
