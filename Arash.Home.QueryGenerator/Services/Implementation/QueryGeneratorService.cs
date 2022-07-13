using Arash.Home.QueryGenerator.Abstracts;
using Arash.Home.QueryGenerator.Exceptions;
using Arash.Home.QueryGenerator.Services.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel;

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
            model.IsForJson = request.Entity.IsForJson;
            var foreignKeys = entityType.GetForeignKeys();
            var invalidColumns = request.Entity.Fields.Where(a => entityType.GetProperties().All(m => (a.DependecyName?.Length > 0 || m.GetColumnName(StoreObjectIdentifier.Table(model.TableName, null)) != a.FieldName)) &&
            (a.DependecyName?.Length <= 0 || foreignKeys.Any(o => o.GetConstraintName() == a.DependecyName &&
            o.PrincipalEntityType.GetProperties().All(m => m.GetColumnName(StoreObjectIdentifier.Table(o.PrincipalEntityType.GetTableName(), null)) != a.FieldName))));
            if (invalidColumns.Count() > 0)
                throw new ColumnNotFoundException($"specified column is not found Columns : {string.Join(',', invalidColumns.Select(m => m.FieldName))}.");
            model.Fields = request.Entity.Fields.Select(m =>
            {
                if (m.DependecyName is not null)
                    m.DependecyName = foreignKeys.FirstOrDefault(o => o.GetConstraintName() == m.DependecyName).PrincipalEntityType.GetTableName();
                return new QueryFieldsModel
                {
                    DisplayName = m.DisplayName,
                    Name = m.FieldName,
                    ParentName = m.DependecyName ?? model.TableName,
                };
            }).ToList();
            var invalidDepndecies = request.Entity.Dependencies.Where(m => foreignKeys.All(o => o.GetConstraintName() != m.Name));
            if (invalidColumns.Count() > 0)
            {
                throw new ColumnNotFoundException($"specified dependency is not found dependencies : {(string.Join(',', invalidDepndecies.Select(m => m.Name)))}.");
            }
            model.Dependencies = request.Entity.Dependencies.Select(m =>
            {
                var key = foreignKeys.FirstOrDefault(o => o.GetConstraintName() == m.Name);
                var dependencyModel = new QueryDependencyModel
                {
                    DependencyName = key.PrincipalKey.DeclaringEntityType.GetTableName(),
                    LeftValue = key.PrincipalKey.Properties.FirstOrDefault().Name,
                    RightValue = key.Properties.FirstOrDefault().Name,
                    TableName = model.TableName
                };
                return dependencyModel;
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

        public async Task<QueryGetTableResponse> GetTables(QueryGetTableRequest request)
        {
            try
            {
                var entityTypes = dbContext.Model.GetEntityTypes();
                return new QueryGetTableResponse
                {
                    IsSuccess = true,
                    Entities = entityTypes.Select(a => new ViewModels.QueryTablesVm
                    {
                        Name = a.GetTableName(),
                        Fields = a.GetProperties().ToDictionary(m => m.GetColumnName(StoreObjectIdentifier.Table(a.GetTableName(), null)), m => (((DisplayNameAttribute)m.PropertyInfo.GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault()).DisplayName ?? m.GetColumnName(StoreObjectIdentifier.Table(a.GetTableName(), null)))),
                        Dependencies = a.GetForeignKeys().Select(o => new ViewModels.QueryDependencyVm
                        {
                            Name = o.GetConstraintName(),
                            TableName = o.PrincipalEntityType.GetTableName()
                        }).ToList()
                    }).ToList(),
                    Message = "operation success"
                };
            }
            catch (Exception ex)
            {
                return new QueryGetTableResponse
                {
                    Entities = null,
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}
