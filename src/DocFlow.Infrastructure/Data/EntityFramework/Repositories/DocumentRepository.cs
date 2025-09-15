
using DocFlow.Application.Repositories;
using DocFlow.Domain.Entities;
using DocFlow.Infrastructure.Data.EntityFramework;

namespace DocFlow.Infrastructure.Data.EntityFramework.Repositories
{
    public class DocumentRepository : EfRepository<DocumentEntity>, IDocumentRepository
    {
        public DocumentRepository(DocFlowDbContext dbContext) : base(dbContext)
        {
        }
    }
}
