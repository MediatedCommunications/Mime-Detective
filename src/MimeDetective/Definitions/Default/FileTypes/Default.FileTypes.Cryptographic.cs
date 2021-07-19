using MimeDetective.Storage;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Security.Cryptography.X509Certificates;

namespace MimeDetective.Definitions {
    public static partial class Default
    {

        public static partial class FileTypes {
            public static partial class Cryptographic {

                public static ImmutableArray<Definition> All() {
                    return new List<Definition>() {
                        AES(),
                        PKR(),
                        SKR_V1(),
                        SKR_V2(),
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> SKR() {
                    return new List<Definition>() {
                        SKR_V1(),
                        SKR_V2(),
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> AES() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"aes"}.ToImmutableArray(),
                                MimeType = ApplicationOctetStream,
                                Categories = new[]{
                                    Category.Encrypted,
                                }.ToImmutableHashSet(),
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "41 45 53")
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> PKR() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"pkr"}.ToImmutableArray(),
                                MimeType = ApplicationOctetStream,
                                Categories = new[]{
                                    Category.Encrypted,
                                }.ToImmutableHashSet(),
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "99 01")
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> SKR_V1() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"skr"}.ToImmutableArray(),
                                MimeType = ApplicationOctetStream,
                                Categories = new[]{
                                    Category.Encrypted,
                                }.ToImmutableHashSet(),
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "95 00") 
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> SKR_V2() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"skr"}.ToImmutableArray(),
                                MimeType = ApplicationOctetStream,
                                Categories = new[]{
                                    Category.Encrypted,
                                }.ToImmutableHashSet(),
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "95 01") 
                            }.ToSignature(),
                        },
                    }.ToImmutableArray();
                }

            }
        }
    }
}
