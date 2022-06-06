using Microsoft.EntityFrameworkCore.Design;

namespace Arash.Home.DynamicReports.DbTest.Database
{
    public class TestDbDesignFactory : IDesignTimeDbContextFactory<TestDb>
    {
        public TestDb CreateDbContext(string[] args)
        {
            return new TestDb();
        }
    }
}
