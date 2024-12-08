using MimeDetective.Diagnostics;

namespace MimeDetective.Storage;

public record Meta : DisplayRecord {
    public string? Id { get; init; }
    public Reference? Reference { get; init; }
    public Created? Created { get; init; }
}
