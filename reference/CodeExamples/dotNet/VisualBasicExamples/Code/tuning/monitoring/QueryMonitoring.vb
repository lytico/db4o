Imports System.Collections.Generic
Imports System.Linq
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config
Imports Db4objects.Db4o.Linq
Imports Db4objects.Db4o.Monitoring

Namespace Db4oDoc.Code.Tuning.Monitoring
    Public Class QueryMonitoring
        Public Shared Sub Main(ByVal args As String())
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            ' #example: Add query monitoring
            configuration.Common.Add(New QueryMonitoringSupport())
            configuration.Common.Add(New NativeQueryMonitoringSupport())
            ' #end example
            configuration.Common.ObjectClass(GetType(DataObject)).ObjectField("indexedNumber").Indexed(True)
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, "database.db4o")
                StoreALotOfObjects(container)
                Console.WriteLine("Press any key to end application...")
                QueryLoop(container)
                Console.WriteLine("done.")
            End Using
        End Sub


        Private Shared Sub QueryLoop(ByVal container As IObjectContainer)
            While Not Console.KeyAvailable
                RunLINQOptimizedQuery(container)
                RunLINQNotOptimizedQuery(container)
                RunUnoptimizedQuery(container)
                RunQueryOnNotIndexedField(container)
                RunQueryOnIndexedField(container)
            End While
        End Sub

        Private Shared Sub RunLINQOptimizedQuery(ByVal container As IObjectContainer)
            Dim result = (From o As DataObject In container _
             Where o.IndexedNumber = 42 _
             Select o).ToArray()
        End Sub

        Private Shared Sub RunLINQNotOptimizedQuery(ByVal container As IObjectContainer)
            Dim result = (From o As DataObject In container _
             Where o.IndexedNumber = New Random().Next() _
             Select o).ToArray()
        End Sub

        Private Shared Sub RunQueryOnIndexedField(ByVal container As IObjectContainer)
            Dim result As IList(Of DataObject) = container.Query(Function(o As DataObject) o.IndexedNumber = 42)
        End Sub

        Private Shared Sub RunQueryOnNotIndexedField(ByVal container As IObjectContainer)
            Dim result As IList(Of DataObject) = container.Query(Function(o As DataObject) o.Number = 42)
        End Sub

        Private Shared Sub RunUnoptimizedQuery(ByVal container As IObjectContainer)
            Dim result As IList(Of DataObject) = container.Query(Function(o As DataObject) o.Number = New Random().Next())
        End Sub

        Private Shared Sub StoreALotOfObjects(ByVal container As IObjectContainer)
            Dim rnd As New Random()
            For i As Integer = 0 To 9999
                container.Store(New DataObject(rnd.Next()))
            Next
        End Sub

        Private Class DataObject
            Private m_number As Integer
            Private m_indexedNumber As Integer

            Public Sub New(ByVal number As Integer)
                Me.m_number = number
                m_indexedNumber = number
            End Sub

            Public Property Number() As Integer
                Get
                    Return m_number
                End Get
                Set(ByVal value As Integer)
                    m_number = value
                End Set
            End Property

            Public Property IndexedNumber() As Integer
                Get
                    Return m_indexedNumber
                End Get
                Set(ByVal value As Integer)
                    m_indexedNumber = value
                End Set
            End Property
        End Class
    End Class
End Namespace