Imports System.IO
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config
Imports Db4objects.Db4o.Diagnostic
Imports Db4objects.Db4o.Linq

Namespace Db4oDoc.Code.Indexing.Where
    Public Class WhereToIndexExample
        Private Const DatabaseFile As String = "database.db4o"

        Public Shared Sub Main(ByVal args As String())
            StoreObjects()
            RunQuery()
            AddIndex()
            RunQuery()
        End Sub

        Private Shared Sub AddIndex()
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            configuration.Common.ObjectClass(GetType(IndexedClass)).ObjectField("id").Indexed(True)
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)
                container.Query(Of IndexedClass)()
            End Using
        End Sub

        Private Shared Sub RunQuery()
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            ' #example: Find queries which cannot use index
            configuration.Common.Diagnostic.AddListener(New IndexDiagnostics())
            ' #end example
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, DatabaseFile)
                Dim result = From i As IndexedClass In container Select i
                result.Count()
            End Using
        End Sub


        Private Shared Sub StoreObjects()
            CleanUp()
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(DatabaseFile)
                StoreObjects(container)
            End Using
        End Sub

        Private Shared Sub CleanUp()
            File.Delete(DatabaseFile)
        End Sub

        Private Shared Sub StoreObjects(ByVal container As IObjectContainer)
            Dim rnd As New Random()
            For i As Integer = 0 To 9999
                container.Store(IndexedClass.Create(rnd))
            Next
        End Sub
    End Class

    ' #example: Index diagnostics
    Friend Class IndexDiagnostics
        Implements IDiagnosticListener
        Public Sub OnDiagnostic(ByVal diagnostic As IDiagnostic) _
            Implements IDiagnosticListener.OnDiagnostic
            If TypeOf diagnostic Is LoadedFromClassIndex Then
                Console.WriteLine("This query couldn't use field indexes " & DirectCast(diagnostic, LoadedFromClassIndex).Reason())
                Console.WriteLine(diagnostic)
            End If
        End Sub
    End Class
    ' #end example

    Friend Class IndexedClass
        Private m_id As Integer
        Private otherData As String

        Public Sub New(ByVal id As Integer)
            Me.m_id = id
            otherData = "This is more data =)"
        End Sub

        Public Shared Function Create(ByVal rnd As Random) As IndexedClass
            Dim intIndex As Integer = NewInt(rnd)
            Return New IndexedClass(intIndex)
        End Function

        Private Shared Function NewInt(ByVal rnd As Random) As Integer
            Return rnd.Next()
        End Function

        Public ReadOnly Property ID() As Integer
            Get
                Return m_id
            End Get
        End Property
    End Class
End Namespace
