using CoDodoApi.Entities;
using CoDodoApi.Services.DTOs;

namespace CoDodoApi.Converters;

public interface IProcessConverter
{
    ProcessDTO ConvertToDto(Process process);
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
            daysSinceUpdate: DaysSinceUpdate(p),
            daysSinceCreation: DaysSinceCreation(p));
    }

    private int DaysSinceUpdate(Process process)
    {
        var diff = this.timeProvider.GetUtcNow() - process.UpdatedDate;

        return NumberOfWholeDays(diff);
    }

    private int DaysSinceCreation(Process process)
    {
        var diff = this.timeProvider.GetUtcNow() - process.CreatedDate;

        return NumberOfWholeDays(diff);
    }

    private static int NumberOfWholeDays(TimeSpan diff)
    {
        double numberOfDays = diff.TotalDays;

        return (int)numberOfDays;
    }
}