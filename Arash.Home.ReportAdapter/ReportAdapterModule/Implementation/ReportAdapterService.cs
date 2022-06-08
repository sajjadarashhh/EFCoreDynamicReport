using Arash.Home.ExcelGenerator.ExcelGenerator;
using Arash.Home.QueryGenerator.Exceptions;
using Arash.Home.QueryGenerator.Services.Implementation;
using Arash.Home.QueryGenerator.Services.Messaging;
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
        private readonly Func<ReportExecuteQueryRequest, Task<ReportExecuteQueryResponse>> ExecuteQueryCustom;
        public ReportAdapterService(IQueryGeneratorService queryGeneratorService, IExcelGenerator excelGenerator, DbContext dbContext, Func<ReportExecuteQueryRequest, Task<ReportExecuteQueryResponse>> executeQueryCustom = null)
        {
            this.queryGeneratorService = queryGeneratorService;
            this.excelGenerator = excelGenerator;
            this.dbContext = dbContext;
            ExecuteQueryCustom = executeQueryCustom;
        }

        public async Task<ReportExecuteQueryResponse> ExecuteQuery(ReportExecuteQueryRequest request)
        {
            if (ExecuteQueryCustom is not null)
            {
                return await ExecuteQueryCustom.Invoke(request);
            }
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
                    await connection.OpenAsync();
                    try
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
                    catch
                    {

                    }
                    await connection.CloseAsync();
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
 
        public async Task<ReportCreateResponse> ReportCreate(ReportCreateRequest request)
        {
            try
            {
                QueryGenerateResponse response;
                try
                {
                    response = await queryGeneratorService.GenerateQuery(new QueryGenerator.Services.Messaging.QueryGenerateRequest
                    {
                        Entity = request.Entity.QueryGenerateRequest
                    });
                }
                catch (TableNotFoundException)
                {
                    return new ReportCreateResponse()
                    {
                        IsSuccess = false,
                        Message = "Table not found"
                    };
                }
                catch (ColumnNotFoundException ex)
                {
                    return new ReportCreateResponse()
                    {
                        IsSuccess = false,
                        Message = ex.Message
                    };
                }
                var queryResult = await ExecuteQuery(new ReportExecuteQueryRequest
                {
                    Entity = new ViewModels.QueryExecuteVm
                    {
                        Query = response.Entity.Query
                    }
                });

                excelGenerator.GenerateExcelFromAnonymousType(new ExcelGenerator.ExcelGenerator.Model.ExcelGenerateVm
                {
                    FilePath = Path.Combine(Directory.GetCurrentDirectory(), "test.xlsx"),
                    Type = DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook,
                    Sheets = new List<ExcelGenerator.ExcelGenerator.Model.ExcelWorksheetsVm>
                    {
                        new ExcelGenerator.ExcelGenerator.Model.ExcelWorksheetsVm
                        {
                            SheetName="first",
                            Data = new ExcelGenerator.ExcelGenerator.Model.ExcelSheetDataVm
                            {
                                Entities = new List<List<string>>{ queryResult.Entity.Names }.Union(queryResult.Entity.Values).ToList()
                            },
                        },
                    },
                });
                return new ReportCreateResponse
                {
                    IsSuccess = true,
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
