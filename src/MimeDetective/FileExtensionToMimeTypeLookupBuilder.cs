using MimeDetective.Storage;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace MimeDetective;

public class FileExtensionToMimeTypeLookupBuilder {
    public IList<Definition> Definitions { get; set; } = [];

    public FileExtensionToMimeTypeLookup Build() {
        var defs = Definitions.ToImmutableArray();

        return new(defs);
    }
}
