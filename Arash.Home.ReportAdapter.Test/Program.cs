using Arash.Home.DynamicReports.DbTest.Database;
using Arash.Home.ExcelGenerator.ExcelGenerator;
using Arash.Home.ExcelGenerator.ExcelGenerator.AdapterOptions;
using Arash.Home.QueryGenerator.ConsoleTest.DataBase;
using Arash.Home.QueryGenerator.Services.Implementation;
using Arash.Home.QueryGenerator.Services.ViewModels;
using Arash.Home.ReportAdapter;
using Arash.Home.ReportAdapter.ReportAdapterModule.Implementation;
using Arash.Home.ReportAdapter.ReportAdapterModule.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

internal class Program
{
    private static void Main(string[] args)
    {
        var db = new TestDb();
        IServiceCollection services = new ServiceCollection();
        services.AddScoped<TestDb>();
        services.AddScoped<DbContext, TestDb>();
        services.ConfigureReportManager(adapters: new List<AdapterBase>
        {
            new ToPersianDate()
        });
        IReportAdapterService reportAdapter = services.BuildServiceProvider().GetRequiredService<IReportAdapterService>();
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
                        DisplayName="Date",
                        FieldName="Date",
                        IsMapped=false
                    },
                },
                    TableName = "Post",
                    Dependencies = new List<Arash.Home.QueryGenerator.Services.ViewModels.QueryDependencyVm>
                {
                    new Arash.Home.QueryGenerator.Services.ViewModels.QueryDependencyVm
                    {
                        Name = "FK_Post_Category_CategoryId"
                    }
                },
                    GroupBy = new QueryFieldVm
                    {
                        DisplayName = "Title"
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