using Arash.Home.ExcelGenerator.ExcelGenerator.Model;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Arash.Home.ExcelGenerator.ExcelGenerator
{
    public class ExcelGenerator
    {
        private SpreadsheetDocument _spreadsheetDocument;


        public void GenerateExcel<TEntity>(ExcelGenerateVm<TEntity> generateVm) where TEntity : class, new()
        {
            _spreadsheetDocument = SpreadsheetDocument.Create(generateVm.FilePath, generateVm.Type);

            var workbookpart = _spreadsheetDocument.AddWorkbookPart();
            char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            uint id = 0;
            foreach (var item in generateVm.Sheets)
            {
                WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());
                Sheets sheets = _spreadsheetDocument.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());
                Sheet sheet = new Sheet()
                {
                    Id = _spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = ++id,
                    Name = item.SheetName
                };
                uint rowIndex = 0;
                foreach (var data in item.Data.Entities)
                {
                    int colName = 0;
                    var properties = data.GetType().GetProperties().Where(a => a.PropertyType.IsValueType || a.PropertyType == typeof(string)).ToList();
                    foreach (var props in properties)
                    {
                        InsertTextExistingExcel(InsertCellInWorksheet(alpha[colName++].ToString(), ++rowIndex, worksheetPart),props.GetValue(data).ToString());
                    }
                }
                sheets.Append(sheet);
                worksheetPart.Worksheet.Save();
            }
            _spreadsheetDocument.Save();
        }
        public static void InsertTextExistingExcel(Cell cell ,string text)
        {

            cell.CellValue = new CellValue(text);
            cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);

        }
        private static Cell InsertCellInWorksheet(string columnName, uint rowIndex, WorksheetPart worksheetPart)
        {
            Worksheet worksheet = worksheetPart.Worksheet;
            SheetData sheetData = worksheet.GetFirstChild<SheetData>();
            string cellReference = columnName + rowIndex;

            Row row;
            if (sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).Count() != 0)
            {
                row = sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
            }
            else
            {
                row = new Row() { RowIndex = rowIndex };
                sheetData.Append(row);
            }

            Cell refCell = row.Descendants<Cell>().LastOrDefault();

            Cell newCell = new Cell() { CellReference = cellReference };
            row.InsertAfter(newCell, refCell);

            worksheet.Save();
            return newCell;

        }
    }
}
