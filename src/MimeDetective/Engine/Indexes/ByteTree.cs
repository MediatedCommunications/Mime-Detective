using System.Collections.Generic;
using System.Linq;

namespace MimeDetective.Engine;

/// <summary>
///     A specialized tree that allows quick searching of a prefix.  Null indicates that any byte should match.
/// </summary>
/// <typeparam name="T"></typeparam>
internal class ByteTree<T> {
    public ByteTreeNode<T> Root { get; } = new();

    public List<T> Find(params byte[] key) {
        return Root.Find(key, 0).ToList();
    }

    public void Add(byte?[] key, T value) {
        Root.Add(key, value, 0);
    }
}
