using FileDetection.Data.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Immutable;
using System.Linq;

namespace FileDetection.Tests
{
    [TestClass]
    public class MatchingTests
    {
        static MatchingTests()
        {
            GetEngine();
        }

        [TestMethod]
        public void Engine_Test_Exe()
        {
            Test_Extension($@"C:\Windows\System32\Notepad.exe", "EXE");
        }

        [TestMethod]
        public void Engine_Test_Doc()
        {
            Test_Extension($@"C:\Windows\System32\MSDRM\MsoIrmProtector.doc", "DOC");
        }

        [TestMethod]
        public void Engine_Test_Msc()
        {
            Test_Extension($@"C:\Windows\System32\azman.msc", "MSC");
        }

        [TestMethod]
        public void Engine_Test_Ico()
        {
            Test_Extension($@"C:\Windows\SysWow64\OneDrive.ico", "ICO");
        }

        [TestMethod]
        public void Engine_Test_Bmp()
        {
            Test_Extension($@"C:\Windows\System32\oobe\info\surface.bmp", "BMP");
        }

        [TestMethod]
        public void Engine_Test_Uce()
        {
            Test_Extension($@"C:\Windows\System32\SubRange.uce", "UCE");
        }

        [TestMethod]
        public void Engine_Test_Wim()
        {
            Test_Extension($@"C:\Windows\System32\DrtmAuthTxt.wim", "WIM");
        }

        [TestMethod]
        public void Engine_Test_Rtf()
        {
            Test_Extension($@"C:\Windows\System32\license.rtf", "RTF");
        }

        [TestMethod]
        public void Engine_Test_Gif()
        {
            Test_Extension($@"C:\Windows\System32\DesktopKeepOnToastImg.gif", "GIF");
        }


        [TestMethod]
        public void Engine_Test_Png()
        {
            Test_Extension($@"C:\Windows\System32\ComputerToastIcon.png", "PNG");
        }


        private static FileDetectionEngine? GetEngine_Result;
        private static FileDetectionEngine GetEngine()
        {
            if(GetEngine_Result == default)
            {
                var Defintions = Data.Large.Definitions();

                GetEngine_Result = new FileDetectionEngine()
                {
                    Definitions = Defintions,
                    MatchEvaluator = new()
                    {
                        Options = new()
                        {
                            //Include_Segments_Middle = false,
                        }
                    }
                };

                GetEngine_Result.WarmUp();
            }

            return GetEngine_Result;
        }

        private static ImmutableArray<ExtensionMatch> Test_Extension(string FileName, string Extension) {
            var ret = ImmutableArray<ExtensionMatch>.Empty;

            for (var i = 0; i < 10; i++)
            {
                ret = Test_Extension_Internal(FileName, Extension);
            }


            return ret;
        }


        private static ImmutableArray<ExtensionMatch> Test_Extension_Internal(string FileName, string Extension)
        {

            var Content = System.IO.File.ReadAllBytes(FileName);

            var Engine = GetEngine();
            var Results = Engine.Detect(Content).ByExtension();
            
            Assert.AreEqual(Extension, Results.First().Extension);

            return Results;
        }

    }

}
