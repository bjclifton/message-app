using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using server.Data;
using server.Services;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add vaultsettings.json
builder.Configuration.AddJsonFile("vaultsettings.json", optional: true, reloadOnChange: true);

// Configure MongoDB settings
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDB"));

// Add services to the container
builder.Services.AddSingleton<UserService>();
builder.Services.AddControllers(); // Add controllers

// Configure Swagger/OpenAPI
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Your API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "server v1");
        c.RoutePrefix = string.Empty; // Serve Swagger UI at the app's root
    });
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization(); // Enable authorization
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // Map controllers
});

app.Run();

