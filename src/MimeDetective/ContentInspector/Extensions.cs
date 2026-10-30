using MimeDetective.Storage;
using System.Collections.Generic;
using System.IO;

namespace MimeDetective;

public static class Extensions {
    public static ContentInspector ToContentInspector(this IEnumerable<Definition> This) {
        var ret = ContentInspector.Create([.. This]);
        return ret;
    }

    public static ContentInspectorResults Inspect(this ContentInspector This, string FileName, ContentReader Reader) {
        var Content = Reader.ReadFromFile(FileName);
        return This.Inspect(Content);
    }

    public static ContentInspectorResults Inspect(this ContentInspector This, Stream Source, ContentReader Reader) {
        var Content = Reader.ReadFromStream(Source);
        return This.Inspect(Content);
    }
}
