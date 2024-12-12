using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MimeDetective.Storage;

public static class DefinitionExtensions {
    /// <summary>
    ///     Returns a copy of <paramref name="This" /> with no <see cref="Definition.Meta" />
    /// </summary>
    /// <param name="This"></param>
    /// <returns></returns>
    public static IEnumerable<Definition> TrimMeta(this IEnumerable<Definition> This) {
        return This.Select(x => x.TrimMeta());
    }


    /// <summary>
    ///     Returns a copy of <paramref name="This" /> with no <see cref="Definition.Meta" />
    /// </summary>
    /// <param name="This"></param>
    /// <returns></returns>
    public static Definition TrimMeta(this Definition This) {
        var ret = This with { Meta = default };

        return ret;
    }

    /// <summary>
    ///     Returns a copy of <paramref name="This" /> with no <see cref="FileType.Categories" />
    /// </summary>
    /// <param name="This"></param>
    /// <returns></returns>
    public static IEnumerable<Definition> TrimCategories(this IEnumerable<Definition> This) {
        return This.Select(x => x.TrimCategories());
    }


    /// <summary>
    ///     Returns a copy of <paramref name="This" /> with no <see cref="FileType.Categories" />
    /// </summary>
    /// <param name="This"></param>
    /// <returns></returns>
    public static Definition TrimCategories(this Definition This) {
        var ret = This with { File = This.File with { Categories = [] } };

        return ret;
    }

    /// <summary>
    ///     Returns a copy of <paramref name="This" /> with no <see cref="FileType.Description" />
    /// </summary>
    /// <param name="This"></param>
    /// <returns></returns>
    public static IEnumerable<Definition> TrimDescription(this IEnumerable<Definition> This) {
        return This.Select(x => x.TrimDescription());
    }

    /// <summary>
    ///     Returns a copy of <paramref name="This" /> with no <see cref="FileType.Description" />
    /// </summary>
    /// <param name="This"></param>
    /// <returns></returns>
    public static Definition TrimDescription(this Definition This) {
        return This with { File = This.File with { Description = default } };
    }

    /// <summary>
    ///     Returns a copy of <paramref name="This" /> with no <see cref="FileType.MimeType" />
    /// </summary>
    /// <param name="This"></param>
    /// <returns></returns>
    public static IEnumerable<Definition> TrimMimeType(this IEnumerable<Definition> This) {
        return This.Select(x => x.TrimMimeType());
    }

    /// <summary>
    ///     Returns a copy of <paramref name="This" /> with no <see cref="FileType.MimeType" />
    /// </summary>
    /// <param name="This"></param>
    /// <returns></returns>
    public static Definition TrimMimeType(this Definition This) {
        return This with { File = This.File with { MimeType = default } };
    }

    /// <summary>
    ///     Returns a copy of the <see cref="Definition" />s whose <see cref="FileType.Extensions" /> are intersected with the
    ///     provided <paramref name="extensions" />.  If a <see cref="Definition" /> would contain no
    ///     <see cref="FileType.Extensions" />, it is still returned.
    /// </summary>
    /// <param name="This"></param>
    /// <param name="extensions"></param>
    /// <returns></returns>
    public static IEnumerable<Definition> TrimExtensions(this IEnumerable<Definition> This, ISet<string> extensions) {
        return This.Select(x => x.TrimExtensions(extensions));
    }

    /// <summary>
    ///     Returns a copy of the <see cref="Definition" /> whose <see cref="FileType.Extensions" /> are intersected with the
    ///     provided <paramref name="extensions" />.  If a <see cref="Definition" /> would contain no
    ///     <see cref="FileType.Extensions" />, it is still returned.
    /// </summary>
    /// <param name="This"></param>
    /// <param name="extensions"></param>
    /// <returns></returns>
    public static Definition TrimExtensions(this Definition This, ISet<string> extensions) {
        return This with {
            File = This.File with {
                Extensions = This.File.Extensions
                    .Where(extensions.Contains)
                    .ToImmutableArray()
            }
        };
    }

    /// <summary>
    ///     Returns a copy of the <see cref="Definition" />s whose <see cref="FileType.Extensions" /> are intersected with the
    ///     provided <paramref name="extensions" />.  If a <see cref="Definition" /> would contain no
    ///     <see cref="FileType.Extensions" />, it is omitted from the results.
    /// </summary>
    /// <param name="This"></param>
    /// <param name="extensions"></param>
    /// <returns></returns>
    public static IEnumerable<Definition> ScopeExtensions(this IEnumerable<Definition> This, IEnumerable<string> extensions) {
        return ScopeExtensions(This, extensions.ToImmutableHashSet(StringComparer.InvariantCultureIgnoreCase));
    }

    private static IEnumerable<Definition> ScopeExtensions(this IEnumerable<Definition> This, ImmutableHashSet<string> extensions) {
        return This.Select(x => x.ScopeExtensions(extensions)).OfType<Definition>();
    }

    /// <summary>
    ///     Returns a copy of the <see cref="Definition" /> whose <see cref="FileType.Extensions" /> are intersected with the
    ///     provided <paramref name="extensions" />.  If this list would be empty, no value is returned.
    /// </summary>
    /// <param name="This"></param>
    /// <param name="extensions"></param>
    /// <returns></returns>
    public static Definition? ScopeExtensions(this Definition This, IEnumerable<string> extensions) {
        return TrimExtensions(This, extensions.ToImmutableHashSet(StringComparer.InvariantCultureIgnoreCase));
    }

    private static Definition? ScopeExtensions(this Definition This, ImmutableHashSet<string> extensions) {
        var tret = This.TrimExtensions(extensions);

        var ret = tret.File.Extensions.Length > 0 ? tret : default;

        return ret;
    }

    public static DefinitionDeduplicateResults Deduplicate(this IEnumerable<Definition> This) {
        var definitions = This.ToList();

        var extensionCache = (
                from x in definitions
                from y in x.File.Extensions
                select y.ToUpperInvariant()
            )
            .Distinct(StringComparer.InvariantCultureIgnoreCase)
            .ToImmutableDictionary(x => x, x => x, StringComparer.InvariantCultureIgnoreCase);

        var mimeTypeCache = (
                from x in definitions
                let v = x.File.MimeType?.ToUpperInvariant()
                where v is not null
                select v
            )
            .Distinct(StringComparer.InvariantCultureIgnoreCase)
            .ToImmutableDictionary(x => x, x => x, StringComparer.InvariantCultureIgnoreCase);

        var descriptionCache = (
                from x in definitions
                let v = x.File.Description
                where v is not null
                select v
            )
            .Distinct(StringComparer.InvariantCultureIgnoreCase)
            .ToImmutableDictionary(
                x => x,
                x => x,
                StringComparer.InvariantCultureIgnoreCase
            );

        var prefixCache = (
                from x in definitions
                from y in x.Signature.Prefix
                select y
            )
            .Distinct(PrefixSegmentEqualityComparer.Instance)
            .ToImmutableDictionary(
                x => x,
                x => x,
                PrefixSegmentEqualityComparer.Instance
            );

        var singularizedPrefixCache = (
                from x in prefixCache.Keys
                from y in x.Singularize()
                select y
            )
            .Distinct(PrefixSegmentEqualityComparer.Instance)
            .ToImmutableDictionary(
                x => x,
                x => x,
                PrefixSegmentEqualityComparer.Instance
            );

        var prefixCacheLookup =
            prefixCache.Keys
                .ToImmutableDictionary(
                    x => x,
                    x => x.Singularize().Select(y => singularizedPrefixCache[y]).ToImmutableArray(),
                    PrefixSegmentEqualityComparer.Instance
                );

        var stringCache = (
                from x in definitions
                from y in x.Signature.Strings
                select y
            )
            .Distinct(StringSegmentEqualityComparer.Instance)
            .ToImmutableDictionary(
                x => x,
                x => x,
                StringSegmentEqualityComparer.Instance
            );


        var categoryCache = (
                from x in definitions
                select x.File.Categories
            ).Distinct(SequenceComparer<ImmutableHashSet<Category>, Category>.Instance)
            .ToImmutableDictionary(
                x => x,
                x => x,
                SequenceComparer<ImmutableHashSet<Category>, Category>.Instance
            );


        var newDefinitions = (
            from Definition in definitions
            let Description = Definition.File.Description is { } v1 ? descriptionCache[v1] : default
            let Extensions = (
                from y in Definition.File.Extensions
                select extensionCache[y]
            ).ToImmutableArray()
            let MimeType = Definition.File.MimeType is { } v1 ? mimeTypeCache[v1] : default
            let Categories = categoryCache[Definition.File.Categories]
            let Prefixes = (
                from y in Definition.Signature.Prefix
                from z in prefixCacheLookup[y]
                select z
            ).ToImmutableArray()
            let Strings = (
                from y in Definition.Signature.Strings
                select stringCache[y]
            ).ToImmutableArray()
            let NewDef = new Definition {
                File = new() {
                    Description = Description,
                    Extensions = Extensions,
                    MimeType = MimeType,
                    Categories = Categories
                },
                Meta = Definition.Meta,
                Signature = new() {
                    Prefix = Prefixes,
                    Strings = Strings
                }
            }
            select NewDef
        ).ToImmutableArray();

        var ret = new DefinitionDeduplicateResults {
            Definitions = newDefinitions,
            Extensions = extensionCache,
            MimeTypes = mimeTypeCache,
            Descriptions = descriptionCache,
            Prefixes = singularizedPrefixCache,
            Strings = stringCache,
            Categories = categoryCache
        };

        return ret;
    }
}
