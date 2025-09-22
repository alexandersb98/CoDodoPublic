using CoDodoApi.BackendServices;
using CoDodoApi.Converters;
using CoDodoApi.Data.Repositories;
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
        IProcessRepository processRepository,
        TimeProvider provider,
        IProcessConverter processConverter,
        ILoggerFactory logger)
    {
        try
        {
            var process = dto.ToProcess(provider);

            var r = await processRepository.DeleteProcessByKey(process.Key()).ConfigureAwait(false);

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
        IProcessRepository processRepository,
        IProcessConverter processConverter,
        ILoggerFactory logger)
    {
        try
        {
            var process = processConverter.ConvertToEntity(dto);

            var r = await processRepository.CreateProcess(process);

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
        IProcessRepository processRepository,
        ILoggerFactory logger,
        HttpContext context)
    {
        try
        {
            var r = await processRepository.GetProcesses().ConfigureAwait(false);

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

    static IResult OkProcessDto(Process? process, IProcessConverter processConverter)
    {
        if (process is null) return TypedResults.NoContent();

        var dto = processConverter.ConvertToDto(process);

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