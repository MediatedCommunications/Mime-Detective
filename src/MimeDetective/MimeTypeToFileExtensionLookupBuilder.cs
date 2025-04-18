﻿using MimeDetective.Storage;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace MimeDetective;

public class MimeTypeToFileExtensionLookupBuilder {
    public IList<Definition> Definitions { get; set; } = [];

    public MimeTypeToFileExtensionLookup Build() {
        var defs = Definitions.ToImmutableArray();

        return new(defs);
    }
}
