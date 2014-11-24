Imports System.Collections.Generic
Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config
Imports Db4objects.Db4o.Monitoring

Namespace Db4oDoc.Code.Tuning.Monitoring
    Public Class ReferenceSystemMonitoring
        Public Shared Sub Main(ByVal args As String())
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            ' #example: Add reference system monitoring
            configuration.Common.Add(New ReferenceSystemMonitoringSupport())
            ' #end example
            Using container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, "database.db4o")
                StoreObjects(container)
                Console.WriteLine("Press any key to end application...")
                BlowReferenceSystem(container)
                Console.WriteLine("done.")
            End Using
        End Sub


        Private Shared Sub StoreObjects(ByVal container As IObjectContainer)
            Dim rnd As New Random()
            For i As Integer = 0 To 499999
                container.Store(New DataObject(rnd))
            Next
            container.Commit()
        End Sub

        Private Shared Sub BlowReferenceSystem(ByVal container As IObjectContainer)
            Dim dataObjects As IList(Of DataObject) = container.Query(Of DataObject)()
            Dim hardReferences As New List(Of DataObject)()
            While Not Console.KeyAvailable
                For Each reference As DataObject In dataObjects
                    hardReferences.Add(reference)
                Next
                hardReferences.Clear()
            End While
        End Sub
    End Class
End Namespace