
using DocFlow.Domain.Entities;

namespace DocFlow.Application.Repositories
{
    public interface IAuditLogRepository : IRepository<AuditLogEntity, Guid>
    {
    }
}
