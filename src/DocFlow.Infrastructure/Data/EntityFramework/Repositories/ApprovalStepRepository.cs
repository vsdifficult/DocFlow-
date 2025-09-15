
using DocFlow.Application.Repositories;
using DocFlow.Domain.Entities;
using DocFlow.Infrastructure.Data.EntityFramework;

namespace DocFlow.Infrastructure.Data.EntityFramework.Repositories
{
    public class ApprovalStepRepository : EfRepository<ApprovalStepEntity>, IApprovalStepRepository
    {
        public ApprovalStepRepository(DocFlowDbContext dbContext) : base(dbContext)
        {
        }
    }
}
