using Arash.Home.ExcelGenerator.ExcelGenerator;
using Arash.Home.ExcelGenerator.ExcelGenerator.AdapterOptions;
using Arash.Home.ExcelGenerator.ExcelGenerator.Model;
using Arash.Home.QueryGenerator.Exceptions;
using Arash.Home.QueryGenerator.Services.Implementation;
using Arash.Home.QueryGenerator.Services.Messaging;
using Arash.Home.ReportAdapter.ReportAdapterModule.Abstracts;
using Arash.Home.ReportAdapter.ReportAdapterModule.Messaging;
using Arash.Home.ReportAdapter.ReportAdapterModule.ViewModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Dynamic;
using System.Reflection;

namespace Arash.Home.ReportAdapter.ReportAdapterModule.Implementation
{
    public class ReportAdapterService : IReportAdapterService
    {
        private readonly IQueryGeneratorService queryGeneratorService;
        private readonly IExcelGenerator excelGenerator;
        private readonly DbContext dbContext;
        private readonly List<AdapterBase> Adapters;
        private readonly Func<ReportExecuteQueryRequest, Task<ReportExecuteQueryResponse>> ExecuteQueryCustom;
        public ReportAdapterService(IQueryGeneratorService queryGeneratorService, IExcelGenerator excelGenerator, DbContext dbContext, Func<ReportExecuteQueryRequest, Task<ReportExecuteQueryResponse>> executeQueryCustom = null, List<AdapterBase> _adapters = default(List<AdapterBase>))
        {
            this.queryGeneratorService = queryGeneratorService;
            this.excelGenerator = excelGenerator;
            this.dbContext = dbContext;
            ExecuteQueryCustom = executeQueryCustom;
            Adapters = _adapters;
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
                    connection.ConnectionString = dbContext.Database.GetConnectionString();
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
                                while (await result.ReadAsync());
                        }
                    }
                    catch
                    {

                    }
                    await connection.CloseAsync();
                }
                response.IsSuccess = true;
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

        public async Task<ReportExecuteQueryResponse> GetData(ReportGetDataRequest request)
        {
            var query = await queryGeneratorService.GenerateQuery(new QueryGenerateRequest
            {
                Entity = request.Entity
            });
            if (!query.IsSuccess)
            {
                return new ReportExecuteQueryResponse
                {
                    IsSuccess = query.IsSuccess,
                    Entity = null,
                    Message = query.Message
                };
            }
            var result = await ExecuteQuery(new ReportExecuteQueryRequest
            {
                Entity = new ViewModels.QueryExecuteVm
                {
                    Query = query.Entity.Query
                }
            });
            var notMapped = result.Entity.Names.Select((m, i) =>
            {
                return new
                {
                    IsMapped = m.Substring(m.Length - 5) == "isits",
                    Index = i
                };
            }).Where(a => !a.IsMapped);
            result.Entity.Names = result.Entity.Names.Select((m, i) => new { m, i }).Where(m => notMapped.All(o => o.Index != m.i)).Select(a => a.m.Remove(a.m.Length - 5)).ToList();
            foreach (var item in result.Entity.Values)
            {
                for (int i = 0; i < item.Count; i++)
                {
                    if (notMapped.Any(a => a.Index == i))
                        item.Remove(item[i]);
                    else
                        item[i] = item[i];
                }
            }
            return result;
        }

        public async Task<QueryGetTableResponse> GetTables(QueryGetTableRequest request)
        {
            return await queryGeneratorService.GetTables(request);
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
                catch (DependencyNotFoundException ex)
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
                ReportAdapterDataContainer reportAdapterData = new ReportAdapterDataContainer(queryResult.Entity.Values.Select(o => o.Select((m,i)=>new { m, i }).ToDictionary(a => queryResult.Entity.Names[a.i].Remove(queryResult.Entity.Names[a.i].Length-5), a => a.m)).ToList());
                var adapterOptions = request.Entity.QueryGenerateRequest.Fields.Where(a => a.CalculatorNames?.Any() ?? false).SelectMany(a => a.CalculatorNames.Select(m => new { name = a.DisplayName, calcName = Adapters.FirstOrDefault(a => a.Name == m) })).ToDictionary(o => o.name, m => m.calcName);
                int row = 0;
                foreach (var item in queryResult.Entity.Values)
                {
                    for (int i = 0; i < item.Count; i++)
                    {
                        var itemAdapterOption = adapterOptions.Where(a => a.Key == queryResult.Entity.Names[i]);
                        foreach (var option in itemAdapterOption)
                        {
                            option.Value.setValues(reportAdapterData);
                            item[i] = option.Value.Execute(row,item[i]);
                        }
                    }
                    row++;
                }
                var notMapped = queryResult.Entity.Names.Select((m, i) =>
                {
                    return new
                    {
                        IsMapped = m.Substring(m.Length - 5) == "isits",
                        Index = i
                    };
                }).Where(a => !a.IsMapped);
                queryResult.Entity.Names = queryResult.Entity.Names.Select((m, i) => new { m, i }).Where(m => notMapped.All(o => o.Index != m.i)).Select(a => a.m.Remove(a.m.Length - 5)).ToList();
                foreach (var item in queryResult.Entity.Values)
                {
                    for (int i = 0; i < item.Count; i++)
                    {
                        if (notMapped.Any(a => a.Index == i))
                            item.Remove(item[i]);
                        else
                            item[i] = item[i];
                    }
                }
                var groupKey = queryResult.Entity.Names.IndexOf(request.Entity.QueryGenerateRequest.GroupBy.DisplayName);
                var sheets = queryResult.Entity.Values.GroupBy(a => a[groupKey]).Select(a => new ExcelWorksheetsVm
                {
                    SheetName = a.Key,
                    Data = new ExcelSheetDataVm
                    {
                        Entities = new List<List<string>> { queryResult.Entity.Names }.Union(a.ToList()).ToList(),
                    },

                }).ToList();
                excelGenerator.GenerateExcelFromAnonymousType(new ExcelGenerateVm
                {
                    FilePath = request.Entity.FilePath,
                    Type = DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook,
                    Sheets = sheets
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
