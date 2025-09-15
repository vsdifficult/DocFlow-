
using DocFlow.Application.Repositories;
using DocFlow.Domain.Entities;
using DocFlow.Infrastructure.Data.EntityFramework;

namespace DocFlow.Infrastructure.Data.EntityFramework.Repositories
{
    public class AuditLogRepository : EfRepository<AuditLogEntity>, IAuditLogRepository
    {
        public AuditLogRepository(DocFlowDbContext dbContext) : base(dbContext)
        {
        }
    }
}
