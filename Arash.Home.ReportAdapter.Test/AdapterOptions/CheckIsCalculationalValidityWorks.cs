using Arash.Home.ExcelGenerator.ExcelGenerator.AdapterOptions;

namespace Arash.Home.ReportAdapter.Test.AdapterOptions
{
    public class CheckIsCalculationalValidityWorks : AdapterBase
    {
        public override string Name => "name-plus-title";

        public override string Description => "nothing";

        public override string Execute(int row, string value)
        {
            var title = base.Container.GetName("Title", row);
            var category = base.Container.GetName("Category", row);
            return $"{title}-{category}";
        }
    }
}
