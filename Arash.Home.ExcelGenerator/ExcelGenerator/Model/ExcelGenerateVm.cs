using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Arash.Home.ExcelGenerator.ExcelGenerator.Model
{
    public class ExcelGenerateVm<TEntity> where TEntity : class,new()
    {
        public string FilePath { get; set; }
        public SpreadsheetDocumentType Type { get; set; }
        public List<ExcelWorksheetsVm<TEntity>> Sheets { get; set; }
    }
}
