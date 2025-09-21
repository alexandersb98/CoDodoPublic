using ClosedXML.Excel;
using CoDodoApi.Entities;

namespace CoDodoApi.BackendServices;

public sealed class ExcelImporter(
    ProcessInMemoryStore processStore,
    ILogger<ExcelImporter> logger)
{

    public void Import(IFormFile file)
    {
        try
        {
            Stream readStream = file.OpenReadStream();

            using XLWorkbook wb = new(readStream);

            IXLWorksheet ws = wb.Worksheet(1);

            IXLRows rows = ws.Rows();

            IEnumerable<Process> processes = rows
                .Skip(1)
                .TakeWhile(x => !x.Cell(1).IsEmpty())
                .Select(RowToProcess);

            _ = processes.Select(processStore.Add).ToArray();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Exception in {nameof(ExcelImporter)}");
            throw ex;
        }
    }

    private Process RowToProcess(IXLRow row)
    {
        IXLCell NAME = row.Cell(1);
        IXLCell CAPABILITY = row.Cell(2);
        IXLCell OPPORTUNITY = row.Cell(3);
        IXLCell STATUS = row.Cell(4);
        IXLCell SALESLEAD = row.Cell(5);
        IXLCell HOURLYRATE = row.Cell(6);
        IXLCell LASTUPDATE = row.Cell(7);
        IXLCell GENERATIONDATE = row.Cell(8);

        var name = NAME.GetValue<string>();
        var capability = CAPABILITY.GetValue<string>();
        var company = OPPORTUNITY.GetValue<string>();
        var status = STATUS.GetValue<string>();
        var salesLead = SALESLEAD.GetValue<string>();
        HOURLYRATE.TryGetValue(out int hourlyRate);
        var lu = LASTUPDATE.GetValue<string>();
        var gd = GENERATIONDATE.GetValue<string>();

        DateTime lastUpdate = DateTime.Parse(lu);
        DateTime generationDate = DateTime.Parse(gd);

        var uri = Guid.NewGuid().ToString();

        Opportunity opportunity = new(
            uriForAssignment: uri,
            company: company,
            capability: capability,
            nameOfSalesLead: salesLead,
            hourlyRate);

        return new Process(
            name: name,
            opportunity: opportunity,
            status: status,
            createdDate: generationDate,
            updatedDate: lastUpdate);
    }
}
