Imports System.Threading
Imports Db4objects.Db4o
Imports Db4objects.Db4o.CS

Namespace Db4oDoc.Code.ClientServer.Basics
    Public Class ServerExample
        Public Shared Sub Main(ByVal args As String())
            ' #example: Start a db4o server
            Using server As IObjectServer = Db4oClientServer.OpenServer("database.db4o", 8080)
                server.GrantAccess("user", "password")

                ' Let the server run.
                LetServerRun()
            End Using
            ' #end example
        End Sub

        Private Shared Sub LetServerRun()
            Console.WriteLine("Press a key to close the server")
            While Console.KeyAvailable
                Thread.Sleep(1000)
            End While
        End Sub
    End Class
End Namespace
