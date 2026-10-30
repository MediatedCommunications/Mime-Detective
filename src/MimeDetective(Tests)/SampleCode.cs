using MimeDetective.Storage;
using System.Collections.Generic;

namespace MimeDetective.Tests {

    public class SampleCode {

        public static class CustomContentInspector {

            public static IContentInspector Instance { get; }

            static CustomContentInspector() {

                var MyDefinitions = new List<Definition>();
                
                //Add a predefined definition
                MyDefinitions.AddRange(MimeDetective.Definitions.MinimalDefinitions.FileTypes.Audio.MP3());

                //Add a custom definition
                MyDefinitions.Add(new() {
                    File = new() {
                        Categories = [Category.Other,],
                        Description = "Magic File Type",
                        Extensions = ["magic",],
                        MimeType = "application/octet-stream",
                    },
                    //All of these rules must match
                    Signature = new Segment[] {
                        StringSegment.Create("MAGIC"), //anywhere in the file, expect "MAGIC" (exact case)
                        PrefixSegment.Create(100, [0x4D, 0x41, 0x47, 0x49, 0x43,]), //At offset 100 in the file, expect the bytes "MAGIC".
                    }.ToSignature(),
                });

                Instance = new ContentInspectorBuilder() {
                    Definitions = MyDefinitions,
                    StringSegmentOptions = new() {
                        OptimizeFor = Engine.StringSegmentResourceOptimization.HighSpeed,
                    },
                }.Build();
            }

        }

    }

}
