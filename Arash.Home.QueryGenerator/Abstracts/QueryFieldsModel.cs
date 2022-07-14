namespace Arash.Home.QueryGenerator.Abstracts
{
    public class QueryFieldsModel : QueryBase
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string ParentName { get; set; }
        public bool IsMapped { get; set; } = true;
        public override string GenerateQuery()
        {
            return $"{ParentName}.{Name} as [{DisplayName}{(IsMapped ? "isits" : "isnot")}]";
        }
    }
}