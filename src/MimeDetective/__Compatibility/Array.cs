#if NETSTANDARD2_0 || NETSTANDARD2_1 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET45 || NET451 || NET452 || NET6 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48

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

#endif