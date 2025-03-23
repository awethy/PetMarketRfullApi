using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using PetMarketRfullApi.Data.Contexts;
using System.Security.Claims;

namespace PetMarketRfullApi.Extensions
{
    public static class ServiceCollectionsExtensions
    {
        public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo
                { 
                    Title = "Pet Market API",
                    Version = "v1",
                });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Enter a valid token",
                    Name = "Auth",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
            return builder;
        }

        public static WebApplicationBuilder AppData(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("MyConString")));

            return builder;
        }

        public static WebApplicationBuilder AddAuthorization(this WebApplicationBuilder builder)
        {
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
                options.AddPolicy("CreateOrder", policy =>
                {
                    policy.RequireAuthenticatedUser();
                });
                options.AddPolicy("AdminOnly", policy => 
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("admin");
                });
            });
            return builder;
        }
    }
}
