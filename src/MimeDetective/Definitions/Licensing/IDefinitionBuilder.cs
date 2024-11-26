using MimeDetective.Storage;
using System.Collections.Immutable;
using System.Linq;

namespace MimeDetective.Definitions.Licensing;

public interface IDefinitionBuilder {
    public ImmutableArray<Definition> Build();
}

public abstract class DefinitionBuilder : IDefinitionBuilder {

    protected static void EnsureValidUsageType(UsageType type, UsageType[] allowedTypes, string error) {

        var allowed = false
                || allowedTypes.Contains(type)
            ;

        if (!allowed) {
            throw new UsageTypeNotAllowedException(type, allowedTypes, error);
        }
    }

    public abstract ImmutableArray<Definition> Build();
}