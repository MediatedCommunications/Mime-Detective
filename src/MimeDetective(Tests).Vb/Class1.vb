Imports Microsoft.VisualStudio.TestTools.UnitTesting

Namespace MimeDetective_VB.Tests.Vb

    <TestClass>
    Public Class VbTests

        '<TestMethod>
        Public Sub Test1()
            Dim E = New MimeDetective.ContentInspectorBuilder().Build()
            Dim R = E.Inspect(Array.Empty(Of Byte)).First()
            Dim S = R.Definition.File.MimeType




        End Sub


    End Class

End Namespace