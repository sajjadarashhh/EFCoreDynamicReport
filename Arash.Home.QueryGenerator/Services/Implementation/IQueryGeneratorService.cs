using Arash.Home.QueryGenerator.Services.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arash.Home.QueryGenerator.Services.Implementation
{
    public interface IQueryGeneratorService
    {
        Task<QueryGenerateResponse> GenerateQuery(QueryGenerateRequest request);
        Task<QueryGetTableResponse> GetTables(QueryGetTableRequest request);
    }
}
