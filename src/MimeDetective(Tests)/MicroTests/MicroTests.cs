using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MimeDetective.Tests {
    [TestClass]
    public abstract class MicroTests {
        private static ContentInspector? GetEngine_Result;
        protected static ContentInspector GetEngine() {

            var IsDebug = false;

            if (GetEngine_Result == default && IsDebug) {
                var inspector = new ContentInspectorBuilder() {
                    Definitions = {
                        Definitions.Default.FileTypes.Email.EML(),
                    },
                    Parallel = false,
                };
                GetEngine_Result = inspector.Build();
            }




            if (GetEngine_Result == default) {
                var Defintions = Definitions.Default.All();

                GetEngine_Result = new ContentInspectorBuilder() {
                    Definitions = Defintions,
                    Parallel = true,
                }.Build();
                ;
            }

            return GetEngine_Result;
        }

        [TestMethod]
        public void A_GenerateTests() {
            Console.WriteLine(GenerateTests());
        }

        protected string GenerateTests() {
            var tret = new StringBuilder();

            var FullPath = System.IO.Path.GetFullPath(RelativeRoot());

            foreach (var item in System.IO.Directory.GetFiles(FullPath)) {
                var FN = System.IO.Path.GetFileName(item);
                var Name = FN.Replace(".", "_");

                var Content = $@"
[TestMethod]
public void {Name}(){{
    Test(""{FN}"");
}}";

                tret.AppendLine(Content);
            }

            var ret = tret.ToString();
            return ret;
        }

        protected void Test(string RelativeFileName) {
            var FN = $@"{RelativeRoot()}{RelativeFileName}";
            var FullPath = System.IO.Path.GetFullPath(FN);
            var FileName = System.IO.Path.GetFileName(FullPath);

            var Content = ContentReader.Default.ReadFromFile(FullPath);

            var Engine = GetEngine();

            var AllResults = Engine.Inspect(Content).ByFileExtension();

            var Results = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            if (AllResults.Length > 0) {
                var MaxPoints = AllResults.First().Points;
                Results.UnionWith(
                    from x in AllResults 
                    where x.Points == MaxPoints
                    select x.Extension
                );
            }

            var Expected = FileName.Split('.').LastOrDefault()?.ToLowerInvariant() ?? string.Empty;

            var IsValid = Results.Contains(Expected);

            if (!IsValid) {
                Assert.AreNotEqual(Expected, string.Join(",", Results));
            }

        }

        protected virtual string RelativeRoot() {
            return @".\MicroTests\Data\";
        }
    }

}
