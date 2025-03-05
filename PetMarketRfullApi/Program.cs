using PetMarketRfullApi.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using PetMarketRfullApi.Domain.Services;
using PetMarketRfullApi.Sevices;
using PetMarketRfullApi.Domain.Repositories;
using PetMarketRfullApi.Data.Repositories;
using AutoMapper;
using PetMarketRfullApi.Mapping;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(ModelToResourceProfile));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyConString")));

builder.Services.AddScoped<ICategoryServices, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

// Add services to the container.
//test2

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
