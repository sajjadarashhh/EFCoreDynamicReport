namespace Arash.Home.QueryGenerator.Abstracts
{
    public class QueryFieldsModel : QueryBase
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string ParentName { get; set; }

        public override string GenerateQuery()
        {
            return $"{ParentName}.{Name} as {DisplayName}";
        }
    }
}