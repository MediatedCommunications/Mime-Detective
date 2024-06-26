﻿using MimeDetective.Storage;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace MimeDetective.Definitions {
    public static partial class Default
    {

        public static partial class FileTypes {
            public static partial class Documents {
                public static ImmutableArray<Definition> All() {
                    return new List<Definition>() {
                        DOC(),
                        DOCX(),
                        DWG(),
                        PDF(),
                        PPT(),
                        PPTX(),
                        RTF(),
                        ODT(),
                        ODP(),
                        ODS(),
                        XLS(),
                        XLSX(),
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> MicrosoftOffice() {
                    return new List<Definition>() {
                        DOC(),
                        DOCX(),
                        PPT(),
                        PPTX(),
                        XLS(),
                        XLSX(),
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> OpenOffice() {
                    return new List<Definition>() {
                        ODT(),
                        ODP(),
                        ODS(),
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> DOC() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"doc"}.ToImmutableArray(),
                                MimeType = "application/msword",
                                Categories = new[]{
                                    Category.Document,
                                }.ToImmutableHashSet(),
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "D0 CF 11 E0 A1 B1 1A E1"),
                                StringSegment.Create("W'O'R'D'D'O'C'U'M'E'N'T"),
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> DOCX() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"docx"}.ToImmutableArray(),
                                MimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                                Categories = new[]{
                                    Category.Compressed,
                                    Category.Document,
                                    Category.Archive,
                                }.ToImmutableHashSet(),
                            },
                            Signature = new Segment[] {                                
                                PrefixSegment.Create(0, "50 4B 03 04"),
                                
                                StringSegment.Create("_RELS"),
                                StringSegment.Create("CONTENT_TYPES"),
                                StringSegment.Create("XML.REL"),            
                                StringSegment.Create("WORD"),
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> DWG() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"dwg"}.ToImmutableArray(),
                                MimeType = "application/acad",
                                Categories = new[]{
                                    Category.Document,
                                    Category.Vector,
                                }.ToImmutableHashSet(),
                            },
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, "41 43 31 30")
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> PDF() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"pdf"}.ToImmutableArray(),
                                MimeType = "application/pdf",
                                Categories = new[]{
                                    Category.Bitmap,
                                    Category.Document,
                                    Category.Image,
                                }.ToImmutableHashSet(),
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "25 50 44 46")
                            }.ToSignature(),
                        }
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> ODT() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"odt"}.ToImmutableArray(),
                                MimeType = "application/vnd.oasis.opendocument.text",
                                Categories = new[]{
                                    Category.Compressed,
                                    Category.Document,
                                }.ToImmutableHashSet(),
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "50 4B 03 04"),
                                StringSegment.Create("VND.OASIS.OPENDOCUMENT.TEXTPK"),
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> ODP() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"odp"}.ToImmutableArray(),
                                MimeType = "application/vnd.oasis.opendocument.presentation",
                                Categories = new[]{
                                    Category.Presentation,
                                    Category.Compressed,
                                }.ToImmutableHashSet(),
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "50 4B 03 04"),
                                StringSegment.Create("VND.OASIS.OPENDOCUMENT.PRESENTATIONPK"),
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> ODS() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"ods"}.ToImmutableArray(),
                                MimeType = "application/vnd.oasis.opendocument.spreadsheet",
                                Categories = new[]{
                                    Category.Spreadsheet,
                                    Category.Compressed,
                                }.ToImmutableHashSet(),
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "50 4B 03 04"),
                                StringSegment.Create("VND.OASIS.OPENDOCUMENT.SPREADSHEETPK"),
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> PPT() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"ppt"}.ToImmutableArray(),
                                MimeType = "application/mspowerpoint",
                                Categories = new[]{
                                    Category.Presentation,
                                }.ToImmutableHashSet(),
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "D0 CF 11 E0 A1 B1 1A E1"),
                                StringSegment.Create("POWERPOINT"),
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> PPTX() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"pptx"}.ToImmutableArray(),
                                MimeType = "application/vnd.openxmlformats-officedocument.presentationml.presentation",
                                Categories = new[]{
                                    Category.Compressed,
                                    Category.Archive,
                                    Category.Presentation,
                                }.ToImmutableHashSet(),
                            },
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, "50 4B 03 04"),

                                StringSegment.Create("_RELS"),
                                StringSegment.Create("CONTENT_TYPES"),
                                StringSegment.Create("XML.RELS"),

                                StringSegment.Create("SLIDES"),
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> RTF() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"rtf"}.ToImmutableArray(),
                                MimeType = "application/rtf",
                                Categories = new[]{
                                    Category.Document,
                                }.ToImmutableHashSet(),
                            },
                            Signature = new Segment[]{ 
                                PrefixSegment.Create(0, "7B 5C 72 74 66 31")
                            }.ToSignature()
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> XLS() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"xls"}.ToImmutableArray(),
                                MimeType = "application/excel",
                                Categories = new[]{
                                    Category.Spreadsheet,
                                }.ToImmutableHashSet(),
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "D0 CF 11 E0 A1 B1 1A E1"),
                                StringSegment.Create("EXCEL"),
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> XLSX() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"xlsx"}.ToImmutableArray(),
                                MimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                Categories = new[]{
                                    Category.Compressed,
                                    Category.Archive,
                                    Category.Spreadsheet,
                                }.ToImmutableHashSet(),
                            },
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, "50 4B 03 04"),
                    
                                StringSegment.Create("_RELS"),
                                StringSegment.Create("CONTENT_TYPES"),
                                StringSegment.Create("XML.REL"),

                                StringSegment.Create("WORKBOOK"),
                            }.ToSignature(),
                        }
                    }.ToImmutableArray();
                }

            }
        }
    }
}
