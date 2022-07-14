namespace Arash.Home.QueryGenerator.Abstracts
{
    public class QueryFieldsModel : QueryBase
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string ParentName { get; set; }
        public bool IsMapped { get; set; } = true;
        public bool IsCalculational { get; set; } = false;
        public override string GenerateQuery()
        {
            return IsCalculational ? $"null as [{DisplayName}{(IsMapped ? "isits" : "isnot")}]" : $"{ParentName}.{Name} as [{DisplayName}{(IsMapped ? "isits" : "isnot")}]";
        }
    }
}