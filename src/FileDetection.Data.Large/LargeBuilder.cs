using FileDetection.Data.Licensing;
using FileDetection.Engine;
using FileDetection.Storage;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileDetection.Data
{
    public class LargeBuilder : IDefinitionBuilder
    {
        public UsageType UsageType { get; set; }

        public ImmutableArray<Definition> Build() {
            var Allowed = true
                && UsageType != UsageType.CommercialPaid
                && UsageType != UsageType.PersonalNonCommercial
                ;

            if (Allowed) {
                var Error = "Please change your usage type or visit https://mark0.net/soft-tridnet-e.html to purchase a license.";
                throw new UsageTypeNotAllowedException(UsageType, Error);
            }

            var raw = FileDetection.Data.Resources.data;
            var ret = FileDetection.Storage.DefinitionBinarySerializer
                .FromBinary(raw)
                .ToImmutableArray()
                ;

            return ret;

        }

    }

}
