using FileDetection.Storage;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace FileDetection.Data {
    public static partial class Micro
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
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0x46, 0x72, 0x6F, 0x6D
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
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
                    Signature = new() {
                        Prefix = new[] {
                            new PrefixSegment() {
                                Pattern = new byte[] {
                                    0x21, 0x42, 0x44, 0x4E
                                }.ToImmutableArray()
                            }
                        }.ToImmutableArray()
                    },
                },
            }.ToImmutableArray();
                }
            }
        }
    }
}
