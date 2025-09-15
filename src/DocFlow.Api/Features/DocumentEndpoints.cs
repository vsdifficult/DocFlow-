using DocFlow.Application.Services.Interfaces;
using DocFlow.Domain.Dtos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;

namespace DocFlow.Api.Features;

public static class DocumentEndpoints
{
    public static void MapDocumentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/documents").RequireAuthorization();

        group.MapPost("/", async (IDocumentService documentService, CreateDocumentDto createDocumentDto, HttpContext httpContext) =>
        {
            var userId = GetUserId(httpContext);
            var document = await documentService.CreateDocumentAsync(createDocumentDto, userId);
            return Results.Created($"/api/documents/{document.Id}", document);
        });

        group.MapGet("/{id}", async (IDocumentService documentService, Guid id) =>
        {
            var document = await documentService.GetDocumentByIdAsync(id);
            return document is not null ? Results.Ok(document) : Results.NotFound();
        });

        group.MapGet("/", async (IDocumentService documentService) =>
        {
            var documents = await documentService.GetAllDocumentsAsync();
            return Results.Ok(documents);
        });

        group.MapPut("/{id}", async (IDocumentService documentService, Guid id, UpdateDocumentDto updateDocumentDto) =>
        {
            var document = await documentService.UpdateDocumentAsync(id, updateDocumentDto);
            return Results.Ok(document);
        });

        group.MapDelete("/{id}", async (IDocumentService documentService, Guid id) =>
        {
            await documentService.DeleteDocumentAsync(id);
            return Results.NoContent();
        });

        group.MapPost("/{id}/versions", async (IDocumentService documentService, Guid id, IFormFile file) =>
        {
            var document = await documentService.AddDocumentVersionAsync(id, file);
            return Results.Ok(document);
        });

        group.MapPost("/{id}/approve", async (IDocumentService documentService, Guid id, string? comment, HttpContext httpContext) =>
        {
            var userId = GetUserId(httpContext);
            var document = await documentService.ApproveDocumentAsync(id, userId, comment);
            return Results.Ok(document);
        });

        group.MapPost("/{id}/reject", async (IDocumentService documentService, Guid id, string? comment, HttpContext httpContext) =>
        {
            var userId = GetUserId(httpContext);
            var document = await documentService.RejectDocumentAsync(id, userId, comment);
            return Results.Ok(document);
        });
    }

    private static Guid GetUserId(HttpContext httpContext)
    {
        // This is a placeholder. In a real application, you would get the user ID from the claims.
        // For example: return Guid.Parse(httpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        return Guid.NewGuid(); 
    }
}
