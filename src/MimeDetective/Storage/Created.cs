using MimeDetective.Diagnostics;
using System;

namespace MimeDetective.Storage;

/// <summary>
/// Contains metadata regarding the creation of the <see cref="Definition"/>.
/// </summary>
public record Created : DisplayRecord {
    /// <summary>
    /// When the <see cref="Definition"/> was created
    /// </summary>
    public DateTime? At { get; init; }

    /// <summary>
    /// The author of the <see cref="Definition"/>
    /// </summary>
    public Author? By { get; init; }

    /// <summary>
    /// The application that generated this <see cref="Definition"/>
    /// </summary>
    public Generator? Using { get; init; }

    /// <summary>
    /// Information regarding the source files used to create this <see cref="Definition"/>
    /// </summary>
    public Source? Source { get; init; }

    public override string? GetDebuggerDisplay() {
        return $@"{By?.GetDebuggerDisplay()} {Using?.GetDebuggerDisplay()} {Source?.GetDebuggerDisplay()}";
    }
}