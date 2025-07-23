using System.Reflection;
using DevOpsTaskApp.Application.AzureDevOps.Application.Common.Models;
using DevOpsTaskApp.Application.Common.Interfaces;
using DevOpsTaskApp.Infrastructure.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var basePath = AppContext.BaseDirectory;

    var webApiXml = Path.Combine(basePath, "DevOpsTaskApp.WebAPI.xml");
    if (File.Exists(webApiXml)) c.IncludeXmlComments(webApiXml);

    var appXml = Path.Combine(basePath, "DevOpsTaskApp.Application.xml");
    if (File.Exists(appXml)) c.IncludeXmlComments(appXml);
   
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DevOps Task API", Version = "v1" });
});
builder.Services.AddHttpClient();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IApplicationDbContext>(provider =>
    provider.GetRequiredService<ApplicationDbContext>());

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(DevOpsTaskApp.Application.AssemblyReference).Assembly));

builder.Services.Configure<AzureDevOpsSettings>(
    builder.Configuration.GetSection("AzureDevOps"));
builder.Services.AddMemoryCache();
builder.Services.AddValidatorsFromAssembly(typeof(DevOpsTaskApp.Application.AssemblyReference).Assembly);
builder.Services.AddAutoMapper(typeof(DevOpsTaskApp.Application.AssemblyReference).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
//builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
builder.Services.AddScoped<IAzureDevOpsService, DevOpsTaskApp.Infrastructure.Services.AzureDevOpsService>();

var app = builder.Build();
app.UseMiddleware<DevOpsTaskApp.WebAPI.Middleware.ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
