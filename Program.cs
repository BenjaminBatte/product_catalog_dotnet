using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProductCatalogApi.Data;
using ProductCatalogApi.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// ---------------------- Services ----------------------

// DbContext (PostgreSQL)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Controllers
builder.Services.AddControllers();

// Application Services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Product Catalog API",
        Version = "v1",
        Description = "API for managing products and categories",
        Contact = new OpenApiContact
        {
            Name = "API Support",
            Email = "support@productcatalog.com"
        }
    });

    // Optional: XML Comments for better Swagger docs
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// ---------------------- App Pipeline ----------------------

var app = builder.Build();

// Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product Catalog API v1");
    c.RoutePrefix = app.Environment.IsDevelopment() ? "swagger" : "api-docs";
    c.DocumentTitle = "Product Catalog API Documentation";
});

// Middleware
app.UseHttpsRedirection();
app.UseAuthorization();

// Map controllers
app.MapControllers();

app.Run();
