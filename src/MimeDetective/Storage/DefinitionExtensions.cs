using MimeDetective.Storage;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace MimeDetective.Storage {
    public static class DefinitionExtensions {

        /// <summary>
        /// Returns a copy of <paramref name="This"/> with no <see cref="Definition.Meta"/>
        /// </summary>
        /// <param name="This"></param>
        /// <returns></returns>
        public static IEnumerable<Definition> TrimMeta(this IEnumerable<Definition> This) {
            return This.Select(x => x.TrimMeta());
        }


        /// <summary>
        /// Returns a copy of <paramref name="This"/> with no <see cref="Definition.Meta"/>
        /// </summary>
        /// <param name="This"></param>
        /// <returns></returns>
        public static Definition TrimMeta(this Definition This) {

            var ret = This with {
                Meta = default,
            };

            return ret;
        }

        /// <summary>
        /// Returns a copy of <paramref name="This"/> with no <see cref="FileType.Description"/>
        /// </summary>
        /// <param name="This"></param>
        /// <returns></returns>
        public static IEnumerable<Definition> TrimDescription(this IEnumerable<Definition> This) {
            return This.Select(x => x.TrimDescription());
        }

        /// <summary>
        /// Returns a copy of <paramref name="This"/> with no <see cref="FileType.Description"/>
        /// </summary>
        /// <param name="This"></param>
        /// <returns></returns>
        public static Definition TrimDescription(this Definition This) {
            return This with {
                File = This.File with {
                    Description = default,
                }
            };
        }

        /// <summary>
        /// Returns a copy of <paramref name="This"/> with no <see cref="FileType.MimeType"/>
        /// </summary>
        /// <param name="This"></param>
        /// <returns></returns>
        public static IEnumerable<Definition> TrimMimeType(this IEnumerable<Definition> This) {
            return This.Select(x => x.TrimMimeType());
        }

        /// <summary>
        /// Returns a copy of <paramref name="This"/> with no <see cref="FileType.MimeType"/>
        /// </summary>
        /// <param name="This"></param>
        /// <returns></returns>
        public static Definition TrimMimeType(this Definition This) {
            return This with {
                File = This.File with {
                    MimeType = default,
                }
            };
        }


        /// <summary>
        /// Returns a copy of the <see cref="Definition"/>s whose <see cref="FileType.Extensions"/> are intersected with the provided <paramref name="Extensions"/>.  If a <see cref="Definition"/> would contain no <see cref="FileType.Extensions"/>, it is still returned.
        /// </summary>
        /// <param name="This"></param>
        /// <param name="Extensions"></param>
        /// <returns></returns>
        public static IEnumerable<Definition> TrimExtensions(this IEnumerable<Definition> This, IEnumerable<string> Extensions) {
            return TrimExtensions(This, Extensions.ToImmutableHashSet(StringComparer.InvariantCultureIgnoreCase));
        }

        private static IEnumerable<Definition> TrimExtensions(this IEnumerable<Definition> This, ImmutableHashSet<string> Extensions) {
            return This.Select(x => x.TrimExtensions(Extensions));
        }

        /// <summary>
        /// Returns a copy of the <see cref="Definition"/> whose <see cref="FileType.Extensions"/> are intersected with the provided <paramref name="Extensions"/>.  If a <see cref="Definition"/> would contain no <see cref="FileType.Extensions"/>, it is still returned.
        /// </summary>
        /// <param name="This"></param>
        /// <param name="Extensions"></param>
        /// <returns></returns>
        public static Definition TrimExtensions(this Definition This, IEnumerable<string> Extensions) {
            return TrimExtensions(This, Extensions.ToImmutableHashSet(StringComparer.InvariantCultureIgnoreCase));
        }

        private static Definition TrimExtensions(this Definition This, ImmutableHashSet<string> Extensions) {
            return This with {
                File = This.File with {
                    Extensions = Extensions
                        .Intersect(This.File.Extensions)
                        .ToImmutableArray()
                }
            };
        }

        /// <summary>
        /// Returns a copy of the <see cref="Definition"/>s whose <see cref="FileType.Extensions"/> are intersected with the provided <paramref name="Extensions"/>.  If a <see cref="Definition"/> would contain no <see cref="FileType.Extensions"/>, it is ommitted from the results.
        /// </summary>
        /// <param name="This"></param>
        /// <param name="Extensions"></param>
        /// <returns></returns>
        public static IEnumerable<Definition> ScopeExtensions(this IEnumerable<Definition> This, IEnumerable<string> Extensions) {
            return ScopeExtensions(This, Extensions.ToImmutableHashSet(StringComparer.InvariantCultureIgnoreCase));
        }

        private static IEnumerable<Definition> ScopeExtensions(this IEnumerable<Definition> This, ImmutableHashSet<string> Extensions) {
            return This.Select(x => x.ScopeExtensions(Extensions)).OfType<Definition>();
        }

        /// <summary>
        /// Returns a copy of the <see cref="Definition"/> whose <see cref="FileType.Extensions"/> are intersected with the provided <paramref name="Extensions"/>.  If this list would be empty, no value is returned.
        /// </summary>
        /// <param name="This"></param>
        /// <param name="Extensions"></param>
        /// <returns></returns>
        public static Definition? ScopeExtensions(this Definition This, IEnumerable<string> Extensions) {
            return TrimExtensions(This, Extensions.ToImmutableHashSet(StringComparer.InvariantCultureIgnoreCase));
        }

        private static Definition? ScopeExtensions(this Definition This, ImmutableHashSet<string> Extensions) {
            var tret = This.TrimExtensions(Extensions);

            var ret = tret.File.Extensions.Length > 0 ? tret : default;

            return ret;
        }


    }
}
