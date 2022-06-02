namespace Arash.Home.QueryGenerator.Abstracts
{
    public class QueryValueModel : QueryBase
    {
        public string Value { get; set; }

        public override string GenerateQuery()
        {
            return $"N'{Value}'";
        }
    }
}
