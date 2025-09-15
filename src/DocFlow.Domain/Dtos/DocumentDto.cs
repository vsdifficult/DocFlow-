

namespace DocFlow.Domain.Dtos;

public record DocumentDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Content { get; init; }
    public Guid UserId { get; init; }
}