namespace System.Collections.Generic;

internal static class ListExtensions {
    public static void Add<T>(this IList<T> @this, IEnumerable<T> items) {
        foreach (var item in items) {
            @this.Add(item);
        }
    }
}
