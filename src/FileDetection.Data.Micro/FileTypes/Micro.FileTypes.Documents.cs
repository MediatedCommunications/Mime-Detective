using FileDetection.Storage;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace FileDetection.Data {
    public static partial class Micro
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


                public static ImmutableArray<Definition> DOC() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"doc"}.ToImmutableArray(),
                                MimeType = "application/msword",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Start = 512,
                                        Pattern = new byte[] {
                                            0xEC, 0xA5, 0xC1, 0x00
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> DOCX() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"docx"}.ToImmutableArray(),
                                MimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0x50, 0x4B, 0x03, 0x04,
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray(),
                                Strings = new[] {
                                    StringSegment.Create("_RELS"),
                                    StringSegment.Create("CONTENT_TYPES"),
                                    StringSegment.Create("XML.REL"),
                                    
                                    StringSegment.Create("WORD"),
                                }.ToImmutableArray(),
                            },
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> DWG() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"dwg"}.ToImmutableArray(),
                                MimeType = "application/acad",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0x41, 0x43, 0x31, 0x30
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> PDF() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"pdf"}.ToImmutableArray(),
                                MimeType = "application/pdf",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0x25, 0x50, 0x44, 0x46
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        }
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> PPT() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"ppt"}.ToImmutableArray(),
                                MimeType = "application/mspowerpoint",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Start = 512,
                                        Pattern = new byte[] {
                                            0xA0, 0x46, 0x1D, 0xF0
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> PPTX() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"pptx"}.ToImmutableArray(),
                                MimeType = "application/vnd.openxmlformats-officedocument.presentationml.presentation",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0x50, 0x4B, 0x03, 0x04,
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray(),
                                Strings = new[] {
                                    StringSegment.Create("_RELS"),
                                    StringSegment.Create("CONTENT_TYPES"),
                                    StringSegment.Create("XML.RELS"),
                                    
                                    StringSegment.Create("SLIDES"),
                                }.ToImmutableArray(),
                            },
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> RTF() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"rtf"}.ToImmutableArray(),
                                MimeType = "application/rtf",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0x7B, 0x5C, 0x72, 0x74, 0x66, 0x31
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> XLS() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"xls"}.ToImmutableArray(),
                                MimeType = "application/excel",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Start = 512,
                                        Pattern = new byte[] {
                                            0x09, 0x08, 0x10, 0x00, 0x00, 0x06, 0x05, 0x00
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> XLSX() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"xlsx"}.ToImmutableArray(),
                                MimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0x50, 0x4B, 0x03, 0x04,
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray(),
                                Strings = new[] {
                                    StringSegment.Create("_RELS"),
                                    StringSegment.Create("CONTENT_TYPES"),
                                    StringSegment.Create("XML.REL"),

                                    StringSegment.Create("WORKBOOK"),
                                }.ToImmutableArray(),
                            },
                        },
                    }.ToImmutableArray();
                }

            }
        }
    }
}
