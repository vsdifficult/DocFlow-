
using DocFlow.Domain.Dtos;
using Microsoft.AspNetCore.Http;

namespace DocFlow.Application.Services.Interfaces
{
    public interface IDocumentService
    {
        Task<DocumentDto> CreateDocumentAsync(CreateDocumentDto createDocumentDto, Guid authorId);
        Task<DocumentDto> GetDocumentByIdAsync(Guid documentId);
        Task<IEnumerable<DocumentDto>> GetAllDocumentsAsync();
        Task<DocumentDto> UpdateDocumentAsync(Guid documentId, UpdateDocumentDto updateDocumentDto);
        Task DeleteDocumentAsync(Guid documentId);

        Task<DocumentDto> AddDocumentVersionAsync(Guid documentId, IFormFile file);
        Task<DocumentDto> ApproveDocumentAsync(Guid documentId, Guid approverId, string? comment);
        Task<DocumentDto> RejectDocumentAsync(Guid documentId, Guid approverId, string? comment);
    }
}
