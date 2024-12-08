using System;
using System.Collections.Generic;
using System.Linq;

namespace MimeDetective.Definitions.Licensing;

public class UsageTypeNotAllowedException : Exception {
    public UsageTypeNotAllowedException(UsageType invalidUsageType, UsageType[] allowedUsageTypes, string? message = default) :
        base(CreateMessage(invalidUsageType, allowedUsageTypes, message)) { }

    private static string CreateMessage(UsageType usageType, UsageType[] allowedUsageTypes, string? message) {
        var allowedUsageString = string.Join(", ", allowedUsageTypes.Select(x => x.ToString()));

        var lines = new List<string?> {
            $@"{usageType.GetType().Name} '{usageType}' is not allowed.",
            $@"Acceptable usage types are: {{ {allowedUsageString} }}",
            message
        };

        lines = lines.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

        var ret = string.Join("\n", lines);

        return ret;
    }
}
