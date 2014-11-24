Imports System.Collections.Generic
Imports Db4oUnit.Extensions
Imports NUnit.Framework

Namespace Db4oDoc.Code.DB4OTests


    ' #example: Basic test case
    Public Class ExampleTestCase
        Inherits AbstractDb4oTestCase
        Public Shared Sub Main(ByVal args As String())
            Dim testCase = New ExampleTestCase()
            testCase.RunSolo()
        End Sub

        Public Sub TestStoresElement()
            Db().Store(New TestItem())
            Dim result As IList(Of TestItem) = Db().Query(Of TestItem)()
            Assert.AreEqual(1, result.Count)
        End Sub


        Private Class TestItem

        End Class
    End Class
    ' #end example
End Namespace
