using FileDetection.Storage;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.Serialization;
using System.Text;

namespace FileDetection.Data.Licensing {
    public enum UsageType {
        None,
        CommercialPaid,
        CommercialFree,
        PersonalNonCommercial,
    }

}
