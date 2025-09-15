
using DocFlow.Domain.Entities;

namespace DocFlow.Application.Repositories
{
    public interface IDocumentRepository : IRepository<DocumentEntity, Guid>
    {
        // Add document specific methods here
    }
}
