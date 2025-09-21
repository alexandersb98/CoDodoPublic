using CoDodoApi.Entities;
using System.Collections.Concurrent;

namespace CoDodoApi.Services;

public sealed
class ProcessInMemoryStore()
{
    readonly ConcurrentDictionary<string, Process> keyedProcesses = [];

    public Task<Process> Add(Process process)
    {
        string key = process.Key();

        keyedProcesses.AddOrUpdate(key, _ => process, (a, b) => process);

        return Task.FromResult(process);
    }

    public Task<Process> Delete(Process process)
    {
        string key = process.Key();

        keyedProcesses.Remove(key, out _);

        return Task.FromResult(process);
    }

    public Task<Process?> GetFromKey(string key)
    {
        if (keyedProcesses.TryGetValue(key, out Process? process))
            return Task.FromResult<Process?>(process);
        else
            return Task.FromResult<Process?>(null);
    }

    public Task<Process[]> GetAll()
    {
        ICollection<Process> v = keyedProcesses.Values;

        return Task.FromResult<Process[]>([.. v]);
    }
}