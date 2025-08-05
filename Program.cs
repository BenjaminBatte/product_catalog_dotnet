using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProductCatalogApi.Data;
using ProductCatalogApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Register the DbContext with PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
// Register app services BEFORE Build()
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CategoryService>();
// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Product Catalog API", Version = "v1" });
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
