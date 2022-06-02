namespace Arash.Home.ExcelGenerator.ExcelGenerator.Model
{
    public class ExcelWorksheetsVm<TEntity> where TEntity : class,new()
    {
        public string SheetName { get; set; }
        public ExcelSheetDataVm<TEntity> Data { get; set; }
    }
}
