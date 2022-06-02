namespace Arash.Home.QueryGenerator.Abstracts
{
    public class QueryGeneratorRequestModel : QueryBase
    {
        public string Schema { get; set; }
        public string TableName { get; set; }
        public List<QueryFieldsModel> Fields { get; set; }
        public List<QueryGeneratorFilterModel> Filters { get; set; }

        public override string GenerateQuery()
        {
            return $"select {string.Join(',',Fields.Select(a=>a.GenerateQuery()))} from {Schema}.{TableName} {(Filters?.Count>0?$"where {string.Join(" and ",Filters.Select(m=>m.GenerateQuery()))}":"")}";
        }
    }
}
