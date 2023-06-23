using API.Extensions;
using API.Middleware;
using Infrastructue.Data;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseMiddleware<ExceptionMiddleware>();

app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseSwagger();
app.UseSwaggerUI();
//Serving static content from the API
app.UseStaticFiles();

app.UseCors("CorePolicy");

app.UseAuthorization();

app.MapControllers();
#region Applying the migrations and creating the database at startup
using var scope = app.Services.CreateScope();
var servuces = scope.ServiceProvider;
var context = servuces.GetService<StoreContext>();
var logger = servuces.GetService<ILogger<Program>>();
try
{
    await context.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(context);
}
catch(Exception ex)
{
    logger.LogError(ex, "An error occured during migration");
}
#endregion
app.Run();
