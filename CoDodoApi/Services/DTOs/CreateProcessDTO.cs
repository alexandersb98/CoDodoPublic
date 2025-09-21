using CoDodoApi.Entities;

namespace CoDodoApi.Services.DTOs;

public sealed class CreateProcessDTO
{
    public string Name { get; set; } = "";
    public string UriForAssignment {get; set;} = "";
    public string Company {get; set;} = "";
    public string Capability {get; set;} = "";
    public string Opportunity {get; set;} = "";
    public string Status {get; set;} = "";
    public string NameOfSalesLead {get; set;} = "";
    public int HourlyRateInSEK {get; set;}
    public string Notes { get; set; } = "";
}

public static class CreateProcessDtoExtensions
{
    public static Process ToProcess(this CreateProcessDTO dto, TimeProvider provider)
    {
        Opportunity o = new(
            uriForAssignment: dto.UriForAssignment,
            company: dto.Company,
            capability: dto.Capability,
            nameOfSalesLead: dto.NameOfSalesLead,
            hourlyRateInSEK: dto.HourlyRateInSEK);

        return new Process(
            name: dto.Name,
            opportunity: o,
            status: dto.Status,
            createdDate: provider.GetUtcNow(),
            updatedDate: provider.GetUtcNow(),
            provider: provider);
    }
}