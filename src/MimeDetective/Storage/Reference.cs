using MimeDetective.Diagnostics;

namespace MimeDetective.Storage {
    public record Reference : DisplayRecord {
        public string? Text { get; init; }
        public string? Uri { get; init; }
    }
}
