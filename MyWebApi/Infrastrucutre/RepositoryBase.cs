using MyWebApi.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyWebApi.Infrastructure
{
    
    public abstract class RepositoryBase<T>
    {
        protected readonly string connectionString;

        public RepositoryBase(IConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            this.connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException("Ошибка конфигурации. Не заполнен параметр ConnectionStrings:DefaultConnection");
        }

        
        public abstract Task DeleteAsync(Guid id);
        public abstract Task<IEnumerable<T>> GetAllAsync();
        public abstract Task<T> GetByIdAsync(Guid id);
        public abstract Task<Guid> InsertAsync(T entity);
        public abstract Task UpdateAsync(Guid id, T entity);

        
        protected virtual async Task<IEnumerable<T>> ExecuteSqlReaderAsync(string sql)
        {
            var result = new List<T>();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var sqlCommand = new NpgsqlCommand(sql, connection))
                {
                    var reader = await sqlCommand.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        result.Add(this.GetEntityFromReader(reader));
                    }
                }
            }

            return result;
        }

        protected virtual async Task ExecuteSqlAsync(string sql)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var sqlCommand = new NpgsqlCommand(sql, connection))
                {
                    await sqlCommand.ExecuteNonQueryAsync();
                }
            }
        }

      
        protected abstract T GetEntityFromReader(NpgsqlDataReader reader);
    }
}
