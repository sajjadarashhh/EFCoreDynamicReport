using Arash.Home.ExcelGenerator.ExcelGenerator.Model;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Arash.Home.ExcelGenerator.ExcelGenerator
{
    public class ExcelGenerator:IExcelGenerator
    {
        private SpreadsheetDocument _spreadsheetDocument;
        private string[] headerColumns = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z".Split(',');
        public void GenerateExcel2<TEntity>(ExcelGenerateVm<TEntity> generateVm) where TEntity : class, new()
        {
            _spreadsheetDocument = SpreadsheetDocument.Create(generateVm.FilePath, SpreadsheetDocumentType.Workbook);

            WorkbookPart workbookPart = _spreadsheetDocument.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            Sheets sheets = _spreadsheetDocument.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());
            int sheetCount = 0;

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
                    Id = _spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = item.SheetName
                };
                sheets.Append(sheet);
            }
            _spreadsheetDocument.Close();
        }
        public void GenerateExcel<TEntity>(ExcelGenerateVm<TEntity> generateVm) where TEntity : class, new()
        {
            _spreadsheetDocument = SpreadsheetDocument.Create(generateVm.FilePath, SpreadsheetDocumentType.Workbook);

            WorkbookPart workbookPart = _spreadsheetDocument.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            Sheets sheets = _spreadsheetDocument.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());
            int sheetCount = 0;

            foreach (var item in generateVm.Sheets)
            {
                item.GenerateSheet(sheets, workbookPart, _spreadsheetDocument);
            }
            _spreadsheetDocument.Close();
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

        public void GenerateExcelFromAnonymousType(ExcelGenerateVm generateVm)
        {
            _spreadsheetDocument = SpreadsheetDocument.Create(generateVm.FilePath, SpreadsheetDocumentType.Workbook);

            WorkbookPart workbookPart = _spreadsheetDocument.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            Sheets sheets = _spreadsheetDocument.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

            foreach (var item in generateVm.Sheets)
            {
                item.GenerateSheet(sheets, workbookPart, _spreadsheetDocument);
            }
            _spreadsheetDocument.Close();
        }
    }
}
