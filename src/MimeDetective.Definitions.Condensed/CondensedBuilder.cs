using MimeDetective.Definitions.Licensing;
using MimeDetective.Storage;
using System.Collections.Immutable;
using System.IO;

namespace MimeDetective.Definitions;

public class CondensedBuilder : DefinitionBuilder {
    public UsageType UsageType { get; set; }

    public override ImmutableArray<Definition> Build() {
        var allowedUsageTypes = new[] {
            UsageType.CommercialPaid,
            UsageType.PersonalNonCommercial
        };

        var error = "Please change your usage type or visit https://mark0.net/soft-tridnet-e.html to purchase a license.";

        EnsureValidUsageType(UsageType, allowedUsageTypes, error);

        var raw = MimeDetective.Definitions.Resources.data;
        var ret = MimeDetective.Storage.DefinitionBinarySerializer
                .FromBinary(new MemoryStream(raw))
                .ToImmutableArray()
            ;

        return ret;

    }

}