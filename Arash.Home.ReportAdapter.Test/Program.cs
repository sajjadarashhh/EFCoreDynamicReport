using Arash.Home.DynamicReports.DbTest.Database;
using Arash.Home.ExcelGenerator.ExcelGenerator;
using Arash.Home.QueryGenerator.ConsoleTest.DataBase;
using Arash.Home.QueryGenerator.Services.Implementation;
using Arash.Home.ReportAdapter.ReportAdapterModule.Implementation;
using Arash.Home.ReportAdapter.ReportAdapterModule.Messaging;
using Newtonsoft.Json;

internal class Program
{
    private static void Main(string[] args)
    {
        var db = new TestDb();
        IReportAdapterService reportAdapter = new ReportAdapterService(new QueryGeneratorService(db), new ExcelGenerator(), db,_adapters: new List<Arash.Home.ExcelGenerator.ExcelGenerator.AdapterOptions.AdapterBase> { });
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
                        FieldName="Name",
                        DisplayName = "Title"
                    },
                    new Arash.Home.QueryGenerator.Services.ViewModels.QueryFieldVm
                    {
                        FieldName="Content",
                        DisplayName = "Content"
                    },
                    new Arash.Home.QueryGenerator.Services.ViewModels.QueryFieldVm
                    {
                        DependecyName="FK_Post_Category_CategoryId",
                        DisplayName="Category",
                        FieldName="Title",
                    },
                    new Arash.Home.QueryGenerator.Services.ViewModels.QueryFieldVm
                    {
                        FieldName = "Date",
                        DisplayName="تاریخ",
                        CalculatorNames=new List<string>(){ "to-persian-date" }
                    }
                },
                    TableName = "Post",
                    Dependencies = new List<Arash.Home.QueryGenerator.Services.ViewModels.QueryDependencyVm>
                {
                    new Arash.Home.QueryGenerator.Services.ViewModels.QueryDependencyVm
                    {
                        Name = "FK_Post_Category_CategoryId"
                    }
                }
                },
                FilePath = Path.Combine(Directory.GetCurrentDirectory(), "sajjadarash.xlsx")
            }
        }).Result;
        var resultQuery = reportAdapter.GetData(new ReportGetDataRequest
        {
            Entity = new Arash.Home.QueryGenerator.Services.ViewModels.QueryVm
            {
                Fields = new List<Arash.Home.QueryGenerator.Services.ViewModels.QueryFieldVm>
                {
                    new Arash.Home.QueryGenerator.Services.ViewModels.QueryFieldVm
                    {
                        FieldName="Name",
                        DisplayName = "Title"
                    },
                    new Arash.Home.QueryGenerator.Services.ViewModels.QueryFieldVm
                    {
                        FieldName="Content",
                        DisplayName = "Content"
                    },
                    new Arash.Home.QueryGenerator.Services.ViewModels.QueryFieldVm
                    {
                        DependecyName="FK_Post_Category_CategoryId",
                        DisplayName="Category",
                        FieldName="Title",
                    }

                },
                TableName = "Post",
                Dependencies = new List<Arash.Home.QueryGenerator.Services.ViewModels.QueryDependencyVm>
                {
                    new Arash.Home.QueryGenerator.Services.ViewModels.QueryDependencyVm
                    {
                        Name = "FK_Post_Category_CategoryId"
                    }
                }
            }
        }).Result;
        if (result.IsSuccess)
            Console.WriteLine("Operation Success.");
        if (resultQuery.IsSuccess)
            Console.WriteLine(JsonConvert.SerializeObject(resultQuery.Entity));
        else
        {
            Console.WriteLine(result.Message);
        }
    }
}