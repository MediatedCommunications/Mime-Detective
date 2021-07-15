using FileDetection.Storage;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Security.Cryptography.X509Certificates;

namespace FileDetection.Data {
    public static partial class Micro
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
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0x41, 0x45, 0x53
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> PKR() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"pkr"}.ToImmutableArray(),
                                MimeType = ApplicationOctetStream,
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0x99, 0x01
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> SKR_V1() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"skr"}.ToImmutableArray(),
                                MimeType = ApplicationOctetStream,
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0x95, 0x00
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> SKR_V2() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"skr"}.ToImmutableArray(),
                                MimeType = ApplicationOctetStream,
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0x95, 0x01
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
