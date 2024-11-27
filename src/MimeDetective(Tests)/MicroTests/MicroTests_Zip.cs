using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MimeDetective.Tests;

[TestClass]
public class MicroTestsZip : MicroTests {

    protected override string RelativeRoot() {
        return base.RelativeRoot() + @"\Zip\";
    }

    [TestMethod]
    public void EmptiedBy7zip_zip() {
        Test("EmptiedBy7zip.zip");
    }

    [TestMethod]
    public void empty_zip() {
        Test("empty.zip");
    }

    [TestMethod]
    public void emptyZip_zip() {
        Test("emptyZip.zip");
    }

    [TestMethod]
    public void images_7z() {
        Test("images.7z");
    }

    [TestMethod]
    public void images_zip() {
        Test("images.zip");
    }

    [TestMethod]
    public void Images7zip_tar() {
        Test("Images7zip.tar");
    }

    [TestMethod]
    public void imagesBy7zip_zip() {
        Test("imagesBy7zip.zip");
    }

    [TestMethod]
    public void TestBlender_rar() {
        Test("TestBlender.rar");
    }

    [TestMethod]
    public void WinRar_rar() {
        Test("WinRar.rar");
    }

}