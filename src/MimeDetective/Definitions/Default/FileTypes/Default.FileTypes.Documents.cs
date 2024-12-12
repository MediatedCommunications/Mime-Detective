using MimeDetective.Storage;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace MimeDetective.Definitions {
    public static partial class DefaultDefinitions
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
                                Extensions = ["doc"],
                                MimeType = "application/msword",
                                Categories = [
                                    Category.Document,
                                ],
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "D0 CF 11 E0 A1 B1 1A E1"),
                                StringSegment.Create("W'O'R'D'D'O'C'U'M'E'N'T"),
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> DOCX() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["docx"],
                                MimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                                Categories = [
                                    Category.Compressed,
                                    Category.Document,
                                    Category.Archive,
                                ],
                            },
                            Signature = new Segment[] {                                
                                PrefixSegment.Create(0, "50 4B 03 04"),
                                
                                StringSegment.Create("_RELS"),
                                StringSegment.Create("CONTENT_TYPES"),
                                StringSegment.Create("XML.REL"),            
                                StringSegment.Create("WORD"),
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> DWG() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["dwg"],
                                MimeType = "application/acad",
                                Categories = [
                                    Category.Document,
                                    Category.Vector,
                                ],
                            },
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, "41 43 31 30")
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> PDF() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["pdf"],
                                MimeType = "application/pdf",
                                Categories = [
                                    Category.Bitmap,
                                    Category.Document,
                                    Category.Image,
                                ],
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "25 50 44 46")
                            }.ToSignature(),
                        }
                    ];
                }

                public static ImmutableArray<Definition> ODT() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["odt"],
                                MimeType = "application/vnd.oasis.opendocument.text",
                                Categories = [
                                    Category.Compressed,
                                    Category.Document,
                                ],
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "50 4B 03 04"),
                                StringSegment.Create("VND.OASIS.OPENDOCUMENT.TEXTPK"),
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> ODP() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["odp"],
                                MimeType = "application/vnd.oasis.opendocument.presentation",
                                Categories = [
                                    Category.Presentation,
                                    Category.Compressed,
                                ],
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "50 4B 03 04"),
                                StringSegment.Create("VND.OASIS.OPENDOCUMENT.PRESENTATIONPK"),
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> ODS() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["ods"],
                                MimeType = "application/vnd.oasis.opendocument.spreadsheet",
                                Categories = [
                                    Category.Spreadsheet,
                                    Category.Compressed,
                                ],
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "50 4B 03 04"),
                                StringSegment.Create("VND.OASIS.OPENDOCUMENT.SPREADSHEETPK"),
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> PPT() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["ppt"],
                                MimeType = "application/mspowerpoint",
                                Categories = [
                                    Category.Presentation,
                                ],
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "D0 CF 11 E0 A1 B1 1A E1"),
                                StringSegment.Create("POWERPOINT"),
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> PPTX() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["pptx"],
                                MimeType = "application/vnd.openxmlformats-officedocument.presentationml.presentation",
                                Categories = [
                                    Category.Compressed,
                                    Category.Archive,
                                    Category.Presentation,
                                ],
                            },
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, "50 4B 03 04"),

                                StringSegment.Create("_RELS"),
                                StringSegment.Create("CONTENT_TYPES"),
                                StringSegment.Create("XML.RELS"),

                                StringSegment.Create("SLIDES"),
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> RTF() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["rtf"],
                                MimeType = "application/rtf",
                                Categories = [
                                    Category.Document,
                                ],
                            },
                            Signature = new Segment[]{ 
                                PrefixSegment.Create(0, "7B 5C 72 74 66 31")
                            }.ToSignature()
                        },
                    ];
                }

                public static ImmutableArray<Definition> XLS() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["xls"],
                                MimeType = "application/excel",
                                Categories = [
                                    Category.Spreadsheet,
                                ],
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "D0 CF 11 E0 A1 B1 1A E1"),
                                StringSegment.Create("EXCEL"),
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> XLSX() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["xlsx"],
                                MimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                Categories = [
                                    Category.Compressed,
                                    Category.Archive,
                                    Category.Spreadsheet,
                                ],
                            },
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, "50 4B 03 04"),
                    
                                StringSegment.Create("_RELS"),
                                StringSegment.Create("CONTENT_TYPES"),
                                StringSegment.Create("XML.REL"),

                                StringSegment.Create("WORKBOOK"),
                            }.ToSignature(),
                        }
                    ];
                }

            }
        }
    }
}
