using MimeDetective.Storage;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.Serialization;
using System.Text;

namespace MimeDetective.Definitions.Licensing {
    public enum UsageType {
        None,
        CommercialPaid,
        CommercialFree,
        PersonalNonCommercial,
    }

}
