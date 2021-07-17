using System;
using System.Collections.Generic;
using System.Text;

namespace MimeDetective.Engine {
    internal static class Array {
        internal static T[] Empty<T>() {
            return System.Array.Empty<T>();
        }

        internal static void Fill<T>(T[] ret, T value) {
            for (var i = 0; i < ret.Length; i++) {
                ret[i] = value;
            }
        }

        internal static int IndexOf(byte[] bytes, byte separator, int start) {
            return System.Array.IndexOf(bytes, separator, start);
        }
    }
}
