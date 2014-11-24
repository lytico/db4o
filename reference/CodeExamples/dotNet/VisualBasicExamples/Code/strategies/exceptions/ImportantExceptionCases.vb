Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config
Imports Db4objects.Db4o.Constraints
Imports Db4objects.Db4o.CS
Imports Db4objects.Db4o.Ext

Namespace Db4oDoc.Code.Strategies.Exceptions
    Public Class ImportantExceptionCases
        Public Shared Sub Main(ByVal args As String())
            AlreadyOpenDatabaseThrows()
            ConnectToNotExistingServer()
            UniqueViolation()
        End Sub

        Private Shared Sub UniqueViolation()
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.ObjectClass(GetType(UniqueId)).ObjectField("id").Indexed(True)
            configuration.Common.Add(New UniqueFieldValueConstraint(GetType(UniqueId), "id"))
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, "database.db4o")
                ' #example: Violation of the unique constraint
                container.Store(New UniqueId(42))
                container.Store(New UniqueId(42))
                Try
                    container.Commit()
                Catch e As UniqueFieldValueConstraintViolationException
                    ' Violated the unique-constraint!
                    ' Retry with a new value or handle this gracefully
                    container.Rollback()
                End Try
                ' #end example
            End Using
        End Sub

        Private Shared Sub ConnectToNotExistingServer()
            ' #example: Cannot connect to the server
            Try
                Dim container As IObjectContainer = Db4oClientServer.OpenClient("localhost", 1337, "sa", "sa")
            Catch e As Db4oIOException
                ' Couldn't connect to the server.
                ' Ask for new connection-settings or handle this case gracefully
            End Try
            ' #end example
        End Sub

        Private Shared Sub AlreadyOpenDatabaseThrows()
            Dim allReadyOpen As IObjectContainer = Db4oEmbedded.OpenFile("database.db4o")
            Try
                ' #example: If the database is already open
                Try
                    Dim container As IObjectContainer = Db4oEmbedded.OpenFile("database.db4o")
                Catch e As DatabaseFileLockedException
                    ' Database is already open!
                    ' Use another database-file or handle this case gracefully
                End Try
                ' #end example
            Finally
                allReadyOpen.Close()
            End Try
        End Sub


        Private Class UniqueId
            Private id As Integer

            Friend Sub New(ByVal id As Integer)
                Me.id = id
            End Sub
        End Class
    End Class
End Namespace