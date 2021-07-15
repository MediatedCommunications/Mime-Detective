using FileDetection.Storage;
using System.Collections.Immutable;

namespace FileDetection.Data.Licensing {
    public interface IDefinitionBuilder {
        public ImmutableArray<Definition> Build();
    }

}
