using MimeDetective.Storage;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace MimeDetective.Definitions {
    public static partial class DefaultDefinitions {
        public static partial class FileTypes {

            public static partial class Text {

                public static ImmutableArray<Definition> All() {
                    return [
                        .. TXT_Utf16_Be(),
                        .. TXT_Utf16_Le(),
                        .. TXT_Utf32_Be(),
                        .. TXT_Utf32_Le(),
                        .. TXT_Utf8(),
                    ];
                }

                public static ImmutableArray<Definition> TXT_Utf8() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["txt"],
                                MimeType = "text/plain",
                                Categories = [
                                    Category.Document,
                                ],
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "EF BB BF")
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> TXT_Utf16_Be() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["txt"],
                                MimeType = "text/plain",
                                Categories = [
                                    Category.Document,
                                    Category.BigEndian,
                                ],
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "FE FF")
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> TXT_Utf16_Le() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["txt"],
                                MimeType = "text/plain",
                                Categories = [
                                    Category.Document,
                                    Category.LittleEndian,
                                ],
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "FF FE")
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> TXT_Utf32_Be() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["txt"],
                                MimeType = "text/plain",
                                Categories = [
                                    Category.Document,
                                    Category.BigEndian,
                                ],
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "00 00 FE FF")
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> TXT_Utf32_Le() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["txt"],
                                MimeType = "text/plain",
                                Categories = [
                                    Category.Document,
                                    Category.LittleEndian,
                                ],
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "FF FE 00 00")
                            }.ToSignature(),
                        },
                    ];
                }

            }
        }
    }
}
