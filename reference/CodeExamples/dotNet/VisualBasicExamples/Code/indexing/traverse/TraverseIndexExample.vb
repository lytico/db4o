Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config.Attributes
Imports Db4objects.Db4o.Ext
Imports Db4objects.Db4o.Foundation

Namespace Db4oDoc.Code.Indexing.Traverse
    Public Class TraverseIndexExample
        Public Shared Sub Main(args As String())
            Using container As IObjectContainer = Db4oEmbedded.OpenFile("database.db4o")
                StoreExampleObjects(container)

                TraverseIndex(container)
            End Using
        End Sub

        Private Shared Sub TraverseIndex(container As IObjectContainer)
            ' #example: Traverse index
            Dim storedField As IStoredField = container.Ext().StoredClass(GetType(Item)).StoredField("data", GetType(Integer))
            storedField.TraverseValues(New IndexVisitor())
            ' #end example
        End Sub

        ' #example: The index visitor
        Private Class IndexVisitor
            Implements IVisitor4
            Public Sub Visit(obj As Object) Implements IVisitor4.Visit
                Dim value = CInt(obj)
                Console.Out.WriteLine("Value {0}", value)
            End Sub
        End Class
        ' #end example

        Private Shared Sub StoreExampleObjects(container As IObjectContainer)
            For i As Integer = 0 To 99
                container.Store(New Item(i))
            Next
        End Sub
    End Class

    Friend Class Item
        <Indexed()> _
        Private data As Integer

        Public Sub New(data As Integer)
            Me.data = data
        End Sub
    End Class
End Namespace
