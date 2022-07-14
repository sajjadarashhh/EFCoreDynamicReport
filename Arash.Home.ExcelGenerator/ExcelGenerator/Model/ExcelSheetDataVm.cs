using DocumentFormat.OpenXml.Spreadsheet;

namespace Arash.Home.ExcelGenerator.ExcelGenerator.Model
{
    public class ExcelSheetDataVm<TEntity> where TEntity : class, new()
    {
        public List<TEntity> Entities { get; set; }

    }
    public class ExcelSheetDataVm
    {
        public List<List<string>> Entities { get; set; }

    }
    internal static class ExcelDataHelper
    {
        private static string[] headerColumns = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z".Split(',');
        public static void GenerateData(this object data, int rowIndex, SheetData sheetData)
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
        public static void GenerateData(this List<string> data, ref int rowIndex, SheetData sheetData)
        {
            int colName = 0;
            Row rowInSheet = new Row();
            rowInSheet.RowIndex = (uint)++rowIndex;
            foreach (var props in data)
            {
                Cell cellInRow = CreateTextCell(headerColumns[colName++], props, rowIndex);
                rowInSheet.Append(cellInRow);
            }
            sheetData.Append(rowInSheet);
        }
        static Cell CreateTextCell(string header, string text, int index)
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
    }
}
