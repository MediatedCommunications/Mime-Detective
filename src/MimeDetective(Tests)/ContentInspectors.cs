using MimeDetective.Definitions;
using MimeDetective.Definitions.Licensing;
using MimeDetective.Storage;
using System.Collections.Immutable;

namespace MimeDetective.Tests;

public static class ContentInspectors {
    public static class Exhaustive {
        public static IContentInspector ContentInspector { get; }
        public static ImmutableArray<Definition> Definitions { get; }
        public static MimeTypeToFileExtensionLookup MimeTypeToFileExtensionLookup { get; }
        public static FileExtensionToMimeTypeLookup FileExtensionToMimeTypeLookup { get; }

        static Exhaustive() {
            Definitions = new ExhaustiveBuilder { UsageType = UsageType.CommercialPaid }.Build();

            ContentInspector = new ContentInspectorBuilder { Definitions = Definitions }.Build();

            MimeTypeToFileExtensionLookup = new MimeTypeToFileExtensionLookupBuilder { Definitions = Definitions }.Build();

            FileExtensionToMimeTypeLookup = new FileExtensionToMimeTypeLookupBuilder { Definitions = Definitions }.Build();
        }
    }
}
