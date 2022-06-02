using Arash.Home.ExcelGenerator.ExcelGenerator.Model;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Arash.Home.ExcelGenerator.ExcelGenerator
{
    public class ExcelGenerator
    {
        private SpreadsheetDocument _spreadsheetDocument;
        private string[] headerColumns = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z".Split(','); 
        public void GenerateExcel2<TEntity>(ExcelGenerateVm<TEntity> generateVm) where TEntity : class, new()
        {
            SpreadsheetDocument ssDoc = SpreadsheetDocument.Create(generateVm.FilePath,
    SpreadsheetDocumentType.Workbook);

            WorkbookPart workbookPart = ssDoc.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            Sheets sheets = ssDoc.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

            int sheetCount=0;

            foreach (var item in generateVm.Sheets)
            {
                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                Worksheet workSheet = new Worksheet();
                SheetData sheetData = new SheetData();

                int rowIndex = 0;
                foreach (var data in item.Data.Entities)
                {
                    int colName = 0;
                    var properties = data.GetType().GetProperties().Where(a => a.PropertyType.IsValueType || a.PropertyType == typeof(string)).ToList();
                    Row rowInSheet = new Row();
                    rowInSheet.RowIndex = (uint)++rowIndex;
                    foreach (var props in properties)
                    {
                        Cell cellInRow = CreateTextCell(headerColumns[colName++], props.GetValue(data).ToString(), rowIndex);
                        rowInSheet.Append(cellInRow);
                    }
                    sheetData.Append(rowInSheet); 
                }
                workSheet.AppendChild(sheetData);
                worksheetPart.Worksheet = workSheet;
                Sheet sheet = new Sheet()
                {
                    Id = ssDoc.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = item.SheetName
                };
                sheets.Append(sheet);
            } 
            ssDoc.Close();
        }
        Cell CreateTextCell(string header, string text, int index)
        {
            Cell c = new Cell();
            c.DataType = CellValues.InlineString;
            c.CellReference = header + index;

            InlineString inlineString = new InlineString();
            Text t = new Text();
            t.Text = text;
            inlineString.AppendChild(t);
            c.AppendChild(inlineString);
            return c;
        }
        public void GenerateExcel<TEntity>(ExcelGenerateVm<TEntity> generateVm) where TEntity : class, new()
        {
            _spreadsheetDocument = SpreadsheetDocument.Create(generateVm.FilePath, generateVm.Type);

            char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            uint id = 0;
            WorkbookPart workbookpart = _spreadsheetDocument.AddWorkbookPart();
            workbookpart.Workbook = new Workbook();
            foreach (var item in generateVm.Sheets)
            {
                WorksheetPart newWorksheetPart = _spreadsheetDocument.WorkbookPart.AddNewPart<WorksheetPart>();
                newWorksheetPart.Worksheet = new Worksheet(new SheetData());
                Sheets sheets = _spreadsheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>() ?? _spreadsheetDocument.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());
                string relationshipId = _spreadsheetDocument.WorkbookPart.GetIdOfPart(newWorksheetPart);

                uint sheetId = 1;
                if (sheets.Elements<Sheet>().Count() > 0)
                {
                    sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                }

                string sheetName = "Sheet" + sheetId;

                Sheet sheet = new Sheet() { Id = relationshipId, SheetId = sheetId, Name = sheetName };
                sheets.Append(sheet);

                uint rowIndex = 0;
                foreach (var data in item.Data.Entities)
                {
                    int colName = 0;
                    var properties = data.GetType().GetProperties().Where(a => a.PropertyType.IsValueType || a.PropertyType == typeof(string)).ToList();
                    foreach (var props in properties)
                    {
                        InsertTextExistingExcel(InsertCellInWorksheet(alpha[colName++].ToString(), ++rowIndex, newWorksheetPart), props.GetValue(data).ToString());
                    }
                }
                newWorksheetPart.Worksheet.Save();
                workbookpart.Workbook.Save();
            }
            _spreadsheetDocument.Save();
        }
        public static Cell InsertTextExistingExcel(Cell cell, string text)
        {

            cell.CellValue = new CellValue(text);
            cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);
            return cell;
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
