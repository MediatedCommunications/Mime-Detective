Imports System.IO
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports MimeDetective
Imports MimeDetective.Definitions
Imports MimeDetective.Definitions.Licensing

<TestClass>
Public Class VbTests
    <TestMethod>
    Public Sub Empty()
        Dim E = New ContentInspectorBuilder().Build()
        Dim R = E.Inspect(Array.Empty (Of Byte)).FirstOrDefault()

        Assert.IsNull(R)
    End Sub

    <TestMethod>
    Public Sub EmptyZip()
        Dim Definitions = New CondensedBuilder() With {.UsageType = UsageType.PersonalNonCommercial}.Build()
        Dim ContentInspector = New ContentInspectorBuilder With {.Definitions = Definitions}.Build()

        Dim R = ContentInspector.Inspect(Path.Combine("Data", "empty.zip")).FirstOrDefault()

        Assert.AreEqual("APPLICATION/ZIP", R.Definition.File.MimeType)
    End Sub
End Class