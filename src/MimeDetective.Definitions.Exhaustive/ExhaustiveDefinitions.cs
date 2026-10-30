
using MimeDetective.Storage;
using System.Collections.Immutable;
using System.IO;

namespace MimeDetective;

public static class ExhaustiveDefinitions {
    public static Definition[] Load(UsageType UsageType) {
        var allowedUsageTypes = new[] { UsageType.CommercialPaid, UsageType.PersonalNonCommercial, };

        var error = "Please change your usage type or visit https://mark0.net/soft-tridnet-e.html to purchase a license.";

        UsageTypeNotAllowedException.EnsureValidUsageType(UsageType, allowedUsageTypes, error);

        var raw = MimeDetective.Definitions.Exhaustive.Resources.data;
        var ret = DefinitionBinarySerializer
            .FromBinary(new MemoryStream(raw))
            ;

        return ret;
    }
}
