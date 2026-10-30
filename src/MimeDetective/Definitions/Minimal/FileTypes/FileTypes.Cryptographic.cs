using MimeDetective.Storage;
using System.Collections.Immutable;

namespace MimeDetective;

public static partial class MinimalDefinitions
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
                            Extensions = ["aes",],
                            MimeType = ApplicationOctetStream,
                            Categories = [
                                Category.Encrypted,
                            ],
                        },
                        Signature = new Rule[] {
                            OffsetRule.Create(0, [0x41, 0x45, 0x53,]),
                        }.ToSignature(),
                    },
                ];
            }

            public static ImmutableArray<Definition> PKR() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["pkr",],
                            MimeType = ApplicationOctetStream,
                            Categories = [
                                Category.Encrypted,
                            ],
                        },
                        Signature = new Rule[] {
                            OffsetRule.Create(0, [0x99, 0x01,]),
                        }.ToSignature(),
                    },
                ];
            }

            public static ImmutableArray<Definition> SKR_V1() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["skr",],
                            MimeType = ApplicationOctetStream,
                            Categories = [
                                Category.Encrypted,
                            ],
                        },
                        Signature = new Rule[] {
                            OffsetRule.Create(0, [0x95, 0x00,]), 
                        }.ToSignature(),
                    },
                ];
            }

            public static ImmutableArray<Definition> SKR_V2() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["skr",],
                            MimeType = ApplicationOctetStream,
                            Categories = [
                                Category.Encrypted,
                            ],
                        },
                        Signature = new Rule[] {
                            OffsetRule.Create(0, [0x95, 0x01,]), 
                        }.ToSignature(),
                    },
                ];
            }

        }
    }
}
