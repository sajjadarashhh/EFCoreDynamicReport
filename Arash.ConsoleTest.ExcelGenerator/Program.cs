using Arash.ConsoleTest.ExcelGenerator.Students;
using Arash.Home.ExcelGenerator.ExcelGenerator;
using Arash.Home.ExcelGenerator.ExcelGenerator.Model;
using DocumentFormat.OpenXml;

internal class Program
{
    private static void Main(string[] args)
    {
        ExcelGenerator excelGenerator = new ExcelGenerator();
        excelGenerator.GenerateExcel2(new ExcelGenerateVm<StudentModel>()
        {
            FilePath=Path.Combine(Directory.GetCurrentDirectory(),"imp.xlsx"),
            Sheets=new List<ExcelWorksheetsVm<StudentModel>>
            {
                new ExcelWorksheetsVm<StudentModel>
                {
                    SheetName= "sajjad arash",
                    Data = new ExcelSheetDataVm<StudentModel>
                    {
                        Entities = new List<StudentModel>
                        {
                            new StudentModel
                            {
                                FirstName="sajjad",
                                LastName="arash",
                                NationalCode="4420970197"
                            },
                            new StudentModel
                            {
                                FirstName="sajjad",
                                LastName="arash",
                                NationalCode="4420970197"
                            },
                            new StudentModel
                            {
                                FirstName="sajjad",
                                LastName="arash",
                                NationalCode="4420970197"
                            },
                            new StudentModel
                            {
                                FirstName="sajjad3",
                                LastName="arash3",
                                NationalCode="4420970197"
                            },
                        }
                    }
                },
                new ExcelWorksheetsVm<StudentModel>
                {
                    SheetName= "sajjad arash2",
                    Data = new ExcelSheetDataVm<StudentModel>
                    {
                        Entities = new List<StudentModel>
                        {
                            new StudentModel
                            {
                                FirstName="sajjad2",
                                LastName="arash2",
                                NationalCode="4420970197"
                            },
                            new StudentModel
                            {
                                FirstName="sajjad2",
                                LastName="arash2",
                                NationalCode="4420970197"
                            },
                            new StudentModel
                            {
                                FirstName="sajjad2",
                                LastName="arash2",
                                NationalCode="4420970197"
                            },
                            new StudentModel
                            {
                                FirstName="sajjad3",
                                LastName="arash3",
                                NationalCode="4420970197"
                            },
                        }
                    }
                }
            },
            Type=SpreadsheetDocumentType.Workbook
        });

    }
}