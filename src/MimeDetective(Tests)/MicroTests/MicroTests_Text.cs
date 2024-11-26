using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MimeDetective.Tests;

[TestClass]
public class MicroTests_Text : MicroTests {

    protected override string RelativeRoot() {
        return base.RelativeRoot() + @"Text\";
    }

    [TestMethod]
    public void MindMap_NoBOM_smmx_xml() {
        Test("MindMap.NoBOM.smmx.xml");
    }

    [TestMethod]
    public void MindMap_UCS2BE_WithBOM_smmx_xml() {
        Test("MindMap.UCS2BE.WithBOM.smmx.xml");
    }

    [TestMethod]
    public void MindMap_UCS2LE_WithBOM_smmx_xml() {
        Test("MindMap.UCS2LE.WithBOM.smmx.xml");
    }

    [TestMethod]
    public void MindMap_WithBOM_smmx_xml() {
        Test("MindMap.WithBOM.smmx.xml");
    }

    [TestMethod]
    public void SuperSmall_txt() {
        Test("SuperSmall.txt");
    }

    [TestMethod]
    public void test_txt() {
        Test("test.txt");
    }

    [TestMethod]
    public void TextFile1_txt() {
        Test("TextFile1.txt");
    }

}