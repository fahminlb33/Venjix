using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venjix.Infrastructure.DataTables
{
    public interface IDataTables
    {
        Task<DataTablesResponseModel> PopulateTable<T>(DataTablesRequestModel request, DbSet<T> data) where T : class;

        Task<DataTablesResponseModel> PopulateTable<T>(DataTablesRequestModel request, DbSet<T> data, Func<T, T> project) where T : class;
    }

    public class DataTables : IDataTables
    {
        public Task<DataTablesResponseModel> PopulateTable<T>(DataTablesRequestModel request, DbSet<T> data) where T : class
        {
            return PopulateTable(request, data, null);
        }

        public async Task<DataTablesResponseModel> PopulateTable<T>(DataTablesRequestModel request, DbSet<T> data, Func<T, T> project) where T : class
        {
            // build select
            var sql = new StringBuilder();
            sql.AppendFormat("SELECT * FROM {0} ", GetTableName(data));

            // build where
            if (!string.IsNullOrEmpty(request.Search.Value))
            {
                sql.Append("WHERE ");
                foreach (var column in request.Columns)
                {
                    if (!column.Searchable) continue;
                    sql.AppendFormat("{0} LIKE '%{1}%' OR", column.Name, request.Search.Value);
                }

                sql.Remove(sql.Length - 2, 2);
            }

            // count filtered data
            var countSql = "SELECT COUNT(*) " + sql.ToString(9, sql.Length - 9);
            var filterCount = ExecuteScalar(data, countSql);

            // build order by
            if (request.Ordering.Count > 0)
            {
                sql.Append("ORDER BY ");
                foreach (var order in request.Ordering)
                {
                    sql.AppendFormat("{0} {1}, ", request.Columns[order.Column].Name, order.Direction == DataTablesOrdering.Ascending ? "ASC" : "DESC");
                }

                sql.Remove(sql.Length - 2, 2);
                sql.Append(' ');
            }

            // build paging
            sql.AppendFormat("LIMIT {0} OFFSET {1}", request.Length, request.Start);
            var sqlString = sql.ToString();

            var recordset = await data.FromSqlRaw(sqlString).ToListAsync();
            if (project != null)
            {
                recordset = recordset.Select(x => project(x)).ToList();
            }

            return new DataTablesResponseModel
            {
                Draw = request.Draw + 1,
                Data = recordset,
                RecordsFiltered = filterCount,
                RecordsTotal = await data.CountAsync()
            };
        }

        private long ExecuteScalar<T>(DbSet<T> set, string sql) where T : class
        {
            var context = GetDbContext(set);
            using var cmd = context.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = System.Data.CommandType.Text;
            context.Database.OpenConnection();

            var result = cmd.ExecuteScalar();
            return result == null ? 0 : (long)result;
        }

        private string GetTableName<T>(DbSet<T> set) where T : class
        {
            var dbContext = GetDbContext(set);

            var model = dbContext.Model;
            var entityTypes = model.GetEntityTypes();
            var entityType = entityTypes.First(t => t.ClrType == typeof(T));
            var tableNameAnnotation = entityType.GetAnnotation("Relational:TableName");
            var tableName = tableNameAnnotation.Value.ToString();
            return tableName;
        }

        private DbContext GetDbContext<T>(DbSet<T> dbSet) where T : class
        {
            var infrastructure = dbSet as IInfrastructure<IServiceProvider>;
            var serviceProvider = infrastructure.Instance;
            var currentDbContext = serviceProvider.GetService(typeof(ICurrentDbContext)) as ICurrentDbContext;
            return currentDbContext.Context;
        }
    }
}