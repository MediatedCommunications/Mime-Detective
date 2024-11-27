using System.Collections.Generic;

namespace MimeDetective.Engine;

internal class ByteTreeNode<T> {
    public const short NullStandinValue = byte.MaxValue + 1;

    public List<T> ChildValues { get; } = new();

    public ByteTreeNode<T>[] ChildNodes { get; } = new ByteTreeNode<T>[NullStandinValue + 1];

    public IEnumerable<T> Find(byte[] key, int index) {
        if (index >= key.Length) {
            var query = ChildValues;

            foreach (var result in query) {
                yield return result;
            }

        } else {


            var newValue = key[index];


            {
                if (ChildNodes[newValue] is { } v1) {
                    var query = v1.Find(key, index + 1);

                    foreach (var result in query) {
                        yield return result;
                    }
                }
            }

            {
                if (ChildNodes[NullStandinValue] is { } v1) {
                    var query = v1.Find(key, index + 1);

                    foreach (var result in query) {
                        yield return result;
                    }
                }
            }
        }

    }

    public void Add(byte?[] key, T value, int index) {

        ChildValues.Add(value);

        if (index < key.Length) {
            var newValue = key[index] ?? NullStandinValue;
            Add(key, value, index, newValue);
        }
    }

    private void Add(byte?[] key, T value, int index, short keyValue) {
        if (!(ChildNodes[keyValue] is { } nextNode)) {
            nextNode = new();
            ChildNodes[keyValue] = nextNode;
        }

        nextNode.Add(key, value, index + 1);

    }



}