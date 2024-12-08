using System.Collections.Generic;
using System.Linq;

namespace MimeDetective.Storage;

internal class SequenceComparer<TCollection, TElement> : IEqualityComparer<TCollection>, IComparer<TCollection>
    where TCollection : class, IEnumerable<TElement> {
    public static SequenceComparer<TCollection, TElement> Instance { get; } = new();

    public EqualityComparer<TElement> EqualityComparer { get; }
    public Comparer<TElement> Comparer { get; }

    public SequenceComparer() : this(Comparer<TElement>.Default, EqualityComparer<TElement>.Default) { }

    public SequenceComparer(Comparer<TElement> comparer, EqualityComparer<TElement> equalityComparer) {
        Comparer = comparer;
        EqualityComparer = equalityComparer;
    }

    public int Compare(TCollection? x, TCollection? y) {
        var ret = 0;

        if (x == y) {
            ret = 0;
        } else if (x is not null && y is null) {
            ret = -1;
        } else if (x is null && y is not null) {
            ret = +1;
        } else if (x is { } v1 && y is { } v2) {
            var ie1 = v1.GetEnumerator();
            var ie2 = v2.GetEnumerator();

            while (true) {
                var m1 = ie1.MoveNext();
                var m2 = ie2.MoveNext();

                if (m1 != m2) {
                    if (m1) {
                        ret = -1;
                        break;
                    }

                    ret = +1;
                    break;
                }

                if (m1) {
                    var c1 = ie1.Current;
                    var c2 = ie2.Current;

                    ret = Comparer.Compare(c1, c2);
                    if (ret != 0) {
                        break;
                    }
                } else {
                    ret = 0;
                    break;
                }
            }
        }


        return ret;
    }

    public bool Equals(TCollection? x, TCollection? y) {
        var ret = false;

        if (x == y) {
            ret = true;
        } else if (x is not null && y is not null) {
            ret = x.SequenceEqual(y);
        }

        return ret;
    }

    public int GetHashCode(TCollection obj) {
        var ret = 17;
        foreach (var element in obj) {
            if (element is not null) {
                ret = ret * 31 + EqualityComparer.GetHashCode(element);
            }
        }

        return ret;
    }
}
