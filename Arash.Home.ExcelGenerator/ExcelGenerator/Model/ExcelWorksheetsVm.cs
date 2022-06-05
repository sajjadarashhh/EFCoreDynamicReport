using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Arash.Home.ExcelGenerator.ExcelGenerator.Model
{
    public class ExcelWorksheetsVm<TEntity> where TEntity : class,new()
    {
        public string SheetName { get; set; }
        public ExcelSheetDataVm<TEntity> Data { get; set; }
        public void GenerateSheet(Sheets sheets,WorkbookPart workbookPart,SpreadsheetDocument _spreadsheetDocument)
        {
            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            Worksheet workSheet = new Worksheet();
            SheetData sheetData = new SheetData();

            int rowIndex = 0;
            foreach (var data in Data.Entities)
            {
                data.GenerateData(rowIndex, sheetData);
            }
            workSheet.AppendChild(sheetData);
            worksheetPart.Worksheet = workSheet;
            Sheet sheet = new Sheet()
            {
                Id = _spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = SheetName
            };
            sheets.Append(sheet);
        }
    }
}
