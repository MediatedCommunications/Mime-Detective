using System.IO;

namespace FileDetection {
    public class ContentReader {
        public int MaxFileSize { get; init; } = 10 * 1024 * 1024;

        public static ContentReader Default { get; } = new();

        public byte[] ReadFromFile(string FileName) {
            using var FS = System.IO.File.OpenRead(FileName);

            return ReadFromStream(FS, false);
        }

        public byte[] ReadFromStream(Stream Input, bool ResetPosition = true) {
            var ret = ResetPosition
                ? FromStream_Reset_True(Input)
                : FromStream_Reset_False(Input)
                ;

            return ret;
        }

        protected byte[] FromStream_Reset_True(Stream Input) {
            var Position = Input.Position;

            var R = new BinaryReader(Input);
            var ret = R.ReadBytes(MaxFileSize);

            Input.Position = Position;

            return ret;
        }

        protected byte[] FromStream_Reset_False(Stream Input) {
            var R = new BinaryReader(Input);
            var ret = R.ReadBytes(MaxFileSize);

            return ret;
        }

    }
}
