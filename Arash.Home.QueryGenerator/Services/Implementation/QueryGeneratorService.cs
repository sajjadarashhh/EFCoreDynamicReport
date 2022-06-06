using Arash.Home.QueryGenerator.Abstracts;
using Arash.Home.QueryGenerator.Services.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Arash.Home.QueryGenerator.Services.Implementation
{
    public class QueryGeneratorService : IQueryGeneratorService
    {
        private readonly DbContext dbContext;

        public QueryGeneratorService(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<QueryGenerateResponse> GenerateQuery(QueryGenerateRequest request)
        {
            QueryGeneratorRequestModel model = new QueryGeneratorRequestModel();
            var entityType = dbContext.Model.GetEntityTypes().Where(a => a.GetTableName() == request.Entity.TableName).FirstOrDefault();
            model.TableName = entityType.GetTableName();
            model.Schema = entityType.GetSchema();
            model.Fields = request.Entity.Fields.Where(a => entityType.GetProperties().Any(m => m.GetColumnName(StoreObjectIdentifier.Table(model.TableName, null)) == a.FieldName)).Select(m => new QueryFieldsModel
            {
                DisplayName = m.DisplayName,
                Name = m.FieldName,
                ParentName = model.TableName
            }).ToList();

            return new QueryGenerateResponse
            {
                Entity = new ViewModels.QuerySQLVm
                {
                    Query = model.GenerateQuery(),
                },
                Message = "operation success.",
                IsSuccess = true,
            };
        }
    }
}
