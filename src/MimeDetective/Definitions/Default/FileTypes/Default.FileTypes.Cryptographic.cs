using MimeDetective.Storage;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace MimeDetective.Definitions {
    public static partial class DefaultDefinitions
    {

        public static partial class FileTypes {
            public static partial class Cryptographic {

                public static ImmutableArray<Definition> All() {
                    return [
                        .. AES(),
                        .. PKR(),
                        .. SKR_V1(),
                        .. SKR_V2(),
                    ];
                }

                public static ImmutableArray<Definition> SKR() {
                    return [
                        .. SKR_V1(),
                        .. SKR_V2(),
                    ];
                }

                public static ImmutableArray<Definition> AES() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["aes"],
                                MimeType = ApplicationOctetStream,
                                Categories = [
                                    Category.Encrypted,
                                ],
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "41 45 53")
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> PKR() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["pkr"],
                                MimeType = ApplicationOctetStream,
                                Categories = [
                                    Category.Encrypted,
                                ],
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "99 01")
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> SKR_V1() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["skr"],
                                MimeType = ApplicationOctetStream,
                                Categories = [
                                    Category.Encrypted,
                                ],
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "95 00") 
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> SKR_V2() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["skr"],
                                MimeType = ApplicationOctetStream,
                                Categories = [
                                    Category.Encrypted,
                                ],
                            },
                            Signature = new Segment[] {
                                PrefixSegment.Create(0, "95 01") 
                            }.ToSignature(),
                        },
                    ];
                }

            }
        }
    }
}
