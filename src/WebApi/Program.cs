using Application;

using Infrastructure;

using SharedKernel;

using WebApi;
using WebApi.Common.Middleware;

using Carter;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json")
    .AddEnvironmentVariables();

builder.Services
    .AddSharedKernel()
    .AddApplication()
    .AddInfrastructure(builder.Configuration, builder.Environment)
    .AddPresentation(builder.Configuration);

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.UseSerilogRequestLogging();

app.UseAuthentication();

app.UseAuthorization();

app.UseExceptionHandler(GlobalExceptionHandler.BaseEndpoint);

app.MapCarter();

app.Run();

/// <summary>
/// Defines the program entry point.
/// </summary>
public partial class Program;
