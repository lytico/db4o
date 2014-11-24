Imports Db4objects.Db4o
Imports Db4objects.Db4o.CS

Namespace Db4oTutorialCode.Code.ClientServer
    Public Class ClientServer
        Private Const DatabaseFile As String = "database.db4o"

        Public Shared Sub Main(args As String())

            SessionContainers()
            RunClientServer()
            StartServer()
        End Sub

        Private Shared Sub SessionContainers()
            Using rootContainer As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                ' #example: Creating a session container
                Using container As IObjectContainer = rootContainer.Ext().OpenSession()
                    ' We now can use this session container like any other container
                End Using
                ' #end example
            End Using
        End Sub

        Private Shared Sub StartServer()
            ' #example: Open server
            Using server As IObjectServer = Db4oClientServer.OpenServer("database.db4o", 8080)
                ' allow access to this server
                server.GrantAccess("user", "password")

                ' Keep server running as long as you need it
                Console.Out.WriteLine("Press any key to exit.")
                Console.Read()
                Console.Out.WriteLine("Exiting...")
            End Using
            ' #end example
        End Sub

        Private Shared Sub RunClientServer()
            Using server As IObjectServer = Db4oClientServer.OpenServer("database.db4o", 8080)
                server.GrantAccess("user", "password")

                OpenClient()
            End Using
        End Sub

        Private Shared Sub OpenClient()
            ' #example: Using the client
            Using container As IObjectContainer = Db4oClientServer.OpenClient("localhost", 8080, "user", "password")
                ' Use the client object container as usual
            End Using
            ' #end example
        End Sub
    End Class
End Namespace
