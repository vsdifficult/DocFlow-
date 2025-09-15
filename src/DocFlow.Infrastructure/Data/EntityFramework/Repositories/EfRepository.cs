
using System.Collections.Generic;
using System.Threading.Tasks;
using DocFlow.Application.Repositories;
using DocFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DocFlow.Infrastructure.Data.EntityFramework.Repositories
{
    public class EfRepository<T> : IRepository<T, Guid> where T : BaseEntity
    {
        protected readonly DocFlowDbContext _dbContext;

        public EfRepository(DocFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        { 
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<Guid> CreateAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _dbContext.Set<T>().FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            _dbContext.Set<T>().Remove(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
