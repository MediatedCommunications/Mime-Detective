using MimeDetective.Storage;
using System.Collections.Immutable;

namespace MimeDetective.Definitions
{

    public static partial class MinimalDefinitions {

        public static partial class FileTypes {

            public static partial class Xml {

                public static ImmutableArray<Definition> All() {
                    return [
                    .. XML_NoBom(),
                    .. XML_Ucs2Be(),
                    .. XML_Ucs2Le(),
                    .. XML_Utf8(),
                ];
                }

                public static ImmutableArray<Definition> XML_NoBom() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["xml",],
                                MimeType = "application/xml",
                                Categories = [
                                    Category.Document,
                                    Category.Xml,
                                ],
                            },
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, [0x3C, 0x3F, 0x78, 0x6D, 0x6C, 0x20,]),
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> XML_Utf8() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["xml",],
                                MimeType = "application/xml",
                                Categories = [
                                    Category.Document,
                                    Category.Utf8,
                                    Category.Xml,
                                ],
                            },
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, [0xEF, 0xBB, 0xBF, 0x3C, 0x3F, 0x78, 0x6D, 0x6C, 0x20,]),
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> XML_Ucs2Be() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["xml",],
                                MimeType = "application/xml",
                                Categories = [
                                    Category.Document,
                                    Category.BigEndian,
                                    Category.Xml,
                                ],
                            },
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, [0xFF, 0xFE, 0x3C, 0x00, 0x3F, 0x00, 0x78, 0x00, 0x6D, 0x00, 0x6C, 0x00, 0x20, 0x00,]),
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> XML_Ucs2Le() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["xml",],
                                MimeType = "application/xml",
                                Categories = [
                                    Category.Document,
                                    Category.LittleEndian,
                                    Category.Xml,
                                ],
                            },
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, [0xFE, 0xFF, 0x00, 0x3C, 0x00, 0x3F, 0x00, 0x78, 0x00, 0x6D, 0x00, 0x6C, 0x00, 0x20,]),
                            }.ToSignature(),
                        },
                    ];
                }
            }


        }
    }
}