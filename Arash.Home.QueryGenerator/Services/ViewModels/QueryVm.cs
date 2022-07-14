using Arash.Home.QueryGenerator.Abstracts;

namespace Arash.Home.QueryGenerator.Services.ViewModels
{
    public class QueryVm
    {
        public string TableName { get; set; }
        public bool IsForJson { get; set; }
        public string OrderBy { get; set; }
        public bool OrderByDesc { get; set; }
        public List<QueryFieldVm> Fields { get; set; }
        public List<QueryDependencyVm> Dependencies { get; set; }
        public QueryFieldVm GroupBy { get; set; }
        public List<QueryGeneratorFilterModel> Filters { get; set; }
    }
}
