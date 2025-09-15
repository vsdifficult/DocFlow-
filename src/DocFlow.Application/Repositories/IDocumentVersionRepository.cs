
using DocFlow.Domain.Entities;

namespace DocFlow.Application.Repositories
{
    public interface IDocumentVersionRepository : IRepository<DocumentVersionEntity, Guid>
    {
    }
}
