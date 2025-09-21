using CoDodoApi.Entities;

namespace CoDodoApi.Services.DTOs;

public sealed class ProcessDTO
{
    public ProcessDTO(
        string name,
        string uriForAssignment,
        string company,
        string capability,
        string status,
        string nameOfSalesLead,
        int hourlyRateInSEK,
        DateTimeOffset updatedDate,
        DateTimeOffset createdDate,
        int daysSinceUpdate,
        int daysSinceCreation)
    {
        Name = name;
        UriForAssignment = uriForAssignment;
        Company = company;
        Capability = capability;
        Status = status;
        NameOfSalesLead = nameOfSalesLead;
        HourlyRateInSEK = hourlyRateInSEK;
        UpdatedDate = updatedDate;
        CreatedDate = createdDate;
        DaysSinceUpdate = daysSinceUpdate;
        DaysSinceCreation = daysSinceCreation;
    }

    public string Name { get; set; } = "";
    public string UriForAssignment { get; set; } = "";
    public string Company { get; set; } = "";
    public string Capability { get; set; } = "";
    public string Status { get; set; } = "";
    public string NameOfSalesLead { get; set; } = "";
    public int HourlyRateInSEK { get; set; }
    public DateTimeOffset UpdatedDate { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    
    // todo: determine why these are required, they seem redundant because they can be computed using other properties of this DTO /ASB
    public int DaysSinceUpdate { get; set; }// Freshness
    public int DaysSinceCreation { get; set; } // Age
}

public static class ProcessDtoExtensions
{
    public static Process ToProcess(this ProcessDTO dto, TimeProvider provider)
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
            createdDate: dto.CreatedDate,
            updatedDate: dto.UpdatedDate);
    }
}