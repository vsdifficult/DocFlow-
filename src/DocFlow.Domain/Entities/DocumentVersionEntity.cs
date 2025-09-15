
namespace DocFlow.Domain.Entities;

public class DocumentVersionEntity : BaseEntity
{
    public int VersionNumber { get; set; }
    public string FilePath { get; set; }
    public Guid DocumentId { get; set; }
    public DocumentEntity Document { get; set; }
}

