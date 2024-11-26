using MimeDetective.Definitions.Licensing;
using MimeDetective.Storage;
using System.Collections.Immutable;
using System.IO;

namespace MimeDetective.Definitions;

public class ExhaustiveBuilder : DefinitionBuilder {
    public UsageType UsageType { get; set; }

    public override ImmutableArray<Definition> Build() {
        var AllowedUsageTypes = new[] {
            UsageType.CommercialPaid,
            UsageType.PersonalNonCommercial
        };

        var Error = "Please change your usage type or visit https://mark0.net/soft-tridnet-e.html to purchase a license.";

        EnsureValidUsageType(UsageType, AllowedUsageTypes, Error);

        var raw = MimeDetective.Definitions.Resources.data;
        var ret = MimeDetective.Storage.DefinitionBinarySerializer
                .FromBinary(new MemoryStream(raw))
                .ToImmutableArray()
            ;

        return ret;

    }

}