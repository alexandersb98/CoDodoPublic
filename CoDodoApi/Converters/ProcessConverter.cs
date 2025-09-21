using CoDodoApi.Entities;
using CoDodoApi.Services.DTOs;

namespace CoDodoApi.Converters;

public interface IProcessConverter
{
    ProcessDTO ConvertToDto(Process process);
    Process ConvertToEntity(CreateProcessDTO dto);
}

public sealed class ProcessConverter : IProcessConverter
{
    private TimeProvider timeProvider;

    public ProcessConverter(TimeProvider timeProvider)
    {
        this.timeProvider = timeProvider;
    }

    public ProcessDTO ConvertToDto(Process process)
    {
        Process p = process;
        Opportunity d = p.Opportunity;

        var now = this.timeProvider.GetUtcNow();
        var daysSinceUpdate = NumberOfWholeDays(now - p.UpdatedDate);
        var daysSinceCreation = NumberOfWholeDays(now - p.CreatedDate);

        return new ProcessDTO(
            name: p.Name,
            uriForAssignment: d.UriForAssignment,
            company: d.Company,
            capability: d.Capability,
            status: p.Status,
            nameOfSalesLead: d.NameOfSalesLead,
            hourlyRateInSEK: d.HourlyRateInSEK,
            updatedDate: p.UpdatedDate,
            createdDate: p.CreatedDate,
            daysSinceUpdate: daysSinceUpdate,
            daysSinceCreation: daysSinceCreation);
    }


    public Process ConvertToEntity(CreateProcessDTO dto)
    {
        Opportunity o = new(
            uriForAssignment: dto.UriForAssignment,
            company: dto.Company,
            capability: dto.Capability,
            nameOfSalesLead: dto.NameOfSalesLead,
            hourlyRateInSEK: dto.HourlyRateInSEK);

        var now = this.timeProvider.GetUtcNow();

        return new Process(
            name: dto.Name,
            opportunity: o, // todo: consider using dto.Opportunity /ASB
            status: dto.Status,
            createdDate: now,
            updatedDate: now);
    }
    
    private static int NumberOfWholeDays(TimeSpan diff)
    {
        double numberOfDays = diff.TotalDays;

        return (int)numberOfDays;
    }
}