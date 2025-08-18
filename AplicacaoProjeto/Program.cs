using AplicacaoProjeto.AppConfig;
using Serilog.Sinks.MSSqlServer;
using Serilog;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using DataAccess.Repositorys;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomSwagger(builder.Configuration);
builder.Services.AddDependencyInjection(builder.Configuration);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.MSSqlServer(
        connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
        sinkOptions: new MSSqlServerSinkOptions
        {
            TableName = "Logs",
            AutoCreateSqlTable = true
        })
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Configuration.AddUserSecrets<Program>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCustomSwagger();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();
