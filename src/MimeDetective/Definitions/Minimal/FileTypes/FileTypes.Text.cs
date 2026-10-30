using MimeDetective.Storage;
using System.Collections.Immutable;

namespace MimeDetective;

public static partial class MinimalDefinitions {
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
                            Extensions = ["txt",],
                            MimeType = "text/plain",
                            Categories = [
                                Category.Document,
                            ],
                        },
                        Signature = new Rule[] {
                            OffsetRule.Create(0, [0xEF, 0xBB, 0xBF,]),
                        }.ToSignature(),
                    },
                ];
            }

            public static ImmutableArray<Definition> TXT_Utf16_Be() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["txt",],
                            MimeType = "text/plain",
                            Categories = [
                                Category.Document,
                                Category.BigEndian,
                            ],
                        },
                        Signature = new Rule[] {
                            OffsetRule.Create(0, [0xFE, 0xFF,]),
                        }.ToSignature(),
                    },
                ];
            }

            public static ImmutableArray<Definition> TXT_Utf16_Le() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["txt",],
                            MimeType = "text/plain",
                            Categories = [
                                Category.Document,
                                Category.LittleEndian,
                            ],
                        },
                        Signature = new Rule[] {
                            OffsetRule.Create(0, [0xFF, 0xFE,]),
                        }.ToSignature(),
                    },
                ];
            }

            public static ImmutableArray<Definition> TXT_Utf32_Be() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["txt",],
                            MimeType = "text/plain",
                            Categories = [
                                Category.Document,
                                Category.BigEndian,
                            ],
                        },
                        Signature = new Rule[] {
                            OffsetRule.Create(0, [0x00, 0x00, 0xFE, 0xFF,]),
                        }.ToSignature(),
                    },
                ];
            }

            public static ImmutableArray<Definition> TXT_Utf32_Le() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["txt",],
                            MimeType = "text/plain",
                            Categories = [
                                Category.Document,
                                Category.LittleEndian,
                            ],
                        },
                        Signature = new Rule[] {
                            OffsetRule.Create(0, [0xFF, 0xFE, 0x00, 0x00,]),
                        }.ToSignature(),
                    },
                ];
            }

        }
    }
}
