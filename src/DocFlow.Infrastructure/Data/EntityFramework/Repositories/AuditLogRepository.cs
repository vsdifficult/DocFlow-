
using DocFlow.Application.Repositories;
using DocFlow.Domain.Entities;
using DocFlow.Infrastructure.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace DocFlow.Infrastructure.Data.EntityFramework.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly DocFlowDbContext _dbContext;

        public AuditLogRepository(DocFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<AuditLogEntity>> GetAllAsync()
        {
            return await _dbContext.AuditLogs.ToListAsync();
        }

        public async Task<AuditLogEntity?> GetByIdAsync(Guid id)
        {
            return await _dbContext.AuditLogs.FindAsync(id);
        }

        public async Task<Guid> CreateAsync(AuditLogEntity entity)
        {
            await _dbContext.AuditLogs.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<bool> UpdateAsync(AuditLogEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _dbContext.AuditLogs.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            _dbContext.AuditLogs.Remove(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
