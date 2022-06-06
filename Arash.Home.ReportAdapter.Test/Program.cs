using Arash.Home.DynamicReports.DbTest.Database;
using Arash.Home.ExcelGenerator.ExcelGenerator;
using Arash.Home.QueryGenerator.ConsoleTest.DataBase;
using Arash.Home.QueryGenerator.Services.Implementation;
using Arash.Home.ReportAdapter.ReportAdapterModule.Implementation;

internal class Program
{
    private static void Main(string[] args)
    {
        var db = new TestDb();
        IReportAdapterService reportAdapter = new ReportAdapterService(new QueryGeneratorService(db), new ExcelGenerator(), db);
        var result = reportAdapter.ReportCreate(new Arash.Home.ReportAdapter.ReportAdapterModule.Messaging.ReportCreateRequest
        {
            Entity = new Arash.Home.ReportAdapter.ReportAdapterModule.ViewModels.ReportCreateVm
            {
                QueryGenerateRequest = new Arash.Home.QueryGenerator.Services.ViewModels.QueryVm
                {
                    Fields = new List<Arash.Home.QueryGenerator.Services.ViewModels.QueryFieldVm>
                    {
                        new Arash.Home.QueryGenerator.Services.ViewModels.QueryFieldVm
                        {
                            DisplayName="عنوان",
                            FieldName="Name"
                        },
                        new Arash.Home.QueryGenerator.Services.ViewModels.QueryFieldVm
                        {
                            DisplayName="محتوا",
                            FieldName="Content"
                        }
                    },
                    TableName = "Post"
                }
            }
        }).Result;
        if(result.IsSuccess)
        Console.WriteLine("Operation Success.");
        else
        {
            Console.WriteLine(result.Message);
        }
    }
}