namespace System.Collections.Generic;

static internal class ListExtensions {
    public static void Add<T>(this IList<T> This, IEnumerable<T> Items) {
        foreach (var item in Items) {
            This.Add(item);
        }
    }
}