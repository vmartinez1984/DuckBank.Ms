using DuckBank.Api.Helpers;
using DuckBank.Api.Persistencia;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<AhorroRepositorio>();
//HttpLogger
builder.Services.AddScoped<HttpLogger>();
builder.Services.AddScoped<HttpLoggerRepository>();
//RequestResponse
builder.Services.AddTransient<RequestResponseRepository>();
//HttpClientFactory
builder.Services.AddHttpClient(string.Empty, client => { }).RemoveAllLoggers().AddLogger<HttpLogger>();
//Serilog
builder.Services.AddLogging(logger => logger.AddSerilog());

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
    // using System.Reflection;
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

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI(x =>
    {
        x.SwaggerEndpoint("/swagger/v1/swagger.json", "/swagger/v1/swagger.json");
        x.RoutePrefix = "";
    });
//}

app.UseMiddleware<RequestResponseMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
