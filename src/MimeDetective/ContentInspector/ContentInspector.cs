using MimeDetective.Storage;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace MimeDetective;

/// <summary>
///     Matches definitions by walking a decision tree of their offset rules, then keeping
///     only definitions whose wildcard values all occur somewhere in the content.
/// </summary>
public sealed class ContentInspector {
    private readonly Node _root;

    /// <summary>
    ///     Stores the decision tree produced by <see cref="Create" />.
    /// </summary>
    /// <param name="root">The root of the offset-rule decision tree.</param>
    private ContentInspector(Node root) {
        _root = root;
    }

    /// <summary>
    ///     Builds an inspector whose decision tree is derived from <paramref name="definitions" />.
    ///     Each definition contributes only its <see cref="OffsetRule" />s, expanded with
    ///     <see cref="OffsetRuleExtensions2.ToByteRules(ImmutableArray{OffsetRule})" />.
    ///     <see cref="WildCardRule" />s are applied later by <see cref="Inspect" />. A definition that
    ///     requires two different bytes at the same offset is omitted because it cannot match any content.
    /// </summary>
    /// <param name="definitions">The definitions to index.</param>
    /// <returns>An inspector that can <see cref="Inspect" /> content against those definitions.</returns>
    public static ContentInspector Create(ImmutableArray<Definition> definitions) {
        var entries = new List<Entry>(definitions.Length);

        for (var i = 0; i < definitions.Length; i++) {
            if (!TryGetRules(definitions[i], out var rules)) {
                continue;
            }

            entries.Add(new Entry(definitions[i], i, rules, 0));
        }

        return new ContentInspector(Build(entries));
    }

    /// <summary>
    ///     Walks the decision tree and returns every definition whose offset rules all match
    ///     <paramref name="content" /> and whose wildcard values each occur somewhere in that content.
    ///     Definitions are returned in their original input order. A definition with no offset rules
    ///     matches every input at the offset stage. A definition with no wildcard rules passes the
    ///     wildcard stage.
    /// </summary>
    /// <param name="content">The bytes to test.</param>
    /// <returns>The matching definitions.</returns>
    public ContentInspectorResults Inspect(ReadOnlySpan<byte> content) {
        var tret = InspectInternal(content)
            .ToResults()
            ;

        return tret;
    }

    private ImmutableArray<Definition> InspectInternal(ReadOnlySpan<byte> content) {
        var hits = new List<Hit>();
        Collect(_root, content, hits);
        hits.Sort(static (left, right) => left.Index.CompareTo(right.Index));

        var definitions = new List<Definition>(hits.Count);
        foreach (var hit in hits) {
            if (MatchesWildcards(hit.Definition, content)) {
                definitions.Add(hit.Definition);
            }
        }

        return [.. definitions,];
    }

    /// <summary>
    ///     Reports whether every wildcard value on <paramref name="definition" /> occurs somewhere in
    ///     <paramref name="content" />. The value may start at any offset. A definition with no wildcard
    ///     rules matches.
    /// </summary>
    /// <param name="definition">A definition that already satisfied its offset rules.</param>
    /// <param name="content">The bytes to test.</param>
    /// <returns><see langword="true" /> when every wildcard value is present.</returns>
    private static bool MatchesWildcards(Definition definition, ReadOnlySpan<byte> content) {
        foreach (var rule in definition.Signature.WildCardRules) {
            if (content.IndexOf(rule.Bytes.AsSpan()) < 0) {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    ///     Expands <paramref name="definition" />'s offset rules into one byte rule per byte, sorted by
    ///     offset. Duplicate rules are removed. Returns <see langword="false" /> when two rules demand
    ///     different bytes at the same offset.
    /// </summary>
    /// <param name="definition">The definition whose offset rules are expanded. Wildcard rules are not read.</param>
    /// <param name="rules">The normalized byte rules, or an empty array when the definition conflicts with itself.</param>
    /// <returns><see langword="true" /> when the definition can match some content.</returns>
    private static bool TryGetRules(Definition definition, out ByteRule[] rules) {
        rules = [
            .. definition.Signature.OffsetRules
                .ToByteRules()
                .Distinct()
                .OrderBy(static rule => rule.Start),
        ];

        if (HasConflictingOffset(rules)) {
            rules = [];
            return false;
        }

        return true;
    }

    /// <summary>
    ///     Reports whether <paramref name="rules" /> requires two different bytes at one offset.
    ///     The rules are already distinct and sorted by offset, so a repeated offset is a conflict.
    /// </summary>
    /// <param name="rules">Normalized byte rules.</param>
    /// <returns><see langword="true" /> when two adjacent rules share an offset.</returns>
    private static bool HasConflictingOffset(ByteRule[] rules) {

        for (var i = 1; i < rules.Length; i++) {
            if (rules[i - 1].Start == rules[i].Start) {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    ///     Builds one tree node from <paramref name="entries" />. Definitions with no remaining byte
    ///     rules are stored on the node. The other definitions are split on the earliest remaining
    ///     offset: one child per required byte, plus a child for definitions that do not constrain
    ///     that offset.
    /// </summary>
    /// <param name="entries">Definitions still waiting to be placed, with the index of their next byte rule.</param>
    /// <returns>The node for this subset of definitions.</returns>
    private static Node Build(List<Entry> entries) {
        var (matches, pending) = Partition(entries);
        if (pending.Count == 0) {
            return new Node { Matches = matches };
        }

        var offset = EarliestOffset(pending);
        var byByte = new Dictionary<byte, List<Entry>>();
        var unconstrained = new List<Entry>();
        Distribute(pending, offset, byByte, unconstrained);

        return new Node {
            Offset = offset,
            Matches = matches,
            Children = byByte.Count == 0 ? null : BuildChildren(byByte),
            Unconstrained = unconstrained.Count == 0 ? null : Build(unconstrained),
        };
    }

    /// <summary>
    ///     Separates definitions that already satisfy every byte rule from definitions that still have rules left.
    /// </summary>
    /// <param name="entries">Definitions waiting to be placed.</param>
    /// <returns>Finished matches, and the definitions that still constrain a later offset.</returns>
    private static (Hit[] Matches, List<Entry> Pending) Partition(List<Entry> entries) {
        var finished = new List<Hit>();
        var pending = new List<Entry>(entries.Count);

        foreach (var entry in entries) {
            if (entry.IsComplete) {
                finished.Add(entry.ToHit());
            } else {
                pending.Add(entry);
            }
        }

        var matches = finished.Count == 0 ? [] : finished.ToArray();
        return (matches, pending);
    }

    /// <summary>
    ///     Returns the smallest offset still required by <paramref name="pending" />.
    /// </summary>
    /// <param name="pending">Definitions that still have at least one byte rule.</param>
    /// <returns>The offset the next tree node tests.</returns>
    private static int EarliestOffset(List<Entry> pending) {
        var offset = int.MaxValue;
        foreach (var entry in pending) {
            var start = entry.Current.Start;
            if (start < offset) {
                offset = start;
            }
        }

        return offset;
    }

    /// <summary>
    ///     Groups definitions that require a byte at <paramref name="offset" /> by that byte.
    ///     Definitions whose next rule is at a later offset are left unconstrained.
    /// </summary>
    /// <param name="pending">Definitions that still have at least one byte rule.</param>
    /// <param name="offset">The offset this node tests.</param>
    /// <param name="byByte">Receives each definition under the byte it requires, advanced to its next rule.</param>
    /// <param name="unconstrained">Receives definitions that do not constrain <paramref name="offset" />.</param>
    private static void Distribute(
        List<Entry> pending,
        int offset,
        Dictionary<byte, List<Entry>> byByte,
        List<Entry> unconstrained) {
        foreach (var entry in pending) {
            var rule = entry.Current;
            if (rule.Start == offset) {
                AddToGroup(byByte, rule.Value, entry.Next());
            } else {
                unconstrained.Add(entry);
            }
        }
    }

    /// <summary>
    ///     Adds <paramref name="entry" /> to the list stored under <paramref name="value" />, creating that list when needed.
    /// </summary>
    private static void AddToGroup(Dictionary<byte, List<Entry>> byByte, byte value, Entry entry) {
        if (!byByte.TryGetValue(value, out var group)) {
            group = [];
            byByte.Add(value, group);
        }

        group.Add(entry);
    }

    /// <summary>
    ///     Builds the child node for each required byte.
    /// </summary>
    /// <param name="groups">Definitions grouped by the byte they require at the current offset.</param>
    /// <returns>A child tree for each byte.</returns>
    private static Dictionary<byte, Node> BuildChildren(Dictionary<byte, List<Entry>> groups) {
        var children = new Dictionary<byte, Node>(groups.Count);
        foreach (var group in groups) {
            children.Add(group.Key, Build(group.Value));
        }

        return children;
    }

    /// <summary>
    ///     Collects definitions that have already satisfied every byte rule, then follows the child
    ///     for <paramref name="content" /> at this node's offset and the child for definitions that
    ///     do not care about that offset. A byte child is skipped when the content is shorter than
    ///     the offset.
    /// </summary>
    /// <param name="node">The tree node currently being visited.</param>
    /// <param name="content">The bytes being tested.</param>
    /// <param name="hits">The matches accumulated so far.</param>
    private static void Collect(Node node, ReadOnlySpan<byte> content, List<Hit> hits) {
        if (node.Matches.Length > 0) {
            hits.AddRange(node.Matches);
        }

        if (node.Unconstrained is not null) {
            Collect(node.Unconstrained, content, hits);
        }

        if (node.Children is null || !ContainsOffset(content, node.Offset)) {
            return;
        }

        if (node.Children.TryGetValue(content[node.Offset], out var child)) {
            Collect(child, content, hits);
        }
    }

    /// <summary>
    ///     Reports whether <paramref name="offset" /> refers to a byte inside <paramref name="content" />.
    ///     A negative offset is out of range.
    /// </summary>
    /// <param name="content">The bytes being tested.</param>
    /// <param name="offset">The file offset a node wants to read.</param>
    /// <returns><see langword="true" /> when the offset can be read.</returns>
    private static bool ContainsOffset(ReadOnlySpan<byte> content, int offset) {
        return (uint)offset < (uint)content.Length;
    }

    private sealed class Node {
        public int Offset { get; init; } = -1;
        public Hit[] Matches { get; init; } = [];
        public Dictionary<byte, Node>? Children { get; init; }
        public Node? Unconstrained { get; init; }
    }

    private readonly record struct Entry(Definition Definition, int Index, ByteRule[] Rules, int Position) {
        public bool IsComplete => Position >= Rules.Length;

        public ByteRule Current => Rules[Position];

        /// <summary>
        ///     Returns this entry advanced to its next byte rule.
        /// </summary>
        /// <returns>The same definition and rules, with <see cref="Position" /> increased by one.</returns>
        public Entry Next() {
            return this with { Position = Position + 1 };
        }

        public Hit ToHit() {
            return new Hit(Index, Definition);
        }
    }

    private readonly record struct Hit(int Index, Definition Definition);
}
