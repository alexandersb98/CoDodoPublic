namespace CoDodoApi.Services.DTOs;

// todo: consider using record here /ASB
public sealed class CreateProcessDTO
{
    public string Name { get; set; } = "";
    public string UriForAssignment {get; set;} = "";
    public string Company {get; set;} = "";
    public string Capability {get; set;} = "";
    public string Opportunity {get; set;} = ""; // todo: why string /ASB
    public string Status {get; set;} = "";
    public string NameOfSalesLead {get; set;} = "";
    public int HourlyRateInSEK {get; set;}
    public string Notes { get; set; } = ""; // todo: determine if needed /ASB
}
