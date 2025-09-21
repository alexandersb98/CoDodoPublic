using System.Text;

namespace CoDodoApi.Entities;

// todo: consider refactor to record /ASB
public sealed class Process
{
    public string Name { get; set; } = "";
    public Opportunity Opportunity { get; set; }
    public string Status { get; set; } = "";
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset UpdatedDate { get; set; }

    public Process(
        string name,
        Opportunity opportunity,
        string status,
        DateTimeOffset createdDate,
        DateTimeOffset updatedDate)
    {
        Name = name;
        Opportunity = opportunity;
        Status = status;
        CreatedDate = createdDate;
        UpdatedDate = updatedDate;
    }

    internal string Key()
    {
        string t = Name + Opportunity.UriForAssignment;

        byte[] b = Encoding.UTF8.GetBytes(t);

        return Convert.ToBase64String(b);
    }

    // todo: determine why not used /ASB
    // todo: consider using enum instead of string /ASB
    internal bool IsWon()
    {
        return Status == "WON";
    }
}