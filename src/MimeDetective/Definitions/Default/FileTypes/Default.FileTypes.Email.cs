using MimeDetective.Storage;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace MimeDetective.Definitions {
    public static partial class DefaultDefinitions
    {

        public static partial class FileTypes {
            public static partial class Email {

                public static ImmutableArray<Definition> All() {
                    return [
                        .. EML(),
                        .. PST(),
                    ];
                }

                public static ImmutableArray<Definition> EML() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["eml"],
                                MimeType = "message/rfc822",
                                Categories = [
                                    Category.Email,
                                    Category.Document,
                                ],
                            },
                            Signature = new Segment[] {
                                StringSegment.Create("FROM"),
                            }.ToSignature(),
                        },
                    ];
                }

                public static ImmutableArray<Definition> PST() {
                    return [
                        new() {
                            File = new() {
                                Extensions = ["pst"],
                                MimeType = ApplicationOctetStream,
                                Categories = [
                                    Category.Email,
                                    Category.Archive,
                                ],
                            },
                            Signature = new Segment[] {
                                    PrefixSegment.Create(0, "21 42 44 4E")
                            }.ToSignature(),
                        },
                    ];
                }
            }
        }
    }
}
