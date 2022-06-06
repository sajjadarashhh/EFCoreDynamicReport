﻿using Arash.Home.QueryGenerator.Abstracts;
using Arash.Home.QueryGenerator.Exceptions;
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
            if (entityType is null)
                throw new TableNotFoundException("specified table is not found.");
            model.TableName = entityType.GetTableName();
            model.Schema = entityType.GetSchema();
            var invalidColumns = request.Entity.Fields.Where(a => entityType.GetProperties().All(m => m.GetColumnName(StoreObjectIdentifier.Table(model.TableName, null)) != a.FieldName));
            if (invalidColumns.Count()>0)
                throw new ColumnNotFoundException($"specified column is not found Columns : {(string.Join(',', invalidColumns.Select(m => m.FieldName)))}.");
            model.Fields = request.Entity.Fields.Select(m => new QueryFieldsModel
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
