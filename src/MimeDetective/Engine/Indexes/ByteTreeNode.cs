using System.Collections.Generic;

namespace MimeDetective.Engine {
    internal class ByteTreeNode<T> {
        public const short NullStandinValue = byte.MaxValue + 1;

        public List<T> ChildValues { get; } = new();

        public ByteTreeNode<T>[] ChildNodes { get; } = new ByteTreeNode<T>[NullStandinValue + 1];

        public IEnumerable<T> Find(byte[] Key, int Index) {
            if (Index >= Key.Length) {
                var Query = ChildValues;

                foreach (var Result in Query) {
                    yield return Result;
                }

            } else {


                var NewValue = Key[Index];


                {
                    if (ChildNodes[NewValue] is { } V1) {
                        var Query = V1.Find(Key, Index + 1);

                        foreach (var Result in Query) {
                            yield return Result;
                        }
                    }
                }

                {
                    if (ChildNodes[NullStandinValue] is { } V1) {
                        var Query = V1.Find(Key, Index + 1);

                        foreach (var Result in Query) {
                            yield return Result;
                        }
                    }
                }
            }
            
        }

        public void Add(byte?[] Key, T Value, int Index) {

            ChildValues.Add(Value);

            if (Index < Key.Length) {
                var NewValue = Key[Index] ?? NullStandinValue;
                Add(Key, Value, Index, NewValue);
            }
        }

        private void Add(byte?[] Key, T Value, int Index, short KeyValue) {
            if (!(ChildNodes[KeyValue] is { } NextNode)) {
                NextNode = new();
                ChildNodes[KeyValue] = NextNode;
            }

            NextNode.Add(Key, Value, Index + 1);

        }



    }

}
