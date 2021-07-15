using FileDetection.Storage;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace FileDetection.Data {
    public static partial class Micro {
        public static partial class FileTypes {

            public static partial class Text {

                public static ImmutableArray<Definition> All() {
                    return new List<Definition>() {
                        TXT_NoBom(),
                        TXT_Utf16_Be(),
                        TXT_Utf16_Le(),
                        TXT_Utf32_Be(),
                        TXT_Utf32_Le(),
                        TXT_Utf8(),
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> TXT_NoBom() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"txt"}.ToImmutableArray(),
                                MimeType = "text/plain",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            //No Pattern
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> TXT_Utf8() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"txt"}.ToImmutableArray(),
                                MimeType = "text/plain",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0xEF, 0xBB, 0xBF
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> TXT_Utf16_Be() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"txt"}.ToImmutableArray(),
                                MimeType = "text/plain",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0xFE, 0xFF
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> TXT_Utf16_Le() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"txt"}.ToImmutableArray(),
                                MimeType = "text/plain",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0xFF, 0xFE
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> TXT_Utf32_Be() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"txt"}.ToImmutableArray(),
                                MimeType = "text/plain",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0x00, 0x00, 0xFE, 0xFF
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> TXT_Utf32_Le() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"txt"}.ToImmutableArray(),
                                MimeType = "text/plain",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0xFF, 0xFE, 0x00, 0x00
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
