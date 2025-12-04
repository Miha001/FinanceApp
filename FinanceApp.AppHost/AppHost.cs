var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin(pgAdmin => pgAdmin.WithHostPort(5050))
    .WithDataVolume();

var databaseName = "finance_db";
var creationScript = $$"""
    -- Create the database
    CREATE DATABASE {{databaseName}};
    """;

var db = postgres.AddDatabase("postgresdb")
                 .WithCreationScript(creationScript);

builder.AddProject<Projects.Data_MigrationService>("migration")
    .WithReference(db)
    .WaitFor(db);

builder.AddProject<Projects.CurrencyUpdater_Worker>("currency-updater")
    .WithReference(db)
    .WaitFor(db);

builder.AddProject<Projects.Users_API>("users-api")
    .WithReference(db)
    .WaitFor(db);

builder.AddProject<Projects.Finances_API>("finances-api")
    .WithReference(db)
    .WaitFor(db);

builder.AddProject<Projects.Gateway>("gateway");

builder.Build().Run();
