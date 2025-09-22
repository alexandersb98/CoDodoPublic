using CoDodoApi.Entities;

namespace CoDodoApi.Data.Repositories;

public interface IProcessRepository
{
    Task<Process?> GetProcessByKey((string name, string opportunityUriForAssignment) key);
    Task<List<Process>> GetProcesses();
    Task<Process> CreateProcess(Process process);
    Task<Process> UpdateProcess(Process process);
    Task<Process?> DeleteProcessByKey((string name, string opportunityUriForAssignment) key);
}