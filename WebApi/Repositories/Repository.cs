using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;

namespace WebApi.Repositories
{
    public class Repository : IRepository
    {
        private readonly string sqlConnectionString;

        public Repository(string sqlConnectionString)
        {
            this.sqlConnectionString = sqlConnectionString;
        }

        public async Task<IEnumerable<WebApi>> GetAllAsync()
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<WebApi>("SELECT * FROM [Environment2D]");
            }
        }

        public async Task<WebApi> GetByIdAsync(Guid id)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QuerySingleOrDefaultAsync<WebApi>("SELECT * FROM [Environment2D] WHERE Id = @Id", new { id });
            }
        }

        public async Task<WebApi> AddAsync(WebApi WebApi)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("INSERT INTO [Environment2D] (Id, Name, OwnerUserId, MaxHeight, MaxLength) VALUES (@Id, @Name, @OwnerUserId, @MaxHeight, @MaxLength)", WebApi);
                return WebApi;
            }
        }

        public async Task UpdateAsync(WebApi WebApi)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("UPDATE [Environment2D] SET " +
                                                 "Name = @Name, " +
                                                 "OwnerUserId = @OwnerUserId, " +
                                                 "MaxHeight = @MaxHeight, " +
                                                 "MaxLength = @MaxLength " +
                                                 "WHERE Id = @Id", WebApi);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("DELETE FROM [Environment2D] WHERE Id = @Id", new { id });
            }
        }
    }
}