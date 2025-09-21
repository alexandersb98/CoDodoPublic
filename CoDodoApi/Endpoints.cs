using CoDodoApi.BackendServices;
using CoDodoApi.Converters;
using CoDodoApi.Entities;
using CoDodoApi.Services.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CoDodoApi;

// todo: move to Services /ASB
// todo: refactor to use minimal API or controllers /ASB
public static class Endpoints
{
    public static async Task<IResult> DeleteProcess(
        [FromBody] DeleteProcessDTO dto,
        ProcessInMemoryStore store,
        TimeProvider provider,
        IProcessConverter processConverter,
        ILoggerFactory logger)
    {
        try
        {
            Process process = dto.ToProcess(provider);

            Process r = await store.Delete(process).ConfigureAwait(false);

            return OkProcessDto(r, processConverter);
        }
        catch (Exception ex)
        {
            logger.CreateLogger(nameof(Endpoints))
                .LogWarning($"Exception in {nameof(DeleteProcess)}: {ex.Message}");
            return TypedResults.Problem(ex.Message);
        }
    }

    public static async Task<IResult> CreateProcess(
        CreateProcessDTO dto,
        ProcessInMemoryStore store,
        IProcessConverter processConverter,
        ILoggerFactory logger)
    {
        try
        {
            Process process = processConverter.ConvertToEntity(dto);

            Process r = await store.Add(process);

            return OkProcessDto(r, processConverter);
        }
        catch (Exception ex)
        {
            logger.CreateLogger(nameof(Endpoints))
                .LogWarning($"Exception in {nameof(CreateProcess)}: {ex.Message}");
            return TypedResults.Problem(ex.Message);
        }
    }

    public static async Task GetAllProcesses(
        ProcessInMemoryStore store,
        ILoggerFactory logger,
        HttpContext context)
    {
        try
        {
            Process[] r = await store.GetAll().ConfigureAwait(false);

            context.Response.StatusCode = 200;

            await context.Response.WriteAsJsonAsync(r);
        }
        catch (Exception ex)
        {
            logger.CreateLogger(nameof(Endpoints))
                .LogWarning($"Exception in {nameof(GetAllProcesses)}: {ex.Message}");

            context.Response.StatusCode = 500;
        }
    }

    static IResult OkProcessDto(Process process, IProcessConverter processConverter)
    {
        ProcessDTO dto = processConverter.ConvertToDto(process);

        return TypedResults.Ok(dto);
    }

    static IResult OkProcessesDto(Process[] processes, IProcessConverter processConverter)
    {
        ProcessDTO[] dtos = processes
            .Select(processConverter.ConvertToDto)
            .ToArray();

        return TypedResults.Ok(dtos);
    }

    public static IResult ImportExcel(IFormFile file, ExcelImporter importer)
    {
        try
        {
            importer.Import(file);

            return Results.Ok();
        }
        catch
        {
            return Results.Problem("Failed to import excel file.");
        }
    }
}