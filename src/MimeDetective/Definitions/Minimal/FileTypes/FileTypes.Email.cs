using MimeDetective.Storage;
using System.Collections.Immutable;

namespace MimeDetective;

public static partial class MinimalDefinitions
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
                            Extensions = ["eml",],
                            MimeType = "message/rfc822",
                            Categories = [
                                Category.Email,
                                Category.Document,
                            ],
                        },
                        Signature = new Rule[] {
                            WildCardRule.Create("FROM"),
                        }.ToSignature(),
                    },
                ];
            }

            public static ImmutableArray<Definition> PST() {
                return [
                    new() {
                        File = new() {
                            Extensions = ["pst",],
                            MimeType = ApplicationOctetStream,
                            Categories = [
                                Category.Email,
                                Category.Archive,
                            ],
                        },
                        Signature = new Rule[] {
                                OffsetRule.Create(0, [0x21, 0x42, 0x44, 0x4E,]),
                        }.ToSignature(),
                    },
                ];
            }
        }
    }
}
