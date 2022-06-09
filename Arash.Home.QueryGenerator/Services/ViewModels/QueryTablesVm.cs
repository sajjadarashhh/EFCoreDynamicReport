namespace Arash.Home.QueryGenerator.Services.ViewModels
{
    public class QueryTablesVm
    {
        public string Name { get; set; }
        public Dictionary<string,string> Fields { get; set; }
        public List<QueryDependencyVm> Dependencies { get; set; }
    }
}
