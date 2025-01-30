namespace CoDodoApi.Entities;

public sealed
class CreateProcessDTO
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
    public static
    Process ToProcess(this CreateProcessDTO dto, TimeProvider provider)
    {
        Opportunity o = new(dto.UriForAssignment,
                                     dto.Company,
                                     dto.Capability,
                                     dto.NameOfSalesLead,
                                     dto.HourlyRateInSEK);

        return new Process(dto.Name,
                           o,
                           dto.Status,
                           provider.GetUtcNow(),
                           provider.GetUtcNow(),
                           provider);
    }
}