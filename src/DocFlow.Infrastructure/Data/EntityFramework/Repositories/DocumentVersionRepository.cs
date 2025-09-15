
using DocFlow.Application.Repositories;
using DocFlow.Domain.Entities;
using DocFlow.Infrastructure.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace DocFlow.Infrastructure.Data.EntityFramework.Repositories
{
    public class DocumentVersionRepository : IDocumentVersionRepository
    {
        private readonly DocFlowDbContext _dbContext;

        public DocumentVersionRepository(DocFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<DocumentVersionEntity>> GetAllAsync()
        {
            return await _dbContext.DocumentVersions.ToListAsync();
        }

        public async Task<DocumentVersionEntity?> GetByIdAsync(Guid id)
        {
            return await _dbContext.DocumentVersions.FindAsync(id);
        }

        public async Task<Guid> CreateAsync(DocumentVersionEntity entity)
        {
            await _dbContext.DocumentVersions.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<bool> UpdateAsync(DocumentVersionEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _dbContext.DocumentVersions.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            _dbContext.DocumentVersions.Remove(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
