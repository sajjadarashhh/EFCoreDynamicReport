using Arash.Home.DynamicReports.DbTest.Database;
using Arash.Home.ExcelGenerator.ExcelGenerator;
using Arash.Home.ExcelGenerator.ExcelGenerator.AdapterOptions;
using Arash.Home.QueryGenerator.Abstracts;
using Arash.Home.QueryGenerator.ConsoleTest.DataBase;
using Arash.Home.QueryGenerator.Services.Implementation;
using Arash.Home.QueryGenerator.Services.ViewModels;
using Arash.Home.ReportAdapter;
using Arash.Home.ReportAdapter.ReportAdapterModule.Implementation;
using Arash.Home.ReportAdapter.ReportAdapterModule.Messaging;
using Arash.Home.ReportAdapter.ReportAdapterModule.ViewModels;
using Arash.Home.ReportAdapter.Test.AdapterOptions;
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
            new ToPersianDate(),
            new CheckIsCalculationalValidityWorks()
        });
        IReportAdapterService reportAdapter = services.BuildServiceProvider().GetRequiredService<IReportAdapterService>();
        var result = reportAdapter.ReportCreate(new ReportCreateRequest
        {
            Entity = new ReportCreateVm
            {
                QueryGenerateRequest = new QueryVm
                {
                    Filters = new List<QueryGeneratorFilterModel>
                    {
                        new QueryGeneratorFilterModel
                        {
                            RightField =new QueryValueModel
                            {
                                Value = "تره"
                            },
                            LeftSide=  new QueryFieldsModel
                            {
                                Name = "Name"
                            },
                            Mode = FilterMode.Equal
                        }
                    },
                    Fields = new List<QueryFieldVm>
                    {
                        new QueryFieldVm
                        {
                            FieldName="Name",
                            DisplayName = "Title"
                        },
                        new QueryFieldVm
                        {
                            FieldName="Content",
                            DisplayName = "Content"
                        },
                        new QueryFieldVm
                        {
                            DependecyName="FK_Post_Category_CategoryId",
                            DisplayName="Category",
                            FieldName="Title",
                            IsMapped=false
                        },
                        new QueryFieldVm
                        {
                            DependecyName="FK_Post_Category_CategoryId",
                            DisplayName="FullName",
                            IsCalculational=true,
                            CalculatorNames = new List<string>(){ "name-plus-title" }

                        },
                        new QueryFieldVm
                        {
                            DisplayName="Date",
                            FieldName="Date",
                            CalculatorNames = new List<string>(){ "to-persian-date" },

                        },
                    },
                    TableName = "Post",
                    Dependencies = new List<QueryDependencyVm>
                    {
                        new QueryDependencyVm
                        {
                            Name = "FK_Post_Category_CategoryId"
                        }
                    },
                    GroupBy = new QueryFieldVm
                    {
                        DisplayName = "Title"
                    },
                    OrderBy = "Title",
                    OrderByDesc = false,

                },

                FilePath = Path.Combine(Directory.GetCurrentDirectory(), "sajjadarash.xlsx")
            }
        }).Result;
        var resultQuery = reportAdapter.GetData(new ReportGetDataRequest
        {
            Entity = new QueryVm
            {
                Fields = new List<QueryFieldVm>
                {
                    new QueryFieldVm
                    {
                        FieldName="Name",
                        DisplayName = "Title"
                    },
                    new QueryFieldVm
                    {
                        FieldName="Content",
                        DisplayName = "Content"
                    },
                    new QueryFieldVm
                    {
                        DependecyName="FK_Post_Category_CategoryId",
                        DisplayName="Category",
                        FieldName="Title",
                    }

                },
                TableName = "Post",
                Dependencies = new List<QueryDependencyVm>
                {
                    new QueryDependencyVm
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