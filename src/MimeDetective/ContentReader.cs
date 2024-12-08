using System;
using System.IO;

namespace MimeDetective;

public class ContentReader {
    /// <summary>
    ///     The default <see cref="ContentReader" /> which will load up to 10MB from a stream.
    /// </summary>
    public static ContentReader Default { get; }

    /// <summary>
    ///     An alternative <see cref="ContentReader" />  which will load up to 2GB from a stream.
    /// </summary>
    public static ContentReader Max { get; }

    /// <summary>
    ///     An alternative <see cref="ContentReader" />  which will load up to 1KB from a stream.
    /// </summary>
    public static ContentReader Min { get; }

    public int MaxFileSize { get; init; }

    static ContentReader() {
        Default = new() { MaxFileSize = 10 * 1024 * 1024 };

        Min = new() { MaxFileSize = 1024 };

        Max = new() { MaxFileSize = int.MaxValue };
    }

    public byte[] ReadFromFile(string fileName) {
        using var fs = File.OpenRead(fileName);

        return ReadFromStream(fs);
    }

    public byte[] ReadFromStream(Stream input, bool resetPosition = false) {
        var ret = resetPosition
                ? FromStream_Reset_True(input)
                : FromStream_Reset_False(input)
            ;

        return ret;
    }

    public byte[] ReadFromStream(Func<Stream> input) {
        using var stream = input();

        var ret = FromStream_Reset_False(stream);

        return ret;
    }

    protected byte[] FromStream_Reset_True(Stream input) {
        var position = input.Position;

        var r = new BinaryReader(input);
        var ret = r.ReadBytes(MaxFileSize);

        input.Position = position;

        return ret;
    }

    protected byte[] FromStream_Reset_False(Stream input) {
        var r = new BinaryReader(input);
        var ret = r.ReadBytes(MaxFileSize);

        return ret;
    }
}
