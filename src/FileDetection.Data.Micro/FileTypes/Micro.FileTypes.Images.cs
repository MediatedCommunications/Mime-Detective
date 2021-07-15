using FileDetection.Storage;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace FileDetection.Data {
    public static partial class Micro
    {
        public static partial class FileTypes {

            public static partial class Images {

                public static ImmutableArray<Definition> All() {
                    return new List<Definition>() {
                        BMP(),
                        GIF(),
                        ICO(),
                        JPEG(),
                        PNG(),
                        PSD(),

                        TIFF_Be(),
                        TIFF_Big(),
                        TIFF_Le(),
                        TIFF_NoBom(),

                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> TIFF() {
                    return new List<Definition>() {
                        TIFF_Be(),
                        TIFF_Big(),
                        TIFF_Le(),
                        TIFF_NoBom(),
                    }.ToImmutableArray();
                }



                public static ImmutableArray<Definition> BMP() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"bmp"}.ToImmutableArray(),
                                MimeType = "image/bmp",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0x42, 0x4D
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> GIF() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"gif"}.ToImmutableArray(),
                                MimeType = "image/gif",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0x47, 0x49, 0x46, 0x38,
                                        }.ToImmutableArray()
                                    },
                                    new PrefixSegment() {
                                        Start = 5,
                                        Pattern = new byte[] {
                                            0x61,
                                        }.ToImmutableArray(),
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> ICO() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"ico"}.ToImmutableArray(),
                                MimeType = "image/x-icon",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0x00, 0x00, 0x01, 0x00
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> JPEG() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"jpg", "jpeg"}.ToImmutableArray(),
                                MimeType = "image/jpeg",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0xFF, 0xD8, 0xFF
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> PNG() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"png"}.ToImmutableArray(),
                                MimeType = "image/png",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> PSD() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"psd"}.ToImmutableArray(),
                                MimeType = ApplicationOctetStream,
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0x38, 0x42, 0x50, 0x53
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> TIFF_Be() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"tif", "tiff"}.ToImmutableArray(),
                                MimeType = "image/tiff",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0x4D, 0x4D, 0, 0x2A
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> TIFF_Big() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"tif", "tiff"}.ToImmutableArray(),
                                MimeType = "image/tiff",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0x4D, 0x4D, 0, 0x2B
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();
                }
            
                public static ImmutableArray<Definition> TIFF_Le() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"tif", "tiff"}.ToImmutableArray(),
                                MimeType = "image/tiff",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0x49, 0x49, 0x2A, 0
                                        }.ToImmutableArray()
                                    }
                                }.ToImmutableArray()
                            },
                        },
                    }.ToImmutableArray();
                }

                public static ImmutableArray<Definition> TIFF_NoBom() {
                    return new List<Definition>() {
                        new() {
                            File = new() {
                                Extensions = new[]{"tif", "tiff"}.ToImmutableArray(),
                                MimeType = "image/tiff",
                            },
                            Signature = new() {
                                Prefix = new[] {
                                    new PrefixSegment() {
                                        Pattern = new byte[] {
                                            0x49, 0x20, 0x49
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
