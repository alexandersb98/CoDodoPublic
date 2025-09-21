using CoDodoApi.Entities;

namespace CoDodoApi.Services.DTOs;

public sealed class DeleteProcessDTO
{
    public string Name { get; set; } = "";
    public string UriForAssignment { get; set; } = ""; // todo: determine if/how to use /ASB
}

// todo: why does this exist? we only need to identify the process, not create it, in a delete operation /ASB
// todo: consider removing /ASB
public static class DeleteProcessDtoExtensions
{
    public static Process ToProcess(this DeleteProcessDTO dto, TimeProvider provider)
    {
        Opportunity details = new(
            uriForAssignment: "", 
            company: "", 
            capability: "", 
            nameOfSalesLead: "", 
            hourlyRateInSEK: 0);

        return new Process(
            name: dto.Name,
            opportunity: details,
            status: "",
            createdDate: provider.GetUtcNow(),
            updatedDate: provider.GetUtcNow());
    }
}