using MyWebApi.Domain.Entities;
using MyWebApi.Domain.interfaces;
using Npgsql;
using Microsoft.Extensions.Configuration;

namespace MyWebApi.Infrastructure
{
    public class PeriodRepository : RepositoryBase<Period>, IRepository<Period>
    {
        public PeriodRepository(IConfiguration configuration) : base(configuration) { }

        public override async Task DeleteAsync(Guid id) =>
            await this.ExecuteSqlAsync($"DELETE FROM cr_bd.periods WHERE id='{id}'");

        public override async Task<IEnumerable<Period>> GetAllAsync() =>
            await this.ExecuteSqlReaderAsync("SELECT p.id, p.name, p.start_date, p.end_date FROM cr_bd.periods p");

        public override async Task<Period> GetByIdAsync(Guid id) =>
            (await this.ExecuteSqlReaderAsync($"SELECT p.id, p.name, p.start_date, p.end_date FROM cr_bd.periods p WHERE p.id='{id}'")).SingleOrDefault();

        public override async Task<Guid> InsertAsync(Period entity)
        {
            var newId = Guid.NewGuid();
            await this.ExecuteSqlAsync($"INSERT INTO cr_bd.periods (id, name, start_date, end_date) VALUES ('{newId}', '{entity.Name}', '{entity.StartDate:yyyy-MM-dd}', '{entity.EndDate:yyyy-MM-dd}')");
            return newId;
        }

        public override async Task UpdateAsync(Guid id, Period entity)
        {
            await this.ExecuteSqlAsync($"UPDATE cr_bd.periods SET name='{entity.Name}', start_date='{entity.StartDate:yyyy-MM-dd}', end_date='{entity.EndDate:yyyy-MM-dd}' WHERE id='{id}'");
        }

        protected override Period GetEntityFromReader(NpgsqlDataReader reader)
        {
            return new Period
            {
                Id = Guid.Parse(reader["id"].ToString()),
                Name = reader["name"].ToString(),
                StartDate = DateTime.Parse(reader["start_date"].ToString()),
                EndDate = DateTime.Parse(reader["end_date"].ToString())
            };
        }
    }
}
