using DocumentFormat.OpenXml;

namespace Arash.Home.ExcelGenerator.ExcelGenerator.Model
{
    public class ExcelGenerateVm<TEntity> where TEntity : class,new()
    {
        public string FilePath { get; set; }
        public SpreadsheetDocumentType Type { get; set; }
        public List<ExcelWorksheetsVm<TEntity>> Sheets { get; set; }
    }
    public class ExcelGenerateVm
    {
        public string FilePath { get; set; }
        public SpreadsheetDocumentType Type { get; set; }
        public List<ExcelWorksheetsVm> Sheets { get; set; } 
    }
}
