Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config
Imports Db4objects.Db4o.Ext

Namespace Db4oDoc.Code.Indexing.Check
    Public Class CheckForAndIndex
        Public Shared Sub Main(ByVal args As String())
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.ObjectClass(GetType(IndexedClass)).ObjectField("id").Indexed(True)
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, "database.db4o")
                container.Store(New IndexedClass(1))
                ' #example: Check for a index
                Dim metaInfo As IStoredClass = container.Ext().StoredClass(GetType(IndexedClass))
                ' list a fields and check if they have a index
                For Each field As IStoredField In metaInfo.GetStoredFields()
                    If field.HasIndex() Then
                        Console.WriteLine("The field '" & field.GetName() & "' is indexed")
                    Else
                        Console.WriteLine("The field '" & field.GetName() & "' isn't indexed")
                    End If
                    ' #end example
                Next
            End Using
        End Sub

        Private Class IndexedClass
            Private id As Integer
            Private data As String

            Public Sub New(ByVal id As Integer)
                Me.id = id
            End Sub
        End Class
    End Class
End Namespace
