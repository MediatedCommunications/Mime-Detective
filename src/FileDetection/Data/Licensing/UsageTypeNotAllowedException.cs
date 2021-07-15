using System;

namespace FileDetection.Data.Licensing {
    public class UsageTypeNotAllowedException : Exception {
        public UsageTypeNotAllowedException(UsageType Type, string? Message = default) : base(CreateMessage(Type, Message)) {

        }

        private static string CreateMessage(UsageType UsageType, string? Message) {
            var ret = $@"{UsageType.GetType().Name} '{UsageType}' is not allowed.";

            if (!string.IsNullOrWhiteSpace(Message)) {
                ret += $"\n{Message}";
            }

            return ret;
        }
    }

}
