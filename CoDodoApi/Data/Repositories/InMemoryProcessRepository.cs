using System.Collections.Concurrent;
using CoDodoApi.Entities;

namespace CoDodoApi.Data.Repositories;

public sealed class InMemoryProcessRepository : IProcessRepository
{
    private readonly ConcurrentDictionary<(string, string), Process> keyedProcesses = [];

    public Task<Process?> GetProcessByKey((string name, string opportunityUriForAssignment) key)
    {
        if (keyedProcesses.TryGetValue(key, out Process? process))
            return Task.FromResult<Process?>(process);
        else
            return Task.FromResult<Process?>(null);
    }

    public Task<List<Process>> GetProcesses()
    {
        ICollection<Process> v = keyedProcesses.Values;

        return Task.FromResult<List<Process>>([.. v]);
    }

    public Task<Process> CreateProcess(Process process)
    {
        return CreateOrUpdateProcess(process);
    }

    public Task<Process> UpdateProcess(Process process)
    {
        return CreateOrUpdateProcess(process);
    }

    private Task<Process> CreateOrUpdateProcess(Process process)
    {
        var key = process.Key();

        keyedProcesses.AddOrUpdate(key, _ => process, (_, _) => process);

        return Task.FromResult(process);
    }

    public Task<Process?> DeleteProcessByKey((string name, string opportunityUriForAssignment) key)
    {
        keyedProcesses.Remove(key, out var removedProcess);

        return Task.FromResult(removedProcess);
    }
}