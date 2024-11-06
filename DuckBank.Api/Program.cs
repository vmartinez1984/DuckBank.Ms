using DuckBank.Api.Persistencia;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Serilog;
using VMtz84.Logger;

var builder = WebApplication.CreateBuilder(args);
// Configura Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)  // Lee configuración desde appsettings.json
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

// Reemplaza el logger predeterminado por Serilog
builder.Host.UseSerilog();
//Muestra el error de serilog
//SelfLog.Enable(Console.Error);

// Add services to the container.
builder.Services.AddScoped<AhorroRepositorio>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v3.0",
        Title = "Ahorros Duckbank",
        Description = @"Cuentas de ahorro",
        Contact = new OpenApiContact
        {
            Name = "Víctor Martínez",
            Url = new Uri("mailto:ahal_tocob@hotmail.com")
        }
    });    
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("*")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithExposedHeaders("*");
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(x =>
{
    x.SwaggerEndpoint("/swagger/v1/swagger.json", "/swagger/v1/swagger.json");
    x.RoutePrefix = string.Empty;
});

app.UseMiddleware<RequestResponseMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// Asegúrate de cerrar el logger al final del programa
Log.CloseAndFlush();