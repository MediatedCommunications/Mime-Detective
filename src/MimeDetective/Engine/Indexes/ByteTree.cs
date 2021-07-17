using System.Collections.Generic;

namespace MimeDetective.Engine {
    /// <summary>
    /// A specialized tree that allows quick searching of a prefix.  Null indicates that any byte should match.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class ByteTree<T> {
        public ByteTreeNode<T> Root { get; } = new();

        public List<T> Find(params byte?[] Key) {
            var Current = Root;
            var ret = Root.ChildValues;

            for (int i = 0; i < Key.Length; i++) {
                var KeyValue = Key[i] ?? ByteTreeNode<T>.Null;
                var NextNode = Current.ChildNodes[KeyValue] ?? Current.ChildNodes[ByteTreeNode<T>.Null];
                if (NextNode == default) {
                    break;
                }

                Current = NextNode;

                ret = NextNode.ChildValues;

            }


            return ret;
        }

        public void Add(byte?[] Key, T Value) {
            Root.Add(Key, Value, 0);
        }


    }

}
