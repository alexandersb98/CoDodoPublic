namespace CoDodoApi;

public static class MiddlewareExtensions
{
    public static WebApplication MapAllRoutes(this WebApplication app)
    {
        RouteGroupBuilder api = app.MapGroup("/api");

        api.MapPost("/ImportExcel", Endpoints.ImportExcel)
            .DisableAntiforgery()
            .RequireAuthorization();

        RouteGroupBuilder processes = api.MapGroup("/processes");

        processes.MapGet("", Endpoints.GetAllProcesses);
            //.RequireAuthorization(); // this is turned off for now to allow MCP to call it without auth

        processes.MapPost("", Endpoints.CreateProcess)
            .RequireAuthorization();

        processes.MapDelete("", Endpoints.DeleteProcess)
            .RequireAuthorization();

        return app;
    }
}