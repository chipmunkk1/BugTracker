using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// 1. Turn on the Visual Swagger Dashboard tools
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. Grab the password and map from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 3. Give the server a fresh PostgreSQL connection for every user request
builder.Services.AddScoped<Npgsql.NpgsqlConnection>(provider => new Npgsql.NpgsqlConnection(connectionString));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // Allows your HTML file to connect
              .AllowAnyMethod()  // Allows POST, GET, PUT, DELETE
              .AllowAnyHeader(); // Allows any headers
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // 4. Actually display the Swagger UI webpage when you run the app
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.Run();