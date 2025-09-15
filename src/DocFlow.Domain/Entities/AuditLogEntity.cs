
namespace DocFlow.Domain.Entities;

public class AuditLogEntity : BaseEntity
{
    public Guid UserId { get; set; }
    public string Action { get; set; }
    public string Entity { get; set; }
    public Guid EntityId { get; set; }
    public string? Changes { get; set; }
}

