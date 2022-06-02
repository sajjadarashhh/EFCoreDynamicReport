using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        DbContext db = new DbContext(new DbContextOptionsBuilder().Options);
        //var sqlGenerator = new SqlQueryGeneratorModule(db);
        Console.WriteLine(sqlGenerator.Generate);
        Dictionary<string, string> parameters = new Dictionary<string, string>();
        List<Dictionary<string, string>> parameterValues = new List<Dictionary<string, string>>();
        var Headers = parameters.Select((a, i) =>
        {
            return new { index = i, Name = a.Key };
        });
        foreach (var item in parameterValues)
        {
            for (int i = 0; i < Headers.Count(); i++)
            {
                var name = Headers.Where(a => a.index == i).First().Name;
                var value = item[name];
            }
        }
    }
}