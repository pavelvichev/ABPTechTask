using Application.Results;
using Microsoft.EntityFrameworkCore;
using Persistance;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(FindResult.Handler).Assembly));

builder.Services.AddDbContext<AbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetService<AbContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedData(context);
}
catch (Exception exception)
{
    var logger = services.GetService<ILogger<Program>>();
    logger.LogError(exception, "An error occured");
}

app.Run();
