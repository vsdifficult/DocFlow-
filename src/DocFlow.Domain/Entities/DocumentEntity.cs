using DocFlow.Domain.Models;

namespace DocFlow.Domain.Entities;

public class DocumentEntity : BaseEntity
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public DocumentStatus Status { get; set; }
    public Guid AuthorId { get; set; }
    public UserEntity Author { get; set; }
    public ICollection<DocumentVersionEntity> Versions { get; set; } = new List<DocumentVersionEntity>();
    public ICollection<ApprovalStepEntity> ApprovalSteps { get; set; } = new List<ApprovalStepEntity>();
}
