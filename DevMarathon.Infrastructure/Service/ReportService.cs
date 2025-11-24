using DevMarathon.Application.Contract.Services;
using DevMarathon.Infrastructure.ServiceImpls.ReportHelper;

namespace DevMarathon.Infrastructure.Service;

public class ReportService:IReportService
{
    private StimulsoftHelper _stimulsoftHelper;

    public ReportService()
    {
        _stimulsoftHelper = new StimulsoftHelper();
    }

    public byte[] SamplePDFReport()
    {
        string json = "{}";
        var report= _stimulsoftHelper.GenerateReport(Environment.CurrentDirectory + @"/reports/sample.mrt",json);
        return report;
    }

   
}