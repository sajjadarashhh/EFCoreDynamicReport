using Arash.Home.ReportAdapter.ReportAdapterModule.Messaging;

namespace Arash.Home.ReportAdapter.ReportAdapterModule.Implementation
{
    public interface IReportAdapterService
    {
        Task<ReportCreateResponse> ReportCreate(ReportCreateRequest request);
        Task<ReportExecuteQueryResponse> ExecuteQuery(ReportExecuteQueryRequest request);
    }
}
