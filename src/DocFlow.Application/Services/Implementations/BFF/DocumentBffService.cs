using DocFlow.Application.Repositories;
using DocFlow.Application.Services.Interfaces;
using DocFlow.Domain.Dtos;
using DocFlow.Domain.Entities;
using DocFlow.Domain.Models;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace DocFlow.Application.Services.Implementations.BFF
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IDocumentVersionRepository _documentVersionRepository;
        private readonly IApprovalStepRepository _approvalStepRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IFileStorageService _fileStorageService;

        public DocumentService(
            IDocumentRepository documentRepository,
            IDocumentVersionRepository documentVersionRepository,
            IApprovalStepRepository approvalStepRepository,
            IAuditLogRepository auditLogRepository,
            IFileStorageService fileStorageService)
        {
            _documentRepository = documentRepository;
            _documentVersionRepository = documentVersionRepository;
            _approvalStepRepository = approvalStepRepository;
            _auditLogRepository = auditLogRepository;
            _fileStorageService = fileStorageService;
        }

        public async Task<DocumentDto> CreateDocumentAsync(CreateDocumentDto createDocumentDto, Guid authorId)
        {
            var filePath = await _fileStorageService.SaveFileAsync(createDocumentDto.File);

            var document = new DocumentEntity
            {
                Title = createDocumentDto.Title,
                Description = createDocumentDto.Description,
                AuthorId = authorId,
                Status = DocumentStatus.Draft,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var documentId = await _documentRepository.CreateAsync(document);

            var documentVersion = new DocumentVersionEntity
            {
                DocumentId = documentId,
                FilePath = filePath,
                VersionNumber = 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _documentVersionRepository.CreateAsync(documentVersion);

            await _auditLogRepository.CreateAsync(new AuditLogEntity
            {
                UserId = authorId,
                Action = "Create Document",
                Entity = "Document",
                EntityId = documentId,
                CreatedAt = DateTime.UtcNow
            });

            return new DocumentDto
            {
                Id = document.Id,
                Name = document.Title,
                Content = string.Empty, // Or some other representation
                UserId = document.AuthorId
            };
        }

        public async Task<DocumentDto> GetDocumentByIdAsync(Guid documentId)
        {
            var document = await _documentRepository.GetByIdAsync(documentId);
            if (document == null)
            {
                throw new Exception("Document not found");
            }

            return new DocumentDto
            {
                Id = document.Id,
                Name = document.Title,
                Content = string.Empty, // Or some other representation
                UserId = document.AuthorId
            };
        }

        public async Task<IEnumerable<DocumentDto>> GetAllDocumentsAsync()
        {
            var documents = await _documentRepository.GetAllAsync();
            return documents.Select(document => new DocumentDto
            {
                Id = document.Id,
                Name = document.Title,
                Content = string.Empty, // Or some other representation
                UserId = document.AuthorId
            });
        }

        public async Task<DocumentDto> UpdateDocumentAsync(Guid documentId, UpdateDocumentDto updateDocumentDto)
        {
            var document = await _documentRepository.GetByIdAsync(documentId);
            if (document == null)
            {
                throw new Exception("Document not found");
            }

            document.Title = updateDocumentDto.Title ?? document.Title;
            document.Description = updateDocumentDto.Description ?? document.Description;
            document.UpdatedAt = DateTime.UtcNow;

            await _documentRepository.UpdateAsync(document);

            await _auditLogRepository.CreateAsync(new AuditLogEntity
            {
                UserId = Guid.Empty, // Should be the current user
                Action = "Update Document",
                Entity = "Document",
                EntityId = documentId,
                Changes = JsonSerializer.Serialize(updateDocumentDto),
                CreatedAt = DateTime.UtcNow
            });

            return new DocumentDto
            {
                Id = document.Id,
                Name = document.Title,
                Content = string.Empty,
                UserId = document.AuthorId
            };
        }

        public async Task DeleteDocumentAsync(Guid documentId)
        {
            var document = await _documentRepository.GetByIdAsync(documentId);
            if (document == null)
            {
                throw new Exception("Document not found");
            }

            await _documentRepository.DeleteAsync(documentId);

            await _auditLogRepository.CreateAsync(new AuditLogEntity
            {
                UserId = Guid.Empty, // Should be the current user
                Action = "Delete Document",
                Entity = "Document",
                EntityId = documentId,
                CreatedAt = DateTime.UtcNow
            });
        }

        public async Task<DocumentDto> AddDocumentVersionAsync(Guid documentId, IFormFile file)
        {
            var document = await _documentRepository.GetByIdAsync(documentId);
            if (document == null)
            {
                throw new Exception("Document not found");
            }

            var filePath = await _fileStorageService.SaveFileAsync(file);
            var latestVersion = document.Versions.Max(v => (int?)v.VersionNumber) ?? 0;

            var documentVersion = new DocumentVersionEntity
            {
                DocumentId = documentId,
                FilePath = filePath,
                VersionNumber = latestVersion + 1,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _documentVersionRepository.CreateAsync(documentVersion);

            document.UpdatedAt = DateTime.UtcNow;
            await _documentRepository.UpdateAsync(document);

            await _auditLogRepository.CreateAsync(new AuditLogEntity
            {
                UserId = Guid.Empty, // Should be the current user
                Action = "Add Document Version",
                Entity = "Document",
                EntityId = documentId,
                CreatedAt = DateTime.UtcNow
            });

            return new DocumentDto
            {
                Id = document.Id,
                Name = document.Title,
                Content = string.Empty,
                UserId = document.AuthorId
            };
        }

        public async Task<DocumentDto> ApproveDocumentAsync(Guid documentId, Guid approverId, string? comment)
        {
            var document = await _documentRepository.GetByIdAsync(documentId);
            if (document == null)
            {
                throw new Exception("Document not found");
            }

            var approvalStep = document.ApprovalSteps
                .FirstOrDefault(s => s.ApproverId == approverId && s.Status == ApprovalStatus.Pending);

            if (approvalStep == null)
            {
                throw new Exception("No pending approval step for this user.");
            }

            approvalStep.Status = ApprovalStatus.Approved;
            approvalStep.Comment = comment;
            approvalStep.UpdatedAt = DateTime.UtcNow;

            await _approvalStepRepository.UpdateAsync(approvalStep);

            var allApproved = document.ApprovalSteps.All(s => s.Status == ApprovalStatus.Approved);
            if (allApproved)
            {
                document.Status = DocumentStatus.Approved;
                await _documentRepository.UpdateAsync(document);
            }

            await _auditLogRepository.CreateAsync(new AuditLogEntity
            {
                UserId = approverId,
                Action = "Approve Document",
                Entity = "Document",
                EntityId = documentId,
                Changes = JsonSerializer.Serialize(new { Comment = comment }),
                CreatedAt = DateTime.UtcNow
            });
            
            return new DocumentDto
            {
                Id = document.Id,
                Name = document.Title,
                Content = string.Empty,
                UserId = document.AuthorId
            };
        }

        public async Task<DocumentDto> RejectDocumentAsync(Guid documentId, Guid approverId, string? comment)
        {
            var document = await _documentRepository.GetByIdAsync(documentId);
            if (document == null)
            {
                throw new Exception("Document not found");
            }

            var approvalStep = document.ApprovalSteps
                .FirstOrDefault(s => s.ApproverId == approverId && s.Status == ApprovalStatus.Pending);

            if (approvalStep == null)
            {
                throw new Exception("No pending approval step for this user.");
            }

            approvalStep.Status = ApprovalStatus.Rejected;
            approvalStep.Comment = comment;
            approvalStep.UpdatedAt = DateTime.UtcNow;

            await _approvalStepRepository.UpdateAsync(approvalStep);

            document.Status = DocumentStatus.Rejected;
            await _documentRepository.UpdateAsync(document);

            await _auditLogRepository.CreateAsync(new AuditLogEntity
            {
                UserId = approverId,
                Action = "Reject Document",
                Entity = "Document",
                EntityId = documentId,
                Changes = JsonSerializer.Serialize(new { Comment = comment }),
                CreatedAt = DateTime.UtcNow
            });

            return new DocumentDto
            {
                Id = document.Id,
                Name = document.Title,
                Content = string.Empty,
                UserId = document.AuthorId
            };
        }
    }
}