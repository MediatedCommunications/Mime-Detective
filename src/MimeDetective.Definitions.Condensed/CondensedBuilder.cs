using MimeDetective.Definitions.Licensing;
using MimeDetective.Storage;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MimeDetective.Definitions
{
    public class CondensedBuilder : DefinitionBuilder
    {
        public UsageType UsageType { get; set; }

        public override ImmutableArray<Definition> Build()
        {
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
}
