using MimeDetective.Storage;
using System.Collections.Generic;

namespace MimeDetective.Tests;

public class SampleCode {

    public static class CustomContentInspector {

        public static IContentInspector Instance { get; }

        static CustomContentInspector() {

            var MyDefinitions = new List<Definition>();

            //Add a predefined definition
            MyDefinitions.AddRange(MimeDetective.Definitions.Default.FileTypes.Audio.MP3());

            //Add a custom definition
            MyDefinitions.Add(new() {
                File = new() {
                    Categories = [Category.Other],
                    Description = "Magic File Type",
                    Extensions = ["magic"],
                    MimeType = "application/octet-stream",
                },
                //All of these rules must match
                Signature = SegmentExtensions.ToSignature<Segment>([
                    StringSegment.Create("MAGIC"), //anywhere in the file, expect "MAGIC" (exact case)
                    PrefixSegment.Create(100, "4d 41 47 49 43") //At offset 100 in the file, expect the bytes "MAGIC".
                ]),
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