using FileDetection.Storage;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace FileDetection.Data {
    public static partial class Micro {
        public static partial class FileTypes {

            public static partial class Text {

                public static ImmutableArray<Definition> All() {
                    return new List<Definition>() {
                        TXT_Utf16_Be(),
                        TXT_Utf16_Le(),
                        TXT_Utf32_Be(),
                        TXT_Utf32_Le(),
                        TXT_Utf8(),
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> TXT_Utf8() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"txt"}.ToImmutableArray(),
                                MimeType = "text/plain",
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "EF BB BF")
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> TXT_Utf16_Be() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"txt"}.ToImmutableArray(),
                                MimeType = "text/plain",
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "FE FF")
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> TXT_Utf16_Le() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"txt"}.ToImmutableArray(),
                                MimeType = "text/plain",
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "FF FE")
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> TXT_Utf32_Be() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"txt"}.ToImmutableArray(),
                                MimeType = "text/plain",
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "00 00 FE FF")
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> TXT_Utf32_Le() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"txt"}.ToImmutableArray(),
                                MimeType = "text/plain",
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "FF FE 00 00")
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

            }
        }
    }
}
