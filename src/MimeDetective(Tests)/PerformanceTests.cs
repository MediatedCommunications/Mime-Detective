using MimeDetective.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Immutable;
using System.Linq;

namespace MimeDetective.Tests {

    [TestClass]
    public class PerformanceTests
    {
        static PerformanceTests()
        {
            _ = ContentInspectors.Exhaustive.ContentInspector;
        }

        private const int Iterations_Per_Test = 1;

        [TestMethod]
        public void Engine_Test_Exe()
        {
            Test_Extension($@"C:\Windows\System32\Notepad.exe", "exe");
        }

        [TestMethod]
        public void Engine_Test_Doc()
        {
            Test_Extension($@"C:\Windows\System32\MSDRM\MsoIrmProtector.doc", "doc");
        }

        [TestMethod]
        public void Engine_Test_Msc()
        {
            Test_Extension($@"C:\Windows\System32\azman.msc", "msc");
        }

        [TestMethod]
        public void Engine_Test_Ico()
        {
            Test_Extension($@"C:\Windows\SysWow64\OneDrive.ico", "ico");
        }

        [TestMethod]
        public void Engine_Test_Bmp()
        {
            Test_Extension($@"C:\ProgramData\Microsoft\User Account Pictures\guest.bmp", "bmp");
        }

        [TestMethod]
        public void Engine_Test_Uce()
        {
            Test_Extension($@"C:\Windows\System32\SubRange.uce", "uce");
        }

        [TestMethod]
        public void Engine_Test_Wim()
        {
            Test_Extension($@"C:\Windows\System32\DrtmAuthTxt.wim", "wim");
        }

        [TestMethod]
        public void Engine_Test_Rtf()
        {
            Test_Extension($@"C:\Windows\System32\license.rtf", "rtf");
        }

        [TestMethod]
        public void Engine_Test_Gif()
        {
            Test_Extension($@"C:\Windows\System32\DesktopKeepOnToastImg.gif", "gif");
        }


        [TestMethod]
        public void Engine_Test_Png()
        {
            Test_Extension($@"C:\Windows\System32\ComputerToastIcon.png", "png");
        }

        private static ImmutableArray<FileExtensionMatch> Test_Extension(string FileName, string Extension) {
            var ret = ImmutableArray<FileExtensionMatch>.Empty;

            var Content = System.IO.File.ReadAllBytes(FileName);

            for (var i = 0; i < Iterations_Per_Test; i++)
            {
                ret = Test_Extension_Internal(Content, Extension);
            }


            return ret;
        }


        private static ImmutableArray<FileExtensionMatch> Test_Extension_Internal(byte[] Content, string Extension)
        {

            

            var Engine = ContentInspectors.Exhaustive.ContentInspector;
            var Results = Engine.Inspect(Content).ByFileExtension();
            
            Assert.AreEqual(Extension, Results.First().Extension);

            return Results;
        }

    }

}
