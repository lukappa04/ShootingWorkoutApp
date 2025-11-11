using swbackend.Db;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddShootingWorkoutDbContext(builder.Configuration.GetConnectionString("DefaultConnection")!);

var app = builder.Build();

app.Run();