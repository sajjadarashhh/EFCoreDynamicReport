using Arash.Home.QueryGenerator.Abstracts;

namespace Arash.Home.QueryGenerator.Services.ViewModels
{
    public class QueryVm
    {
        public string TableName { get; set; }
        public bool IsForJson { get; set; }
        public List<QueryFieldVm> Fields { get; set; }
    }
}
