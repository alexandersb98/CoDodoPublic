using CoDodoApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoDodoApi.Data.Repositories;

public sealed class ProcessRepository(AppDbContext dbContext) : IProcessRepository
{
    public async Task<Process?> GetProcessByKey((string name, string opportunityUriForAssignment) key)
    {
        return await dbContext.Processes.SingleOrDefaultAsync(p => 
            p.Name == key.name 
            && p.Opportunity.UriForAssignment == key.opportunityUriForAssignment);
    }

    public async Task<List<Process>> GetProcesses()
    {
        return await dbContext.Processes.ToListAsync();
    }

    public async Task<Process> CreateProcess(Process process)
    {
        var processIdIsTaken = await dbContext.Processes.AnyAsync(p => p.ID == process.ID);
        if (processIdIsTaken)
            throw new InvalidOperationException("The provided process ID is already in the database. Choose a different ID or update the process instead.");

        var createdProcess = (await dbContext.Processes.AddAsync(process)).Entity;
        await dbContext.SaveChangesAsync();

        return createdProcess;
    }

    public async Task<Process> UpdateProcess(Process process)
    {
        var numberOfMatchingProcesses = await dbContext.Processes.CountAsync(p => p.ID == process.ID);
        if (numberOfMatchingProcesses == 0)
            throw new InvalidOperationException("The provided process ID does not belong to any existing process in the database. Choose a different ID or create the process instead.");

        var updatedProcess = dbContext.Processes.Update(process).Entity;
        await dbContext.SaveChangesAsync();

        return updatedProcess;
    }

    public async Task<Process?> DeleteProcessByKey((string name, string opportunityUriForAssignment) key)
    {
        var process = await dbContext.Processes.SingleOrDefaultAsync(p => 
            p.Name == key.name 
            && p.Opportunity.UriForAssignment == key.opportunityUriForAssignment);

        if (process is null)
            return null;

        var removedProcess = (dbContext.Processes.Remove(process)).Entity;
        await dbContext.SaveChangesAsync();

        return removedProcess;
    }
}