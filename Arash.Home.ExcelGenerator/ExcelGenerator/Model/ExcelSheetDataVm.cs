namespace Arash.Home.ExcelGenerator.ExcelGenerator.Model
{
    public class ExcelSheetDataVm<TEntity> where TEntity : class,new()
    {
        public List<TEntity> Entities { get; set; }
    }
}
