using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services for controllers
builder.Services.AddControllers(); // Add this line
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()   // Allow all origins
              .AllowAnyMethod()   // Allow all HTTP methods
              .AllowAnyHeader();  // Allow all headers
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAll");

app.UseHttpsRedirection();

// Global exception handling middleware
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        await context.Response.WriteAsync("An error occurred: " + ex.Message);
    }
});

// Map controller routes
app.MapControllers(); // This is essential for controller-based endpoints

app.Run();
