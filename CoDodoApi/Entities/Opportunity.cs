using System.ComponentModel.DataAnnotations;

namespace CoDodoApi.Entities;

// todo: consider using record here /ASB
public sealed class Opportunity
{
    [Key] public string UriForAssignment { get; set; } = "";
    public string Company { get; set; } = "";
    public string Capability { get; set; } = "";
    public string NameOfSalesLead { get; set; } = "";
    public int HourlyRateInSEK { get; set; }

    public Opportunity(
        string uriForAssignment,
        string company,
        string capability,
        string nameOfSalesLead,
        int hourlyRateInSEK)
    {
        UriForAssignment = uriForAssignment;
        Company = company;
        Capability = capability;
        NameOfSalesLead = nameOfSalesLead;
        HourlyRateInSEK = hourlyRateInSEK;
    }
}