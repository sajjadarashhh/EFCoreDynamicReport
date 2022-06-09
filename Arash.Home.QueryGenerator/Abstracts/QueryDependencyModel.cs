namespace Arash.Home.QueryGenerator.Abstracts
{
    public class QueryDependencyModel : QueryBase
    {
        public string TableName { get; set; }
        public string DependencyName { get; set; }
        public string RightValue { get; set; }
        public string LeftValue { get; set; }

        public override string GenerateQuery()
        {
            return $"JOIN {DependencyName} on {TableName}.{RightValue} = {DependencyName}.{LeftValue}";
        }
    }
}
