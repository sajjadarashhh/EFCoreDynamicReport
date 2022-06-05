using Arash.Home.QueryGenerator.ConsoleTest.DataBase.Table;
using Microsoft.EntityFrameworkCore;

namespace Arash.Home.QueryGenerator.ConsoleTest.DataBase
{
    public class TestDb : DbContext
    {
        public DbSet<PostModel> Post { get; set; }
        public TestDb():base(new DbContextOptionsBuilder().UseSqlServer("server=.;database=QueryGeneratorTest;integrated security=true;").Options)
        {
        }
    }
}
