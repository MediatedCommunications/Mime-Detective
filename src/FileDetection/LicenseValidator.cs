using System;
using System.ComponentModel;

namespace FileDetection {
    internal class LicenseValidator {
        internal static void ThrowIfUnlicensed(FileDetectionEngine Engine) {
            if(DateTime.Now > new DateTime(2021, 11, 1)) {
                throw new LicenseException(typeof(FileDetectionEngine), Engine, "This library is not licensed.  Please update to the latest version or purchase a paid license.");
            }
        }
    }
}
