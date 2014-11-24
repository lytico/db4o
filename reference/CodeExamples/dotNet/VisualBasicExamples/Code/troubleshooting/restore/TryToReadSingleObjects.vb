Imports Db4objects.Db4o

Namespace Db4oDoc.Code.Troubleshooting.Restore
    Public Class TryToReadSingleObjects
        Private Const DatabaseFileName As String = "database.db4o"

        Public Shared Sub Main(args As String())
            StoreExampleObjects()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFileName)
                ' #example: Try to read the intact objects
                Dim idsOfPersons As Long() = container.Ext().StoredClass(GetType(Person)).GetIDs()
                For Each id As Long In idsOfPersons
                    Try
                        Dim person = DirectCast(container.Ext().GetByID(id), Person)
                        container.Ext().Activate(person, 1)
                        ' store the person to another database
                        Console.Out.WriteLine("This object is ok {0}", person)
                    Catch e As Exception
                        Console.Out.WriteLine("We couldn't read the object with the id {0} anymore." & " It is lost", id)
                        Console.Out.WriteLine(e)
                    End Try
                Next
                ' #end example
            End Using
        End Sub

        Private Shared Sub StoreExampleObjects()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFileName)
                For i As Integer = 0 To 99
                    container.Store(New Person("Fun" & i))
                Next
            End Using
        End Sub
    End Class


    Friend Class Person
        Private ReadOnly m_name As String

        Public Sub New(name As String)
            Me.m_name = name
        End Sub

        Public ReadOnly Property Name() As String
            Get
                Return m_name
            End Get
        End Property
    End Class
End Namespace
