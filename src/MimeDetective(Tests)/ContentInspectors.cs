using MimeDetective.Definitions;

using MimeDetective.Storage;
using System.Collections.Immutable;

namespace MimeDetective.Tests;

public static class ContentInspectors {
    public static class Exhaustive {
        public static ContentInspector ContentInspector { get; }
        public static ImmutableArray<Definition> Definitions { get; }

        static Exhaustive() {
            Definitions = MimeDetective.ExhaustiveDefinitions.Load(UsageType.CommercialPaid).ToImmutableArray();

            ContentInspector = ContentInspector.Create(Definitions);

        }
    }
}
