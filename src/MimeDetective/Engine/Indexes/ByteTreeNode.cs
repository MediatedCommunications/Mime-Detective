using System.Collections.Generic;

namespace MimeDetective.Engine {
    internal class ByteTreeNode<T> {
        public const short Null = (short)byte.MaxValue + 1;

        public List<T> ChildValues { get; } = new();

        public ByteTreeNode<T>[] ChildNodes { get; } = new ByteTreeNode<T>[Null + 1];

        public void Add(byte?[] Key, T Value, int i) {

            ChildValues.Add(Value);

            if (i < Key.Length) {
                if (Key[i] is { } KeyValue) {
                    Add(Key, Value, i, KeyValue);
                }

                Add(Key, Value, i, Null);
            }
        }

        private void Add(byte?[] Key, T Value, int i, short KeyValue) {
            if (!(ChildNodes[KeyValue] is { } NextNode)) {
                NextNode = new();
                ChildNodes[KeyValue] = NextNode;
            }

            NextNode.Add(Key, Value, i + 1);

        }



    }

}
