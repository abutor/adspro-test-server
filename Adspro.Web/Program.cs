using Adspro.Providers;
using Adspro.Web.Middlewares;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Data;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

var sqlConnectionString = builder.Configuration["SQL_CS"] ?? throw new ApplicationException("SQL_CS is required");
var redisConnectionString = builder.Configuration["REDIS_CS"] ?? throw new ApplicationException("REDIS_CS is required");
var redisInstanceName = builder.Configuration["REDIS_INSTANCE_NAME"] ?? throw new ApplicationException("REDIS_INSTANCE_NAME is required");
var tokenSecret = builder.Configuration["TOKEN_ENCRYPTION_KEY"] ?? throw new ApplicationException("TOKEN_ENCRYPTION_KEY is required");

builder.Services.AddProviders();
builder.Services.AddSingleton<AuthorizeFilter>();
builder.Services.AddSingleton<AuthMiddleware>();

builder.Services.AddControllers();
builder.Services.AddScoped<IDbConnection>(c => new SqlConnection(sqlConnectionString));

builder.Services.AddStackExchangeRedisCache(c =>
{
    c.Configuration = redisConnectionString;
    c.InstanceName = redisInstanceName;
});

var app = builder.Build();

app.UseMiddleware<AuthMiddleware>();
app.MapControllers();

app.Run();
