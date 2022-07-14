using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Text.RegularExpressions;

namespace Arash.Home.ExcelGenerator.ExcelGenerator.Model
{
    public class ExcelWorksheetsVm<TEntity> where TEntity : class, new()
    {
        public string SheetName { get; set; }
        public ExcelSheetDataVm<TEntity> Data { get; set; }
        public void GenerateSheet(Sheets sheets, WorkbookPart workbookPart, SpreadsheetDocument _spreadsheetDocument)
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
    public class ExcelWorksheetsVm
    {
        public string SheetName { get; set; }
        public ExcelSheetDataVm Data { get; set; }
        public void GenerateSheet(Sheets sheets, WorkbookPart workbookPart, SpreadsheetDocument _spreadsheetDocument, UInt32Value id)
        {
            if (!new Regex("[A-Za-zآ-ی 0-9]{0,30}").Match(SheetName).Success)
                throw new Exception("Please check sheet name entered.");
            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());
            SheetData sheetData = new SheetData();
            Worksheet workSheet = new Worksheet();
            Sheet sheet = new Sheet()
            {
                Id = _spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                SheetId = id,
                Name = SheetName.Substring(0, SheetName.Length > 30 ? 30 : SheetName.Length),
            };
            sheets.Append(sheet);

            int rowIndex = 0;
            foreach (var data in Data.Entities)
            {
                data.GenerateData(ref rowIndex, sheetData);
            }
            workSheet.AppendChild(sheetData);
            worksheetPart.Worksheet = workSheet;
        } 
    }
}
