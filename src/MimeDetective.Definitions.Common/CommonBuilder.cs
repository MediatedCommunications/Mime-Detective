using MimeDetective.Definitions.Licensing;
using MimeDetective.Storage;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MimeDetective.Definitions
{
    public class CommonBuilder : IDefinitionBuilder
    {
        public UsageType UsageType { get; set; }

        public ImmutableArray<Definition> Build()
        {
            var Allowed = true
                && UsageType != UsageType.CommercialPaid
                && UsageType != UsageType.PersonalNonCommercial
                ;

            if (Allowed) {
                var Error = "Please change your usage type or visit https://mark0.net/soft-tridnet-e.html to purchase a license.";
                throw new UsageTypeNotAllowedException(UsageType, Error);
            }

            var raw = MimeDetective.Definitions.Resources.data;
            var ret = MimeDetective.Storage.DefinitionBinarySerializer
                .FromBinary(raw)
                .ToImmutableArray()
                ;

            return ret;

        }

    }
}
