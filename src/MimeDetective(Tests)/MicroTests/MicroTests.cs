using Microsoft.VisualStudio.TestTools.UnitTesting;
using MimeDetective.Definitions;
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
                var Definitions = MimeDetective.MinimalDefinitions.FileTypes.Email.EML();

                GetEngine_Result = ContentInspector.Create(Definitions);
            }




            if (GetEngine_Result == default) {
                var Definitions = MimeDetective.MinimalDefinitions.All();

                GetEngine_Result = ContentInspector.Create(Definitions);
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
            Test1(RelativeFileName);
        }

        protected void Test1(string RelativeFileName) {
            var FN = System.IO.Path.Combine(RelativeRoot(), RelativeFileName);
            var FullPath = System.IO.Path.GetFullPath(FN);
            var FileName = System.IO.Path.GetFileName(FullPath);

            var Content = ContentReader.Default.ReadFromFile(FullPath);

            var Engine = ExhaustiveDefinitions.Load(UsageType.PersonalNonCommercial).ToContentInspector();

            var AllResults = Engine.Inspect(Content.AsSpan());

            var Results = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            foreach(var Result in AllResults.Definitions) {
                foreach(var Ext in Result.File.Extensions) {
                    Results.Add(Ext);
                }
            }

            var Expected = FileName.Split('.').LastOrDefault()?.ToUpperInvariant() ?? string.Empty;

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
