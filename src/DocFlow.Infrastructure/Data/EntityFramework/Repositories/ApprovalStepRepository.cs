
using DocFlow.Application.Repositories;
using DocFlow.Domain.Entities;
using DocFlow.Infrastructure.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace DocFlow.Infrastructure.Data.EntityFramework.Repositories
{
    public class ApprovalStepRepository : IApprovalStepRepository
    {
        private readonly DocFlowDbContext _dbContext;

        public ApprovalStepRepository(DocFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ApprovalStepEntity>> GetAllAsync()
        {
            return await _dbContext.ApprovalSteps.ToListAsync();
        }

        public async Task<ApprovalStepEntity?> GetByIdAsync(Guid id)
        {
            return await _dbContext.ApprovalSteps.FindAsync(id);
        }

        public async Task<Guid> CreateAsync(ApprovalStepEntity entity)
        {
            await _dbContext.ApprovalSteps.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<bool> UpdateAsync(ApprovalStepEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _dbContext.ApprovalSteps.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            _dbContext.ApprovalSteps.Remove(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
