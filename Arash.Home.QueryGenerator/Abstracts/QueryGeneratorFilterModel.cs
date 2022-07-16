namespace Arash.Home.QueryGenerator.Abstracts
{
    public enum FilterMode
    {
        Equal
    }
    public class QueryGeneratorFilterModel : QueryBase
    {
        public QueryFieldsModel RightField { get; set; }
        public QueryBase LeftSide { get; set; }
        public FilterMode Mode { get; set; }
        public override string GenerateQuery()
        {
            return $"{LeftSide.GenerateQuery()} {Mode switch { FilterMode.Equal=>"=" }} {RightField.GenerateQuery()}";
        }
    }
}
