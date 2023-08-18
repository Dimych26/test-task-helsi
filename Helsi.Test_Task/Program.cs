
using Helsi.Test_Task.Infrastructure.Configuration;
using Helsi.Test_Task.Infrastructure.Swagger;
using Helsi.Test_Task.Infrastructure.Utils;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<UserIdHeaderFilter>();
});

EnvironmentVariablesConfiguration.AddEnvironmentVariablesFromConfig();

builder.Services.AddHelsiCommonServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
