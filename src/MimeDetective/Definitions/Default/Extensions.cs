using System.Collections.Generic;

namespace MimeDetective.Definitions {
    static internal class Extensions {
        public static void Add<T>(this IList<T> This, IEnumerable<T> Items) {
            foreach (var item in Items) {
                This.Add(item);
            }
        }
    }
}