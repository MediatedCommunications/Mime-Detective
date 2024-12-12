using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace MimeDetective.Tests;

[TestClass]
public class MicroTestsDocuments : MicroTests {
    protected override string RelativeRoot() => Path.Combine(base.RelativeRoot(), "Documents");

    [TestMethod]
    public void DocWord2016_doc() {
        Test("DocWord2016.doc");
    }

    [TestMethod]
    public void DocWord97_doc() {
        Test("DocWord97.doc");
    }

    [TestMethod]
    public void DocxWord2016_docx() {
        Test("DocxWord2016.docx");
    }

    [TestMethod]
    public void GithubTestPdf2_pdf() {
        Test("GithubTestPdf2.pdf");
    }

    [TestMethod]
    public void microsoftPrintToPdf_pdf() {
        Test("microsoftPrintToPdf.pdf");
    }

    [TestMethod]
    public void OpenDocWord2016_odt() {
        Test("OpenDocWord2016.odt");
    }

    [TestMethod]
    public void OpenOfficeDoc_odt() {
        Test("OpenOfficeDoc.odt");
    }

    [TestMethod]
    public void OpenOfficeExcel_xls() {
        Test("OpenOfficeExcel.xls");
    }

    [TestMethod]
    public void OpenOfficeExcel50_xls() {
        Test("OpenOfficeExcel50.xls");
    }

    [TestMethod]
    public void OpenOfficeExcel95_xls() {
        Test("OpenOfficeExcel95.xls");
    }

    [TestMethod]
    public void OpenOfficePpt_ppt() {
        Test("OpenOfficePpt.ppt");
    }

    [TestMethod]
    public void OpenOfficePresentation_odp() {
        Test("OpenOfficePresentation.odp");
    }

    [TestMethod]
    public void OpenOfficeRtf_rtf() {
        Test("OpenOfficeRtf.rtf");
    }

    [TestMethod]
    public void OpenOfficeSpreadsheet_ods() {
        Test("OpenOfficeSpreadsheet.ods");
    }

    [TestMethod]
    public void OpenOfficeWord6_0Doc_doc() {
        Test("OpenOfficeWord6.0Doc.doc");
    }

    [TestMethod]
    public void OpenOfficeWord95Doc_doc() {
        Test("OpenOfficeWord95Doc.doc");
    }

    [TestMethod]
    public void OpenOfficeWordDoc_doc() {
        Test("OpenOfficeWordDoc.doc");
    }

    [TestMethod]
    public void PdfWord2016_pdf() {
        Test("PdfWord2016.pdf");
    }

    [TestMethod]
    public void PptPowerpoint2016_ppt() {
        Test("PptPowerpoint2016.ppt");
    }

    [TestMethod]
    public void PptxPowerpoint2016_pptx() {
        Test("PptxPowerpoint2016.pptx");
    }

    [TestMethod]
    public void RichTextWord2016_rtf() {
        Test("RichTextWord2016.rtf");
    }

    [TestMethod]
    public void StrictOpenXMLWord2016_docx() {
        Test("StrictOpenXMLWord2016.docx");
    }

    [TestMethod]
    public void test_xlsx() {
        Test("test.xlsx");
    }

    [TestMethod]
    public void XlsExcel2007_xls() {
        Test("XlsExcel2007.xls");
    }

    [TestMethod]
    public void XlsExcel2016_xls() {
        Test("XlsExcel2016.xls");
    }

    [TestMethod]
    public void XlsxExcel2016_xlsx() {
        Test("XlsxExcel2016.xlsx");
    }
}
