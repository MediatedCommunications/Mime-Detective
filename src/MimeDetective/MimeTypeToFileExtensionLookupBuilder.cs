using MimeDetective.Storage;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace MimeDetective {
    public class MimeTypeToFileExtensionLookupBuilder {
        public IList<Definition> Definitions { get; set; } = new List<Definition>();

        public MimeTypeToFileExtensionLookup Build() {
            var defs = Definitions.ToImmutableArray();

            return new MimeTypeToFileExtensionLookup(defs);
        }

    }
}
