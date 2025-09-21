using CoDodoApi;
using CoDodoApi.BackendServices;
using CoDodoApi.Converters;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.AddConfiguredSerilog();

IServiceCollection services = builder.Services;

services.AddSingleton(TimeProvider.System);
services.AddSingleton<ProcessInMemoryStore>();
services.AddScoped<ExcelImporter>();
services.AddTransient<IProcessConverter, ProcessConverter>();

services.AddConfiguredAuthentication();
services.AddAuthorization();

services.AddSwagger();

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();
app.MapAllRoutes();

// Order is important here
app.UseAuthentication(); // First verify identity of user
app.UseAuthorization(); // Then verify the access rights of the user

app.Run();