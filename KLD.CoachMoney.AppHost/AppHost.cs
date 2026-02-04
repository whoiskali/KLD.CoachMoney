using Aspire.Hosting;
using Microsoft.VisualBasic;

var builder = DistributedApplication.CreateBuilder(args);

var sqlPassword = builder.AddParameterFromConfiguration(
    "SqlServerPassword",
    "Parameters:SqlServerPassword",
    secret: true);

var sql = builder.AddSqlServer("sqlserver", port: 55555)
    .WithPassword(sqlPassword)
    .WithEnvironment("ACCEPT_EULA", "Y")
    .WithLifetime(ContainerLifetime.Persistent)
    .AddDatabase("CoachMoneyDb");

//var database = builder.AddConnectionString("local");

var aiApiKey = builder.AddParameterFromConfiguration(
    "FinancialAiApiKey",
    "FinancialAi:ApiKey",
    secret: true);

var aiModel = builder.AddParameterFromConfiguration(
    "FinancialAiModel",
    "FinancialAi:Model");

var aiMaxTokens = builder.AddParameterFromConfiguration(
    "FinancialAiMaxTokens",
    "FinancialAi:MaxTokens");


var jwtIssuer = builder.AddParameterFromConfiguration(
    "JwtIssuer",
    "Jwt:Issuer");

var jwtAudience = builder.AddParameterFromConfiguration(
    "JwtAudience",
    "Jwt:Audience");

var jwtKey = builder.AddParameterFromConfiguration(
    "JwtKey",
    "Jwt:Key",
    secret: true);

var jwtExpiry = builder.AddParameterFromConfiguration(
    "JwtExpiryMinutes",
    "Jwt:ExpiryMinutes");

builder.AddProject<Projects.KLD_CoachMoney_Web>("api")
    .WithReference(sql)
    .WithEnvironment("FinancialAi__ApiKey", aiApiKey)
    .WithEnvironment("FinancialAi__Model", aiModel)
    .WithEnvironment("FinancialAi__MaxTokens", aiMaxTokens)
    .WithEnvironment("Jwt__Issuer", jwtIssuer)
    .WithEnvironment("Jwt__Audience", jwtAudience)
    .WithEnvironment("Jwt__Key", jwtKey)
    .WithEnvironment("Jwt__ExpiryMinutes", jwtExpiry);


builder.Build().Run();
