namespace Arash.Home.QueryGenerator.Abstracts
{
    public class QueryGeneratorRequestModel : QueryBase
    {
        public string Schema { get; set; }
        public string TableName { get; set; }
        public List<QueryFieldsModel> Fields { get; set; }
        public List<QueryGeneratorFilterModel> Filters { get; set; }
        public List<QueryDependencyModel> Dependencies { get; set; }
        public bool IsForJson { get; set; }
        public bool OrderByDesc { get; set; }
        public string OrderBy { get; set; }
        public string ForJsonMode { get; set; } = "auto";
        public override string GenerateQuery()
        {
            return $"select {string.Join(',', Fields.Select(a => a.GenerateQuery()))} from {$"{(Schema?.Length > 0 ? $"{Schema}." : "")}{TableName}"} {(Dependencies.Count>0?$"{string.Join(' ', Dependencies.Select(o => o.GenerateQuery()))}":"")} {(Filters?.Count > 0 ? $"where {string.Join(" and ", Filters.Select(m => m.GenerateQuery()))}" : "")} {(IsForJson?$"for json {ForJsonMode}":"")} {(OrderBy?.Length>0?$"order by {OrderBy} {(OrderByDesc?"desc":"")}":$"")}";
        }
    }
}
