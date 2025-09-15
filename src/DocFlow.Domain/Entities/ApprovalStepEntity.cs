
using DocFlow.Domain.Models;

namespace DocFlow.Domain.Entities
{
    public class ApprovalStepEntity : BaseEntity
    {
        public int Step { get; set; }
        public ApprovalStatus Status { get; set; }
        public string? Comment { get; set; }
        public Guid ApproverId { get; set; }
        public UserEntity Approver { get; set; }
        public Guid DocumentId { get; set; }
        public DocumentEntity Document { get; set; }
    }
}
