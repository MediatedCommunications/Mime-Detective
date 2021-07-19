using System;
using System.Collections.Generic;
using System.Text;

namespace MimeDetective.Engine {
    static internal class Array {
        static internal T[] Empty<T>() {
            return System.Array.Empty<T>();
        }

        static internal void Fill<T>(T[] ret, T value) {
            for (var i = 0; i < ret.Length; i++) {
                ret[i] = value;
            }
        }

        static internal int IndexOf(byte[] bytes, byte separator, int start) {
            return System.Array.IndexOf(bytes, separator, start);
        }
    }
}
