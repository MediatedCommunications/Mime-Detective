using MimeDetective.Storage;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace MimeDetective.Definitions {
    public static partial class Default
    {

        public static partial class FileTypes {
            public static partial class Email {

                public static ImmutableArray<Definition> All() {
                    return new List<Definition>() {
                        EML(),
                        PST(),
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> EML() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"eml"}.ToImmutableArray(),
                                MimeType = "message/rfc822",
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "46 72 6F 6D") 
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> PST() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"pst"}.ToImmutableArray(),
                                MimeType = ApplicationOctetStream,
                            },
                            Signature = new Segment[] {
                                    PrefixSegment.Create(0, "21 42 44 4E")
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }
            }
        }
    }
}
