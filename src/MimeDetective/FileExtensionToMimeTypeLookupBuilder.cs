using MimeDetective.Storage;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace MimeDetective;

public class FileExtensionToMimeTypeLookupBuilder {
    public IList<Definition> Definitions { get; set; } = new List<Definition>();

    public FileExtensionToMimeTypeLookup Build() {
        var defs = Definitions.ToImmutableArray();

        return new FileExtensionToMimeTypeLookup(defs);
    }

}