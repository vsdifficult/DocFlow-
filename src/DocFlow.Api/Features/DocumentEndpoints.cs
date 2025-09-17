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
        var documentsGroup = app.MapGroup("/api/documents").RequireAuthorization();

        documentsGroup.MapPost("/", async (IDocumentService documentService, CreateDocumentDto createDocumentDto, HttpContext httpContext) =>
        {
            var userId = GetUserId(httpContext);
            var document = await documentService.CreateDocumentAsync(createDocumentDto, userId);
            return Results.Created($"/api/documents/{document.Id}", document);
        });

        documentsGroup.MapGet("/{id}", async (IDocumentService documentService, Guid id) =>
        {
            var document = await documentService.GetDocumentByIdAsync(id);
            return document is not null ? Results.Ok(document) : Results.NotFound();
        });

        documentsGroup.MapGet("/", async (IDocumentService documentService) =>
        {
            var documents = await documentService.GetAllDocumentsAsync();
            return Results.Ok(documents);
        });

        documentsGroup.MapPut("/{id}", async (IDocumentService documentService, Guid id, UpdateDocumentDto updateDocumentDto) =>
        {
            var document = await documentService.UpdateDocumentAsync(id, updateDocumentDto);
            return Results.Ok(document);
        });

        documentsGroup.MapDelete("/{id}", async (IDocumentService documentService, Guid id) =>
        {
            await documentService.DeleteDocumentAsync(id);
            return Results.NoContent();
        });

        var versionsGroup = documentsGroup.MapGroup("/{id}/versions");

        versionsGroup.MapPost("/", async (IDocumentService documentService, Guid id, IFormFile file) =>
        {
            var document = await documentService.AddDocumentVersionAsync(id, file);
            return Results.Ok(document);
        });

        var workflowGroup = documentsGroup.MapGroup("/{id}/workflow");

        workflowGroup.MapPost("/approve", async (IDocumentService documentService, Guid id, string? comment, HttpContext httpContext) =>
        {
            var userId = GetUserId(httpContext);
            var document = await documentService.ApproveDocumentAsync(id, userId, comment);
            return Results.Ok(document);
        });

        workflowGroup.MapPost("/reject", async (IDocumentService documentService, Guid id, string? comment, HttpContext httpContext) =>
        {
            var userId = GetUserId(httpContext);
            var document = await documentService.RejectDocumentAsync(id, userId, comment);
            return Results.Ok(document);
        });
    }

    private static Guid GetUserId(HttpContext httpContext)
    {
        var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        return userIdClaim is not null ? Guid.Parse(userIdClaim.Value) : Guid.Empty;
    }
}
