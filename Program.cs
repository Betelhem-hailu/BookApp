using BookStore.Context;
using BookStore.Models;
using BookStore.Middleware;
using Microsoft.EntityFrameworkCore;
using BookStore.Extensions;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using BookStore.Mapping;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

services.AddLogging(builder => builder.AddConsole());
builder.Services.AddAutoMapper(typeof(BookMapper));
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddLogging();

// Configure DbContext with connection string
builder.Services.AddDbContext<BookDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


ServiceExtensions.ConfigureCors(services);
ServiceExtensions.ConfigureJwtAuthentication(services, configuration);
ServiceExtensions.ConfigureAuthorizationPolicies(services);
services.ConfigureRepositories();
services.ConfigureBusinessServices();
ServiceExtensions.ConfigureCloudinary(services, configuration);


var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowReactApp");
app.UseAuthentication(); // Ensure authentication is added
app.UseAuthorization();  // Ensure authorization is added

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
