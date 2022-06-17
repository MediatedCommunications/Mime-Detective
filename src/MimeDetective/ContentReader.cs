using System;
using System.IO;

namespace MimeDetective {
    public class ContentReader {

        /// <summary>
        /// The default <see cref="ContentReader"/> which will load up to 10MB from a stream.
        /// </summary>
        public static ContentReader Default { get; }

        /// <summary>
        /// An alternative <see cref="ContentReader"/>  which will load up to 2GB from a stream.
        /// </summary>
        public static ContentReader Max { get; }

        static ContentReader() {
            Default = new ContentReader() {
                MaxFileSize = 10 * 1024 * 1024,
            };

            Max = new ContentReader() { 
                MaxFileSize = int.MaxValue,
            };

        }

        public int MaxFileSize { get; init; }

        public byte[] ReadFromFile(string FileName) {
            using var FS = System.IO.File.OpenRead(FileName);

            return ReadFromStream(FS, false);
        }

        public byte[] ReadFromStream(Stream Input, bool ResetPosition = false) {
            var ret = ResetPosition
                ? FromStream_Reset_True(Input)
                : FromStream_Reset_False(Input)
                ;

            return ret;
        }

        public byte[] ReadFromStream(Func<Stream> Input) {
            using var Stream = Input();
            
            var ret = FromStream_Reset_False(Stream);

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
