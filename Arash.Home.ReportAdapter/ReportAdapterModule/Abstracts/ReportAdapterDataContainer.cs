namespace Arash.Home.ReportAdapter.ReportAdapterModule.Abstracts
{
    public class ReportAdapterDataContainer
    {
        private List<Dictionary<string, string>> Values { get; set; }

        public ReportAdapterDataContainer(List<Dictionary<string, string>> values)
        {
            Values = values;
        }

        public string GetName(string fieldName, int row) => Values[row][fieldName];
    }
}
