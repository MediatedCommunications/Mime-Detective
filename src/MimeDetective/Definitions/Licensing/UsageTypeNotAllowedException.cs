using System;
using System.Collections.Generic;
using System.Linq;

namespace MimeDetective.Definitions.Licensing;

public class UsageTypeNotAllowedException : Exception {
    public UsageTypeNotAllowedException(UsageType InvalidUsageType, UsageType[] AllowedUsageTypes, string? Message = default) : base(CreateMessage(InvalidUsageType, AllowedUsageTypes, Message)) {

    }

    private static string CreateMessage(UsageType UsageType, UsageType[] AllowedUsageTypes, string? Message) {
        var AllowedUsageString = string.Join(", ", AllowedUsageTypes.Select(x => x.ToString()));

        var Lines = new List<string?> {
            $@"{UsageType.GetType().Name} '{UsageType}' is not allowed.",
            $@"Acceptable usage types are: {{ {AllowedUsageString} }}",
            Message
        };

        Lines = Lines.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

        var ret = string.Join("\n", Lines);

        return ret;
    }
}