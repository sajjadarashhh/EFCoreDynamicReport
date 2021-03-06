namespace Arash.Home.QueryGenerator.Services.ViewModels
{
    public class QueryFieldVm
    {
        public string FieldName { get; set; }
        public string DisplayName { get; set; }
        public string DependecyName { get; set; }
        public List<string> CalculatorNames { get; set; }
        public bool IsMapped { get; set; } = true;
        public bool IsCalculational { get; set; } = false;
    }
}
