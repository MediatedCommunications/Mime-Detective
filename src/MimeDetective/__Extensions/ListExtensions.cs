namespace System.Collections.Generic;

internal static class ListExtensions {
    public static void Add<T>(this IList<T> This, IEnumerable<T> items) {
        foreach (var item in items) {
            This.Add(item);
        }
    }
}
