using Arash.Home.DynamicReports.DbTest.Database.Table;
using Arash.Home.QueryGenerator.ConsoleTest.DataBase.Table;
using Microsoft.EntityFrameworkCore;

namespace Arash.Home.DynamicReports.DbTest.Database
{
    public class TestDb : DbContext
    {
        public DbSet<PostModel> Post { get; set; }
        public DbSet<CategoryModel> Category { get; set; }
        public TestDb() : base(new DbContextOptionsBuilder().UseSqlServer("server=.;database=QueryGeneratorTest;integrated security=true;").Options)
        {
            this.Database.Migrate();
        }
    }
}
