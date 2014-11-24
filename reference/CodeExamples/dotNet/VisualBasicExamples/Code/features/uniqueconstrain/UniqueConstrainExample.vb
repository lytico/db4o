Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config
Imports Db4objects.Db4o.Constraints

Namespace Db4oDoc.Code.Features.UniqueConstrain
    Public Class UniqueConstrainExample
        Public Shared Sub Main(ByVal args As String())
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            ' #example: Add the index the field and then the unique constrain
            configuration.Common.ObjectClass(GetType(UniqueId)).ObjectField("id").Indexed(True)
            configuration.Common.Add(New UniqueFieldValueConstraint(GetType(UniqueId), "id"))
            ' #end example
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, "database.db4o")
                container.Store(New UniqueId(44))
                ' #example: Violating the constrain throws an exception
                container.Store(New UniqueId(42))
                container.Store(New UniqueId(42))
                Try
                    container.Commit()
                Catch e As UniqueFieldValueConstraintViolationException
                    Console.Out.WriteLine(e.StackTrace)
                End Try
                ' #end example
            End Using
        End Sub

        Private Class UniqueId
            Private ReadOnly id As Integer

            Public Sub New(ByVal id As Integer)
                Me.id = id
            End Sub
        End Class
    End Class
End Namespace
