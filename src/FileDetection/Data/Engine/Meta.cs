using FileDetection.Data.Diagnostics;

namespace FileDetection.Data.Engine
{
    public record Meta : DisplayRecord
    {
        public string? Id { get; init; }
        public Reference? Reference { get; init; }
        public Created? Created { get; init; }
    }
}
