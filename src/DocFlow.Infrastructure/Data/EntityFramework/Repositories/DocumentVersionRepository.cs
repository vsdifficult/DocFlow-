
using DocFlow.Application.Repositories;
using DocFlow.Domain.Entities;
using DocFlow.Infrastructure.Data.EntityFramework;

namespace DocFlow.Infrastructure.Data.EntityFramework.Repositories
{
    public class DocumentVersionRepository : EfRepository<DocumentVersionEntity>, IDocumentVersionRepository
    {
        public DocumentVersionRepository(DocFlowDbContext dbContext) : base(dbContext)
        {
        }
    }
}
