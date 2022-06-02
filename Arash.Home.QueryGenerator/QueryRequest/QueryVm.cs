using Arash.Home.QueryGenerator.Abstracts;

namespace Arash.Home.QueryGenerator.QueryRequest
{
    public class QueryVm
    {
        public string TableName { get; set; }
        public List<QueryFieldVm> Fields { get; set; }
    }
}
