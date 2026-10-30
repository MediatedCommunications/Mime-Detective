using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace MimeDetective.Tests;

[TestClass]
public class PerformanceTests {
    private const int IterationsPerTest = 1;

    static PerformanceTests() {
        _ = ContentInspectors.Exhaustive.ContentInspector;
    }

    [TestMethod]
    public void Engine_Test_Exe() {
        Test_Extension(@"C:\Windows\System32\Notepad.exe", "exe");
    }

    [TestMethod]
    public void Engine_Test_Doc() {
        Test_Extension(@"C:\Windows\System32\MSDRM\MsoIrmProtector.doc", "doc");
    }

    [TestMethod]
    public void Engine_Test_Msc() {
        Test_Extension(@"C:\Windows\System32\azman.msc", "msc");
    }

    [TestMethod]
    public void Engine_Test_Ico() {
        Test_Extension(@"C:\Windows\SysWow64\OneDrive.ico", "ico");
    }

    [TestMethod]
    public void Engine_Test_Bmp() {
        Test_Extension(@"C:\ProgramData\Microsoft\User Account Pictures\guest.bmp", "bmp");
    }

    [TestMethod]
    public void Engine_Test_Uce() {
        Test_Extension(@"C:\Windows\System32\SubRange.uce", "uce");
    }

    [TestMethod]
    public void Engine_Test_Wim() {
        Test_Extension(@"C:\Windows\System32\DrtmAuthTxt.wim", "wim");
    }

    [TestMethod]
    public void Engine_Test_Rtf() {
        Test_Extension(@"C:\Windows\System32\license.rtf", "rtf");
    }

    [TestMethod]
    public void Engine_Test_Gif() {
        Test_Extension(@"C:\Windows\System32\DesktopKeepOnToastImg.gif", "gif");
    }


    [TestMethod]
    public void Engine_Test_Png() {
        Test_Extension(@"C:\Windows\System32\ComputerToastIcon.png", "png");
    }

    private static ContentInspectorResults Test_Extension(string fileName, string extension) {
        if (!File.Exists(fileName)) {
            Assert.Inconclusive("File not found");
        }


        var content = File.ReadAllBytes(fileName);

        var ret = Test_Extension_Internal(content, extension);
        for (var i = 0; i < IterationsPerTest; i++) {
            ret = Test_Extension_Internal(content, extension);
        }


        return ret;
    }


    private static ContentInspectorResults Test_Extension_Internal(byte[] content, string extension) {
        var engine = ContentInspectors.Exhaustive.ContentInspector;
        var results = engine.Inspect(content);

        Assert.AreEqual(extension.ToUpperInvariant(), results.ByExtension.First().Key);

        return results;
    }
}
