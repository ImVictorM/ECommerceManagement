using Application;
using Infrastructure;
using WebApi.Endpoints;
using WebApi;

using Carter;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json")
    .AddEnvironmentVariables();

builder.Services
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

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseExceptionHandler(ErrorEndpoints.BaseEndpoint);

app.MapCarter();

app.Run();

/// <summary>
/// Defines the program entry point.
/// </summary>
public partial class Program;
