
using Microsoft.AspNetCore.Http;

namespace DocFlow.Domain.Dtos
{
    public class CreateDocumentDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public IFormFile File { get; set; }
    }
}
