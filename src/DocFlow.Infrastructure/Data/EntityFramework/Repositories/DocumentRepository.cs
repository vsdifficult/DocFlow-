
using DocFlow.Application.Repositories;
using DocFlow.Domain.Entities;
using DocFlow.Infrastructure.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace DocFlow.Infrastructure.Data.EntityFramework.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly DocFlowDbContext _dbContext;

        public DocumentRepository(DocFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<DocumentEntity>> GetAllAsync()
        {
            return await _dbContext.Documents.ToListAsync();
        }

        public async Task<DocumentEntity?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Documents.FindAsync(id);
        }

        public async Task<Guid> CreateAsync(DocumentEntity entity)
        {
            await _dbContext.Documents.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<bool> UpdateAsync(DocumentEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _dbContext.Documents.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            _dbContext.Documents.Remove(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
