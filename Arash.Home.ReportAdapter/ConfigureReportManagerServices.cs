using Arash.Home.ExcelGenerator.ExcelGenerator;
using Arash.Home.ExcelGenerator.ExcelGenerator.AdapterOptions;
using Arash.Home.QueryGenerator.Services.Implementation;
using Arash.Home.ReportAdapter.ReportAdapterModule.Implementation;
using Arash.Home.ReportAdapter.ReportAdapterModule.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Arash.Home.ReportAdapter
{
    public static class ConfigureReportManagerServices
    {
        public static IServiceCollection ConfigureReportManager(this IServiceCollection services, Func<ReportExecuteQueryRequest, Task<ReportExecuteQueryResponse>> executeQueryCustom = null, List<AdapterBase> adapters = default(List<AdapterBase>))
        {
            services.AddScoped<IQueryGeneratorService, QueryGeneratorService>();
            services.AddScoped<IExcelGenerator, ExcelGenerator.ExcelGenerator.ExcelGenerator>();
            services.AddScoped<IReportAdapterService, ReportAdapterService>(t =>
            {
                return new ReportAdapterService(t.GetRequiredService<IQueryGeneratorService>(), t.GetRequiredService<IExcelGenerator>(), t.GetRequiredService<DbContext>(), executeQueryCustom, adapters);
            });
            return services;
        }
    }
}
