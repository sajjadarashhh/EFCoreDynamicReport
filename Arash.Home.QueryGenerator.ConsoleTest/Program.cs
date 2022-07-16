using Arash.Home.DynamicReports.DbTest.Database;
using Arash.Home.QueryGenerator.Abstracts;
using Arash.Home.QueryGenerator.Services.Implementation;
using Arash.Home.QueryGenerator.Services.ViewModels;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

internal class Program
{
    private static void Main(string[] args)
    {
        var db = new TestDb();
        IQueryGeneratorService queryGeneratorService = new QueryGeneratorService(db);
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject((queryGeneratorService.GetTables(new Arash.Home.QueryGenerator.Services.Messaging.QueryGetTableRequest())).Result.Entities));
        Console.ReadKey();
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;
        var result = queryGeneratorService.GenerateQuery(new Arash.Home.QueryGenerator.Services.Messaging.QueryGenerateRequest
        {
            Entity = new QueryVm
            { 
                Filters = new List<QueryGeneratorFilterModel>
                {
                    new QueryGeneratorFilterModel
                    {
                        RightField= new QueryFieldsModel()
                        {
                            Name="content",
                            ParentName="Category"
                        },
                        LeftSide = new QueryValueModel()
                        {
                            Value = ""
                        }
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
                    },
                    new QueryFieldVm
                    {
                        DisplayName="تاریخ",
                        FieldName="Date",
                    }
                },
                TableName = "Post",
                IsForJson = true,
                Dependencies = new List<QueryDependencyVm>
                {
                    new QueryDependencyVm
                    {
                        Name = "FK_Post_Category_CategoryId"
                    }
                },

            }
        }).Result;
        Console.WriteLine(result.Entity.Query);
    }
}