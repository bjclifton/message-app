using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using server.Data;
using server.Services;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder
            .WithOrigins("http://localhost:5173") // Change to your front-end URL
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

// Add vaultsettings.json
builder.Configuration.AddJsonFile("vaultsettings.json", optional: true, reloadOnChange: true);

// Add JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JWT:Secret"])),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });


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

// Use CORS
app.UseCors("AllowSpecificOrigin");


app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization(); // Enable authorization
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // Map controllers
});

app.Run();

