using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PetMarketRfullApi.Domain.Repositories;
using System.Security.Claims;
using System.Text;
using PetMarketRfullApi.Domain.Options;
using PetMarketRfullApi.Application.Abstractions;
using PetMarketRfullApi.Application.Sevices;
using Microsoft.OpenApi.Models;
using PetMarketRfullApi.Infrastructure.Data.Contexts;
using PetMarketRfullApi.Infrastructure.Data.Repositories;
using PetMarketRfullApi.Web.BackgroundServices;
using RabbitMQ.Client;
using Microsoft.Extensions.Options;
using StackExchange.Redis;


namespace PetMarketRfullApi.Web.Extensions
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
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("PgConString"),
                    opt => opt.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorCodesToAdd: null
                    )
                )
            );
            return builder;
        }

        public static WebApplicationBuilder AddRedis(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer
                .Connect(ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"))));

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

        public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

            builder.Services.AddScoped<IPetService, PetService>();
            builder.Services.AddScoped<IPetRepository, PetRepository>();

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();

            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<ICartRepository, CartRepository>();

            builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();

            builder.Services.AddScoped<IRedisCartRepository, RedisCartRepository>();
            builder.Services.AddScoped<IRedisCartService, RedisCartService>();

            builder.Services.AddSingleton<IRabbitMqChannelFactory, RabbitMqChannelFactory>();

            return builder;
        }

        public static WebApplicationBuilder AddBackgroundService( this WebApplicationBuilder builder)
        {
            builder.Services.AddHostedService<CreateOrderConsumer>();

            return builder;
        }

        public static WebApplicationBuilder AddBearerAuthentication(this WebApplicationBuilder builder)
        {
            builder.Services
                .AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(
                            builder.Configuration["Authentication:TokenPrivateKey"]!)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            builder.Services.AddTransient<IAuthService, AuthService>();

            return builder;
        }

        public static WebApplicationBuilder AddOptions(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<AuthOptions>(builder.Configuration.GetSection("Authentication"));
            builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMQ"));

            builder.Services.AddSingleton<IConnection>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<RabbitMqOptions>>().Value;
                var factory = new ConnectionFactory
                {
                    HostName = options.HostName,
                    Port = options.Port,
                    UserName = options.UserName,
                    Password = options.Password,
                    VirtualHost = options.VirtualHost
                };
                try
                {
                    var connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
                    Console.WriteLine("✅ RabbitMQ connection established");
                    return connection;
                }
                catch (Exception ex) 
                {
                    Console.WriteLine($"❌ Failed to create RabbitMQ connection: {ex.Message}");
                    throw;
                }
            });

            return builder;
        }
    }
}
