using Arash.Home.ExcelGenerator.ExcelGenerator;
using Arash.Home.QueryGenerator.Services.Implementation;
using Arash.Home.ReportAdapter.ReportAdapterModule.Messaging;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Dynamic;

namespace Arash.Home.ReportAdapter.ReportAdapterModule.Implementation
{
    public class ReportAdapterService : IReportAdapterService
    {
        private readonly IQueryGeneratorService queryGeneratorService;
        private readonly IExcelGenerator excelGenerator;
        private readonly DbContext dbContext;

        public ReportAdapterService(IQueryGeneratorService queryGeneratorService, IExcelGenerator excelGenerator)
        {
            this.queryGeneratorService = queryGeneratorService;
            this.excelGenerator = excelGenerator;
        }

        public async Task<ReportExecuteQueryResponse> ExecuteQuery(ReportExecuteQueryRequest request)
        {
            try
            {
                var response = new ReportExecuteQueryResponse();
                response.Entity = new ViewModels.QueryResultVm()
                {
                    Names = new List<string>(),
                    Values = new List<List<string>>()
                };
                using (var connection = dbContext.Database.GetDbConnection())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = request.Entity.Query;
                        var result = await command.ExecuteReaderAsync();
                        var haveResult = await result.ReadAsync();
                        for (int i = 0; i < result.FieldCount; i++)
                        {
                            response.Entity.Names.Add(result.GetName(i));
                        }
                        if (haveResult)
                            do
                            {
                                var row = new List<string>();
                                for (int i = 0; i < result.FieldCount; i++)
                                {
                                    row.Add(result.GetValue(i)?.ToString() ?? "");
                                }
                                response.Entity.Values.Add(row);
                            }
                            while (await result.NextResultAsync());
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                return new ReportExecuteQueryResponse
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
        private object ToExpando(List<string> properties)
        {
            var obj = new ExpandoObject();
            var dictionary = obj as IDictionary<string, object>;

            foreach (var property in properties)
                dictionary.Add("obj-"+new Random().Next(1000000000), property);
            return JsonConvert.DeserializeObject(JsonConvert.SerializeObject(dictionary));
        }
        public async Task<ReportCreateResponse> ReportCreate(ReportCreateRequest request)
        {
            try
            {
                var response = await queryGeneratorService.GenerateQuery(new QueryGenerator.Services.Messaging.QueryGenerateRequest
                {
                    Entity = request.Entity.QueryGenerateRequest
                });
                var queryResult = await ExecuteQuery(new ReportExecuteQueryRequest
                {
                    Entity = new ViewModels.QueryExecuteVm
                    {
                        Query = response.Entity.Query
                    }
                });

                excelGenerator.GenerateExcel(new ExcelGenerator.ExcelGenerator.Model.ExcelGenerateVm<object>
                {
                    FilePath = Path.Combine(Directory.GetCurrentDirectory(),"test.xlsx"),
                    Type= DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook,
                    Sheets = new List<ExcelGenerator.ExcelGenerator.Model.ExcelWorksheetsVm<object>>
                    {
                        new ExcelGenerator.ExcelGenerator.Model.ExcelWorksheetsVm<object>
                        {
                            SheetName="first",
                            Data = new ExcelGenerator.ExcelGenerator.Model.ExcelSheetDataVm<object>
                            {
                                Entities = new List<object>
                                {
                                    ToExpando(queryResult.Entity.Names),
                                    
                                }.Union(queryResult.Entity.Values.Select(m=>ToExpando(m))).ToList()
                            },
                        },
                    },
                });
                return new ReportCreateResponse
                {
                    IsSuccess=true,
                    Message = "operation success."
                };
            }
            catch (Exception ex)
            {
                return new ReportCreateResponse
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}
