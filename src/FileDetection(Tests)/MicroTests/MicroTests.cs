using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileDetection.Tests {
    [TestClass]
    public abstract class MicroTests {
        private static IContentDetectionEngine? GetEngine_Result;
        protected static IContentDetectionEngine GetEngine() {
            if (GetEngine_Result == default) {
                var Defintions = Data.Micro.All();

                GetEngine_Result = new ContentDetectionEngineBuilder() {
                    Definitions = Defintions,
                }.Build();
                ;
            }

            return GetEngine_Result;
        }

        [TestMethod]
        public void __GenerateTests() {
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

        protected void Test(string FileName) {
            var FN = $@"{RelativeRoot()}{FileName}";
            var FullPath = System.IO.Path.GetFullPath(FN);

            var Content = ContentReader.Default.ReadFromFile(FullPath);

            var Engine = GetEngine();

            var AllResults = Engine.Detect(Content).ByFileExtension();

            var Results = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            if (AllResults.Length > 0) {
                var MaxPoints = AllResults.First().Points;
                Results.UnionWith(
                    from x in AllResults 
                    where x.Points == MaxPoints
                    select x.Extension
                );
            }

            var Expected = FileName.Split('.').LastOrDefault()?.ToLower() ?? string.Empty;

            var IsValid = Results.Contains(Expected);

            if (!IsValid) {
                Assert.AreEqual(Expected, string.Join(",", Results));
            }

        }

        protected virtual string RelativeRoot() => @".\MicroTests\Data\";

    }

}
