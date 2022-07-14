using Arash.Home.ReportAdapter.ReportAdapterModule.Abstracts;

namespace Arash.Home.ExcelGenerator.ExcelGenerator.AdapterOptions
{
    public abstract class AdapterBase
    {
        protected ReportAdapterDataContainer Container;
        public void setValues(ReportAdapterDataContainer container)
        {
            Container = container;
        }
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract string Execute(int row,string value);
    }
}
