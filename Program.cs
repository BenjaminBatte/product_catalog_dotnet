using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProductCatalogApi.Data;
using ProductCatalogApi.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);



// Register the DbContext with PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

// Register app services - FIXED: Use interfaces if you have them, otherwise keep as is
// If you have interfaces, use: builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CategoryService>();

// Configure Swagger/OpenAPI with enhanced configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Product Catalog API",
        Version = "v1",
        Description = "API for managing products and categories in a catalog",
        Contact = new OpenApiContact
        {
            Name = "API Support",
            Email = "support@productcatalog.com"
        }
    });

    // Enable XML comments for better documentation (optional but recommended)
    try
    {
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
    }
    catch
    {
        // XML file not found, continue without it
        Console.WriteLine("XML documentation file not found. Continuing without XML comments.");
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product Catalog API V1");
        c.RoutePrefix = "swagger"; // Access at /swagger
        c.DocumentTitle = "Product Catalog API Documentation";
    });
}
else
{
    // You can also enable Swagger in production if desired
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product Catalog API V1");
        c.RoutePrefix = "api-docs"; // Different path for production
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();