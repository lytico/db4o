Imports System.Collections.Generic
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config
Imports Db4objects.Db4o.Monitoring

Namespace Db4oDoc.Code.Tuning.Monitoring
    Public Class FreespaceMonitoring
        Public Shared Sub Main(ByVal args As String())
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            ' #example: Monitor the free-space system
            configuration.Common.Add(New FreespaceMonitoringSupport())
            ' #end example
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, "database.db4o")
                Console.WriteLine("Press any key to end application...")
                TryToFragmentDatabase(container)
                Console.WriteLine("done.")
            End Using
        End Sub

        Private Shared Sub TryToFragmentDatabase(ByVal container As IObjectContainer)
            While Not Console.KeyAvailable
                Dim rnd As New Random()
                StoreData(container, rnd)
                DeleteData(container, rnd)
                container.Commit()
            End While
        End Sub

        Private Shared Sub DeleteData(ByVal container As IObjectContainer, ByVal rnd As Random)
            Dim data As IList(Of DataObject) = container.Query(Of DataObject)()
            For i As Integer = 0 To rnd.Next(4096) - 1
                Dim obj As DataObject = data(rnd.Next(data.Count))
                If obj IsNot Nothing Then
                    container.Delete(obj)
                End If
            Next
        End Sub

        Private Shared Sub StoreData(ByVal container As IObjectContainer, ByVal rnd As Random)
            For i As Integer = 0 To rnd.Next(4096) - 1
                container.Store(New DataObject(rnd))
            Next
        End Sub
    End Class
End Namespace