using MimeDetective.Storage;
using System.Collections.Immutable;

namespace MimeDetective.Definitions;

public static partial class Default {
    public static partial class FileTypes {
        public static class Xml {
            public static ImmutableArray<Definition> All() {
                return [
                    .. XML_NoBom(),
                    .. XML_Ucs2Be(),
                    .. XML_Ucs2Le(),
                    .. XML_Utf8()
                ];
            }

            public static ImmutableArray<Definition> XML_NoBom() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["xml"],
                            MimeType = "application/xml",
                            Categories = [
                                Category.Document,
                                Category.Xml
                            ]
                        },
                        Signature = SegmentExtensions.ToSignature([
                            PrefixSegment.Create(0, "3C 3F 78 6D 6C 20")
                        ])
                    }
                ];
            }

            public static ImmutableArray<Definition> XML_Utf8() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["xml"],
                            MimeType = "application/xml",
                            Categories = [
                                Category.Document,
                                Category.Utf8,
                                Category.Xml
                            ]
                        },
                        Signature = SegmentExtensions.ToSignature([
                            PrefixSegment.Create(0, "EF BB BF 3C 3F 78 6D 6C 20")
                        ])
                    }
                ];
            }

            public static ImmutableArray<Definition> XML_Ucs2Be() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["xml"],
                            MimeType = "application/xml",
                            Categories = [
                                Category.Document,
                                Category.BigEndian,
                                Category.Xml
                            ]
                        },
                        Signature = SegmentExtensions.ToSignature([
                            PrefixSegment.Create(0, "FF FE 3C 00 3F 00 78 00 6D 00 6C 00 20 00")
                        ])
                    }
                ];
            }

            public static ImmutableArray<Definition> XML_Ucs2Le() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["xml"],
                            MimeType = "application/xml",
                            Categories = [
                                Category.Document,
                                Category.LittleEndian,
                                Category.Xml
                            ]
                        },
                        Signature = SegmentExtensions.ToSignature([
                            PrefixSegment.Create(0, "FE FF 00 3C 00 3F 00 78 00 6D 00 6C 00 20")
                        ])
                    }
                ];
            }
        }
    }
}
