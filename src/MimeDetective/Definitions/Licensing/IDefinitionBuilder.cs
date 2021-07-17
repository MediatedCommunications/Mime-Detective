using MimeDetective.Storage;
using System.Collections.Immutable;

namespace MimeDetective.Definitions.Licensing {
    public interface IDefinitionBuilder {
        public ImmutableArray<Definition> Build();
    }

}
