Imports System.Collections.Generic
Imports System.IO
Imports Db4objects.Db4o

Namespace Db4oDoc.Code.Strategies.Refactoring.ArrayChange
    Public Class ChangeArrayType
        Public Const DatabaseFile As String = "database.db4o"

        Public Shared Sub Main(ByVal args As String())
            CleanUp()
            StoreOldData()
            ListItems(Of PersonOld)()
            RefactorToArrayType()
            ListItems(Of PersonNew)()
        End Sub

        Private Shared Sub RefactorToArrayType()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                ' #example: Copy the string-field to the new string-array field
                Dim oldPersons As IList(Of PersonOld) = container.Query(Of PersonOld)()
                For Each old As PersonOld In oldPersons
                    Dim newPerson As New PersonNew()
                    newPerson.Name = New String() {old.Name}
                    container.Store(newPerson)
                    container.Delete(old)
                Next
                ' #end example
            End Using
        End Sub

        Private Shared Sub ListItems(Of T)()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                For Each obj As Object In container.Query(Of T)()
                    Console.WriteLine(obj)
                Next
            End Using
        End Sub

        Private Shared Sub StoreOldData()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                container.Store(New PersonOld("Joe"))
                container.Store(New PersonOld("Joanna"))
                container.Store(New PersonOld("Joel"))
            End Using
        End Sub


        Private Shared Sub CleanUp()
            File.Delete(DatabaseFile)
        End Sub
    End Class


    Friend Class PersonOld
        Private m_name As String = "Roman"

        Public Sub New(ByVal name As String)
            Me.m_name = name
        End Sub

        Public Property Name() As String
            Get
                Return m_name
            End Get
            Set(ByVal value As String)
                m_name = value
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return String.Format("Name: {0}", m_name)
        End Function
    End Class

    Friend Class PersonNew
        Private m_name As String() = New String() {"Roman"}

        Public Sub New(ByVal ParamArray name As String())
            Me.m_name = name
        End Sub

        Public Property Name() As String()
            Get
                Return m_name
            End Get
            Set(ByVal value As String())
                m_name = value
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return String.Format("Name: {0}", m_name)
        End Function
    End Class
End Namespace