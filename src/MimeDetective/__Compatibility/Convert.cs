#if NETSTANDARD2_0 || NETSTANDARD2_1 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET45 || NET451 || NET452 || NET6 || NET461 || NET462 || NET47 || NET471 || NET472 || NET48

using System.Text;

namespace System {
    static internal class Convert {

        public static string ToHexString(byte[] input) {
            return ByteArrayToHexString(input);
        }

        public static byte[] FromHexString(string input) {
            return HexStringToByteArray(input);
        }


        private static string ByteArrayToHexString(byte[] bytes) {
            var result = new StringBuilder(bytes.Length * 2);
            const string hexAlphabet = "0123456789ABCDEF";

            foreach (var b in bytes) {
                result.Append(hexAlphabet[b >> 4]);
                result.Append(hexAlphabet[b & 0xF]);
            }

            return result.ToString();
        }

        private static byte[] HexStringToByteArray(string hex) {
            var bytes = new byte[hex.Length / 2];
            var hexValue = new int[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05,
       0x06, 0x07, 0x08, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
       0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };

            for (int x = 0, i = 0; i < hex.Length; i += 2, x += 1) {
                bytes[x] = (byte)(hexValue[char.ToUpperInvariant(hex[i + 0]) - '0'] << 4 |
                                  hexValue[char.ToUpperInvariant(hex[i + 1]) - '0']);
            }

            return bytes;
        }


    }
}

#endif