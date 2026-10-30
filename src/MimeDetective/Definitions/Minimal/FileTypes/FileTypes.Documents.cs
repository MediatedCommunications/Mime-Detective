using MimeDetective.Storage;
using System.Collections.Immutable;

namespace MimeDetective.Definitions {
    public static partial class MinimalDefinitions
    {

        public static partial class FileTypes {
            public static partial class Documents {
                public static ImmutableArray<Definition> All() {
                    return [
                        .. DOC(),
                        .. DOCX(),
                        .. DWG(),
                        .. PDF(),
                        .. PPT(),
                        .. PPTX(),
                        .. RTF(),
                        .. ODT(),
                        .. ODP(),
                        .. ODS(),
                        .. XLS(),
                        .. XLSX(),
                    ];
                }

                public static ImmutableArray<Definition> MicrosoftOffice() {
                    return [
                        .. DOC(),
                        .. DOCX(),
                        .. PPT(),
                        .. PPTX(),
                        .. XLS(),
                        .. XLSX(),
                    ];
                }

                public static ImmutableArray<Definition> OpenOffice() {
                    return [
                        .. ODT(),
                        .. ODP(),
                        .. ODS(),
                    ];
                }

                public static ImmutableArray<Definition> DOC() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["doc",],
                                MimeType = "application/msword",
                                Categories = [
                                    Category.Document,
                                ],
                            },
                            Signature = new Rule[] {
                                OffsetRule.Create(0, [0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1,]),
                                WildCardRule.Create("W'O'R'D'D'O'C'U'M'E'N'T"),
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> DOCX() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["docx",],
                                MimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                                Categories = [
                                    Category.Compressed,
                                    Category.Document,
                                    Category.Archive,
                                ],
                            },
                            Signature = new Rule[] {                                
                                OffsetRule.Create(0, [0x50, 0x4B, 0x03, 0x04,]),
                                
                                WildCardRule.Create("_RELS"),
                                WildCardRule.Create("CONTENT_TYPES"),
                                WildCardRule.Create("XML.REL"),            
                                WildCardRule.Create("WORD"),
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> DWG() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["dwg",],
                                MimeType = "application/acad",
                                Categories = [
                                    Category.Document,
                                    Category.Vector,
                                ],
                            },
                            Signature = new Rule[]{
                                OffsetRule.Create(0, [0x41, 0x43, 0x31, 0x30,]),
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> PDF() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["pdf",],
                                MimeType = "application/pdf",
                                Categories = [
                                    Category.Bitmap,
                                    Category.Document,
                                    Category.Image,
                                ],
                            },
                            Signature = new Rule[] {
                                OffsetRule.Create(0, [0x25, 0x50, 0x44, 0x46,]),
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> ODT() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["odt",],
                                MimeType = "application/vnd.oasis.opendocument.text",
                                Categories = [
                                    Category.Compressed,
                                    Category.Document,
                                ],
                            },
                            Signature = new Rule[] {
                                OffsetRule.Create(0, [0x50, 0x4B, 0x03, 0x04,]),
                                WildCardRule.Create("VND.OASIS.OPENDOCUMENT.TEXTPK"),
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> ODP() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["odp",],
                                MimeType = "application/vnd.oasis.opendocument.presentation",
                                Categories = [
                                    Category.Presentation,
                                    Category.Compressed,
                                ],
                            },
                            Signature = new Rule[] {
                                OffsetRule.Create(0, [0x50, 0x4B, 0x03, 0x04,]),
                                WildCardRule.Create("VND.OASIS.OPENDOCUMENT.PRESENTATIONPK"),
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> ODS() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["ods",],
                                MimeType = "application/vnd.oasis.opendocument.spreadsheet",
                                Categories = [
                                    Category.Spreadsheet,
                                    Category.Compressed,
                                ],
                            },
                            Signature = new Rule[] {
                                OffsetRule.Create(0, [0x50, 0x4B, 0x03, 0x04,]),
                                WildCardRule.Create("VND.OASIS.OPENDOCUMENT.SPREADSHEETPK"),
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> PPT() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["ppt",],
                                MimeType = "application/mspowerpoint",
                                Categories = [
                                    Category.Presentation,
                                ],
                            },
                            Signature = new Rule[] {
                                OffsetRule.Create(0, [0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1,]),
                                WildCardRule.Create("POWERPOINT"),
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> PPTX() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["pptx",],
                                MimeType = "application/vnd.openxmlformats-officedocument.presentationml.presentation",
                                Categories = [
                                    Category.Compressed,
                                    Category.Archive,
                                    Category.Presentation,
                                ],
                            },
                            Signature = new Rule[]{
                                OffsetRule.Create(0, [0x50, 0x4B, 0x03, 0x04,]),

                                WildCardRule.Create("_RELS"),
                                WildCardRule.Create("CONTENT_TYPES"),
                                WildCardRule.Create("XML.RELS"),

                                WildCardRule.Create("SLIDES"),
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> RTF() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["rtf",],
                                MimeType = "application/rtf",
                                Categories = [
                                    Category.Document,
                                ],
                            },
                            Signature = new Rule[]{ 
                                OffsetRule.Create(0, [0x7B, 0x5C, 0x72, 0x74, 0x66, 0x31,]),
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> XLS() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["xls",],
                                MimeType = "application/excel",
                                Categories = [
                                    Category.Spreadsheet,
                                ],
                            },
                            Signature = new Rule[] {
                                OffsetRule.Create(0, [0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1,]),
                                WildCardRule.Create("EXCEL"),
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> XLSX() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["xlsx",],
                                MimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                Categories = [
                                    Category.Compressed,
                                    Category.Archive,
                                    Category.Spreadsheet,
                                ],
                            },
                            Signature = new Rule[]{
                                OffsetRule.Create(0, [0x50, 0x4B, 0x03, 0x04,]),
                    
                                WildCardRule.Create("_RELS"),
                                WildCardRule.Create("CONTENT_TYPES"),
                                WildCardRule.Create("XML.REL"),

                                WildCardRule.Create("WORKBOOK"),
                            }.ToSignature(),
                        },
                    ];
                }

            }
        }
    }
}
