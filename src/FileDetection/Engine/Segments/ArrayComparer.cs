using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace FileDetection.Engine {
    public class ArrayComparer<T> : IEqualityComparer<IEnumerable<T>>, IComparer<IEnumerable<T>> {

        public static ArrayComparer<T> Instance { get; } = new();

        public EqualityComparer<T> EqualityComparer { get; }
        public Comparer<T> Comparer { get; }

        public ArrayComparer() : this(Comparer<T>.Default, EqualityComparer<T>.Default) {

        }

        public ArrayComparer(Comparer<T> Comparer, EqualityComparer<T> EqualityComparer) {
            this.Comparer = Comparer;
            this.EqualityComparer = EqualityComparer;
        }

        public bool Equals(IEnumerable<T>? x, IEnumerable<T>? y) {
            var ret = false;

            if(x == y) {
                ret = true;
            } else if (x is { } && y is { }) {
                ret = x.SequenceEqual(y);
            }

            return ret;
        }

        public int GetHashCode([DisallowNull] IEnumerable<T> obj) {
            var ret = 17;
            foreach (var element in obj) {

                if(element is { }) {
                    ret = ret * 31 + EqualityComparer.GetHashCode(element);
                }
                
            }
            return ret;
        }

        public int Compare(IEnumerable<T>? x, IEnumerable<T>? y) {
            var ret = 0;

            if(x == y) {
                ret = 0;
            } else if(x is { } && y is null) {
                ret = -1;
            } else if (x is null && y is { }) {
                ret = +1;
            } else if (x is { } V1 && y is { } V2) {
                var IE1 = V1.GetEnumerator();
                var IE2 = V2.GetEnumerator();

                while (true) {
                    var M1 = IE1.MoveNext();
                    var M2 = IE2.MoveNext();

                    if(M1 != M2) {
                        if (M1) {
                            ret = -1;
                            break;
                        } else {
                            ret = +1;
                            break;
                        }
                    } else {
                        if (M1) {
                            var C1 = IE1.Current;
                            var C2 = IE2.Current;

                            ret = Comparer.Compare(C1, C2);
                            if(ret != 0) {
                                break;
                            }

                        } else {
                            ret = 0;
                            break;
                        }
                    }

                }
            }



            return ret;
        }
    }

}
