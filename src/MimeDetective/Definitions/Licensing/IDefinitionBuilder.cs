using MimeDetective.Storage;
using System.Collections.Immutable;
using System.Linq;

namespace MimeDetective.Definitions.Licensing {
    public interface IDefinitionBuilder {
        public ImmutableArray<Definition> Build();
    }

    public abstract class DefinitionBuilder : IDefinitionBuilder {

        protected static void EnsureValidUsageType(UsageType Type, UsageType[] AllowedTypes, string Error) {

            var Allowed = false
                || AllowedTypes.Contains(Type)
                ;

            if (!Allowed) {
                throw new UsageTypeNotAllowedException(Type, AllowedTypes, Error);
            }
        }

        public abstract ImmutableArray<Definition> Build();
    }

}
