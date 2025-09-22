using CoDodoApi;
using CoDodoApi.BackendServices;
using CoDodoApi.Converters;
using CoDodoApi.Data;
using CoDodoApi.Data.Repositories;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.AddConfiguredSerilog();

IServiceCollection services = builder.Services;

services.AddSingleton(TimeProvider.System);
services.AddScoped<ExcelImporter>();

var dataStorage = builder.Configuration.GetSection("DataStorage").Value?.ToUpper();
switch (dataStorage)
{
    case "INMEMORY":
        services.AddSingleton<IProcessRepository, InMemoryProcessRepository>();
        break;
    case "DATABASE":
        services.AddScoped<IProcessRepository, ProcessRepository>();
        break;
    default:
        throw new ArgumentException($"'{dataStorage}' is not a valid value for DataStorage");
}

services.AddScoped<IProcessRepository, ProcessRepository>();
services.AddTransient<IProcessConverter, ProcessConverter>();

services.AddConfiguredAuthentication();
services.AddAuthorization();

services.AddSwagger();

services.AddConfiguredDatabase(builder.Configuration);

WebApplication app = builder.Build();

if (dataStorage == "DATABASE")
{
    // Run the migrations on the database
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();
    }
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();
app.MapAllRoutes();

// Order is important here
app.UseAuthentication(); // First verify identity of user
app.UseAuthorization(); // Then verify the access rights of the user

app.Run();