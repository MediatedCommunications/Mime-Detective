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
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, "3C 3F 78 6D 6C 20")
                            }.ToSignature(),
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
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, "EF BB BF 3C 3F 78 6D 6C 20"),
                            }.ToSignature(),
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
                            Signature = new Segment[]{
                                PrefixSegment.Create(0,"FF FE 3C 00 3F 00 78 00 6D 00 6C 00 20 00"),
                            }.ToSignature(),
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
                            Signature = new Segment[]{
                                PrefixSegment.Create(0, "FE FF 00 3C 00 3F 00 78 00 6D 00 6C 00 20")
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }
            }


        }
    }
}