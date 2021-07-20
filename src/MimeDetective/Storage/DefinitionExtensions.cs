using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

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
        /// Returns a copy of <paramref name="This"/> with no <see cref="FileType.Categories"/>
        /// </summary>
        /// <param name="This"></param>
        /// <returns></returns>
        public static IEnumerable<Definition> TrimCategories(this IEnumerable<Definition> This) {
            return This.Select(x => x.TrimCategories());
        }


        /// <summary>
        /// Returns a copy of <paramref name="This"/> with no <see cref="FileType.Categories"/>
        /// </summary>
        /// <param name="This"></param>
        /// <returns></returns>
        public static Definition TrimCategories(this Definition This) {

            var ret = This with {
                File = This.File with {
                    Categories = ImmutableHashSet<Category>.Empty,
                }
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

        public static DefinitionDeduplicateResults Deduplicate(this IEnumerable<Definition> This) {
            var Definitions = This.ToList();

            var ExtensionCache = (
                from x in Definitions
                from y in x.File.Extensions
                select y.ToLower()
                )
                .Distinct(StringComparer.InvariantCultureIgnoreCase)
                .ToImmutableDictionary(x => x, x => x, StringComparer.InvariantCultureIgnoreCase);

            var MimeTypeCache = (
                from x in Definitions
                let v = x.File.MimeType?.ToLower()
                where v is { }
                select v
                )
                .Distinct(StringComparer.InvariantCultureIgnoreCase)
                .ToImmutableDictionary(x => x, x => x, StringComparer.InvariantCultureIgnoreCase);

            var DescriptionCache = (
                from x in Definitions
                let v = x.File.Description
                where v is { }
                select v
                )
                .Distinct(StringComparer.InvariantCultureIgnoreCase)
                .ToImmutableDictionary(
                x => x,
                x => x,
                StringComparer.InvariantCultureIgnoreCase
                );

            var PrefixCache = (
                from x in Definitions
                from y in x.Signature.Prefix
                select y
                )
                .Distinct(PrefixSegmentEqualityComparer.Instance)
                .ToImmutableDictionary(
                x => x,
                x => x,
                PrefixSegmentEqualityComparer.Instance
                );

            var SingularizedPrefixCache = (
                from x in PrefixCache.Keys
                from y in x.Singularize()
                select y
                )
                .Distinct(PrefixSegmentEqualityComparer.Instance)
                .ToImmutableDictionary(
                x => x,
                x => x,
                PrefixSegmentEqualityComparer.Instance
                );

            var PrefixCacheLookup =
                PrefixCache.Keys
                .ToImmutableDictionary(
                x => x,
                x => x.Singularize().Select(y => SingularizedPrefixCache[y]).ToImmutableArray(),
                PrefixSegmentEqualityComparer.Instance
                );
            
            var StringCache = (
                 from x in Definitions
                 from y in x.Signature.Strings
                 select y
                )
                .Distinct(StringSegmentEqualityComparer.Instance)
                .ToImmutableDictionary(
                x => x,
                x => x,
                StringSegmentEqualityComparer.Instance
                );


            var CategoryCache = (
                from x in Definitions
                select x.File.Categories
                ).Distinct(SequenceComparer<ImmutableHashSet<Category>, Category>.Instance)
                .ToImmutableDictionary(
                x => x,
                x => x,
                SequenceComparer<ImmutableHashSet<Category>, Category>.Instance
                );


            var NewDefinitions = (
                from Definition in Definitions

                let Description = Definition.File.Description is { } V1 ? DescriptionCache[V1] : default

                let Extensions = (
                    from y in Definition.File.Extensions
                    select ExtensionCache[y]
                ).ToImmutableArray()

                let MimeType = Definition.File.MimeType is { } V1 ? MimeTypeCache[V1] : default

                let Categories = CategoryCache[Definition.File.Categories]

                let Prefixes = (
                    from y in Definition.Signature.Prefix
                    from z in PrefixCacheLookup[y]
                    select z
                    ).ToImmutableArray()

                let Strings = (
                    from y in Definition.Signature.Strings
                    select StringCache[y]
                    ).ToImmutableArray()

                let NewDef = new Definition() {
                   File = new FileType() {
                       Description = Description,
                       Extensions = Extensions,
                       MimeType = MimeType,
                       Categories = Categories,
                   },
                   Meta = Definition.Meta,
                   Signature = new Signature() {
                       Prefix = Prefixes,
                       Strings = Strings,
                   }
               }
               select NewDef
            ).ToImmutableArray();

            var ret = new DefinitionDeduplicateResults() {
                Definitions = NewDefinitions,
                Extensions = ExtensionCache,
                MimeTypes = MimeTypeCache,
                Descriptions = DescriptionCache,
                Prefixes = SingularizedPrefixCache,
                Strings = StringCache,
                Categories = CategoryCache,
            };

            return ret;
        }

    }
}
