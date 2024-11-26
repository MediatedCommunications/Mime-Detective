using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MimeDetective.Tests;

[TestClass]
public class LookupTests {

    private string application_octetstream = "application/octet-stream";

    [TestMethod]
    public void FileExtension_AllowVariants() {
        var expected = application_octetstream;

        var V1 = ContentInspectors.Exhaustive.FileExtensionToMimeTypeLookup.TryGetValue("exe");
        var V2 = ContentInspectors.Exhaustive.FileExtensionToMimeTypeLookup.TryGetValue(".exe");
        var V3 = ContentInspectors.Exhaustive.FileExtensionToMimeTypeLookup.TryGetValue("ExE");
        var V4 = ContentInspectors.Exhaustive.FileExtensionToMimeTypeLookup.TryGetValue(".ExE");

        Assert.AreEqual(expected, V1);
        Assert.AreEqual(expected, V2);
        Assert.AreEqual(expected, V3);
        Assert.AreEqual(expected, V4);
    }

    [TestMethod]
    public void MimeType_AllowVariants() {
        var expected = "exe";

        var V1 = ContentInspectors.Exhaustive.MimeTypeToFileExtensionLookup.TryGetValue("application/octet-stream");
        var V2 = ContentInspectors.Exhaustive.MimeTypeToFileExtensionLookup.TryGetValue("application/OCTET-stream");

        Assert.AreEqual(expected, V1);
        Assert.AreEqual(expected, V2);

    }

}