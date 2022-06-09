using Arash.Home.DynamicReports.DbTest.Database;
using Arash.Home.QueryGenerator.Services.Implementation;
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
            Entity = new Arash.Home.QueryGenerator.Services.ViewModels.QueryVm
            {
                Fields=new List<Arash.Home.QueryGenerator.Services.ViewModels.QueryFieldVm>
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
                        DependecyName="FK_Post_CategoryModel_CategoryId",
                        DisplayName="Category",
                        FieldName="Name",
                    }
                    
                },
                TableName= "Post",
                IsForJson=true,
                Dependencies = new List<Arash.Home.QueryGenerator.Services.ViewModels.QueryDependencyVm>
                {
                    new Arash.Home.QueryGenerator.Services.ViewModels.QueryDependencyVm
                    {
                        Name = "FK_Post_CategoryModel_CategoryId"
                    }
                }
            }
        }).Result;
        Console.WriteLine(result.Entity.Query);
    }
}