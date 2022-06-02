using Microsoft.EntityFrameworkCore;

namespace Arash.Home.QueryGenerator.SqlQueryGenerator
{
    public class SqlQueryGeneratorModule
    { 
        private readonly List<string> Keys = new List<string>();
        public SqlQueryGeneratorModule(DbContext db)
        {
        }
    }
}