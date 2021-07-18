using System.Collections.Generic;
using System.Linq;

namespace MimeDetective.Engine {
    /// <summary>
    /// A specialized tree that allows quick searching of a prefix.  Null indicates that any byte should match.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class ByteTree<T> {
        public ByteTreeNode<T> Root { get; } = new();

        public List<T> Find(params byte[] Key) {
            return Root.Find(Key, 0).ToList();
        }

        public void Add(byte?[] Key, T Value) {
            Root.Add(Key, Value, 0);
        }


    }

}
