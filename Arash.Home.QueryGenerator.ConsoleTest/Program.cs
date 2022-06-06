using Arash.Home.QueryGenerator.ConsoleTest.DataBase;
using Arash.Home.QueryGenerator.Services.Implementation;

internal class Program
{
    private static void Main(string[] args)
    {
        var db = new TestDb();
        IQueryGeneratorService queryGeneratorService = new QueryGeneratorService(db);
        var result = queryGeneratorService.GenerateQuery(new Arash.Home.QueryGenerator.Services.Messaging.QueryGenerateRequest
        {
            Entity = new Arash.Home.QueryGenerator.Services.ViewModels.QueryVm
            {
                Fields=new List<Arash.Home.QueryGenerator.Services.ViewModels.QueryFieldVm>
                {
                    new Arash.Home.QueryGenerator.Services.ViewModels.QueryFieldVm
                    {
                        FieldName="Name",
                        DisplayName = "عنوان"
                    }, 
                    new Arash.Home.QueryGenerator.Services.ViewModels.QueryFieldVm
                    {
                        FieldName="Content",
                        DisplayName = "محتوا"
                    }, 
                },
                TableName= "Post"
            }
        }).Result;
        Console.WriteLine(result.Entity.Query);
    }
}