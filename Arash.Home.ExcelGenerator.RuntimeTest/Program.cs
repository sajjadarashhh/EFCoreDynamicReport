using Arash.Home.ExcelGenerator.ExcelGenerator;
using Arash.Home.ExcelGenerator.ExcelGenerator.Model;
using Arash.Home.ExcelGenerator.RuntimeTest.Data;
using DocumentFormat.OpenXml;

internal class Program
{
    private static void Main(string[] args)
    {
        ExcelGenerator excelGenerator = new ExcelGenerator(); 
        excelGenerator.GenerateExcel2(new ExcelGenerateVm<SimpleData>()
        {
            FilePath=Path.Combine(Directory.GetCurrentDirectory(),"arash.xlsx"),
            Sheets = new List<ExcelWorksheetsVm<SimpleData>>()
            {
                new ExcelWorksheetsVm<SimpleData>
                {
                    Data=new ExcelSheetDataVm<SimpleData>
                    {
                        Entities= new List<SimpleData>
                        {
                            new SimpleData
                            {
                                Family="arash9",
                                Name="sajjad10"
                            },
                            new SimpleData
                            {
                                Family="arash11",
                                Name="sajjad12"
                            },
                            new SimpleData
                            {
                                Family="arash13",
                                Name="sajjad14"
                            },
                            new SimpleData
                            {
                                Family="arash15",
                                Name="sajjad16"
                            }
                        }
                    },
                    SheetName="sajjad arash"
                },
                new ExcelWorksheetsVm<SimpleData>
                {
                    Data=new ExcelSheetDataVm<SimpleData>
                    {
                        Entities= new List<SimpleData>
                        {
                            new SimpleData
                            {
                                Family="arash1",
                                Name="sajjad2"
                            },
                            new SimpleData
                            {
                                Family="arash3",
                                Name="sajjad4"
                            },
                            new SimpleData
                            {
                                Family="arash5",
                                Name="sajjad6"
                            },
                            new SimpleData
                            {
                                Family="arash7",
                                Name="sajjad8"
                            }
                        }
                    },
                    SheetName="sajjad arash2"
                },
            },
            Type = SpreadsheetDocumentType.Workbook
        });
    }
}