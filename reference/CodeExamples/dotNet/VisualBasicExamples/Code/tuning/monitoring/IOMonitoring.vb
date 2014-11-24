Imports System.Collections.Generic
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config
Imports Db4objects.Db4o.Monitoring

Namespace Db4oDoc.Code.Tuning.Monitoring
    Public Class IOMonitoring
        Public Shared Sub Main(ByVal args As String())
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            ' #example: Add IO-Monitoring
            configuration.Common.Add(New IOMonitoringSupport())
            ' #end example
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, "database.db4o")
                Console.WriteLine("Press any key to end application...")
                DoIoOperations(container)
                Console.WriteLine("done.")
            End Using
        End Sub

        Private Shared Sub DoIoOperations(ByVal container As IObjectContainer)
            While Not Console.KeyAvailable
                StoreALot(container)
                ReadALot(container)
            End While
        End Sub

        Private Shared Sub ReadALot(ByVal container As IObjectContainer)
            Dim allObjects As IList(Of DataObject) = container.Query(Of DataObject)()
            For Each obj As DataObject In allObjects
                obj.ToString()
            Next
        End Sub

        Private Shared Sub StoreALot(ByVal container As IObjectContainer)
            Dim rnd As New Random()
            For i As Integer = 0 To 1023
                container.Store(New DataObject(rnd))
            Next
            container.Commit()
        End Sub
    End Class
End Namespace