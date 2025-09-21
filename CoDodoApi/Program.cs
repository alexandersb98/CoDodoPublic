using CoDodoApi;
using CoDodoApi.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.AddConfiguredSerilog();

IServiceCollection services = builder.Services;
services.AddSwagger();
services.AddSingleton(TimeProvider.System);
services.AddSingleton<ProcessInMemoryStore>();
services.AddScoped<ExcelImporter>();
services.AddConfiguredAuthentication();
services.AddAuthorization();

WebApplication app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();
app.MapAllRoutes();

// Order is important here
app.UseAuthentication(); // First verify identity of user
app.UseAuthorization(); // Then verify the access rights of the user

app.Run();