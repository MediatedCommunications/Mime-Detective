using FileDetection.Storage;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace FileDetection.Data {

    public static partial class Micro {

        public static partial class FileTypes {

            public static partial class Xml {

                public static ImmutableArray<Definition> All() {
                    return new List<Definition>() {
                    XML_NoBom(),
                    XML_Ucs2Be(),
                    XML_Ucs2Le(),
                    XML_Utf8Bom(),
                }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> XML_NoBom() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"xml"}.ToImmutableArray(),
                                MimeType = "application/xml",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0x3C, 0x3F, 0x78, 0x6D, 0x6C, 0x20
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> XML_Utf8Bom() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"xml"}.ToImmutableArray(),
                                MimeType = "application/xml",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0x0EF, 0xBB, 0xBF, 0x3C, 0x3F, 0x78, 0x6D, 0x6C, 0x20
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> XML_Ucs2Be() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"xml"}.ToImmutableArray(),
                                MimeType = "application/xml",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0x0FF, 0xFE, 0x3C, 0x00, 0x3F, 0x00, 0x78, 0x00, 0x6D, 0x00, 0x6C, 0x00, 0x20, 0x00
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> XML_Ucs2Le() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"xml"}.ToImmutableArray(),
                                MimeType = "application/xml",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0x0FE, 0xFF, 0x00, 0x3C, 0x00, 0x3F, 0x00, 0x78, 0x00, 0x6D, 0x00, 0x6C, 0x00, 0x20
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