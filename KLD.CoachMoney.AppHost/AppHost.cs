var builder = DistributedApplication.CreateBuilder(args);

var database = builder.AddConnectionString("local");

builder.AddProject<Projects.KLD_CoachMoney_Web>("api")
    .WithReference(database);

builder.Build().Run();
