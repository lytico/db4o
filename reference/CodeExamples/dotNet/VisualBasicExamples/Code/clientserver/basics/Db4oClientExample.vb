Imports Db4objects.Db4o
Imports Db4objects.Db4o.CS

Namespace Db4oDoc.Code.ClientServer.Basics
    Public Class Db4oClientExample
        Public Shared Sub main(ByVal args As String())
            ' #example: Connect to the server
            ' Your operations
            Using container As IObjectContainer = Db4oClientServer.OpenClient("localhost", 8080, "user", "password")
            End Using
            ' #end example
        End Sub
    End Class
End Namespace
