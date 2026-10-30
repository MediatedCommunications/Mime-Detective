Imports System.IO
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports MimeDetective
Imports MimeDetective.Definitions
Imports MimeDetective.Definitions.Licensing

<TestClass>
Public Class VbTests
    <TestMethod>
    Public Sub Empty()
        Dim Definitions = CondensedDefinitions.Load(UsageType.PersonalNonCommercial)
        Dim ContentInspector = Definitions.ToContentInspector()

        Dim R = ContentInspector.Inspect(Array.Empty(Of Byte))

        Assert.IsNull(R)
    End Sub

    <TestMethod>
    Public Sub EmptyZip()
        Dim Definitions = CondensedDefinitions.Load(UsageType.PersonalNonCommercial)
        Dim ContentInspector = Definitions.ToContentInspector()

        Dim R = ContentInspector.Inspect(Path.Combine("Data", "empty.zip"), ContentReader.Default)

        Assert.AreEqual("APPLICATION/ZIP", R.Definitions.FirstOrDefault()?.File?.MimeType)
    End Sub
End Class