namespace CoDodoApi.Entities;

public sealed class DeleteProcessDTO
{
    public string Name { get; set; } = "";
    public string UriForAssignment { get; set; } = "";
}

public static class DeleteProcessDtoExtensions
{
    public static 
    Process ToProcess(this DeleteProcessDTO dto, TimeProvider provider)
    {
        Opportunity details = new("", "", "", "", 0);

        return new Process(dto.Name,
                           details,
                           "",
                           provider.GetUtcNow(),
                           provider.GetUtcNow(),
                           provider);
    }
}