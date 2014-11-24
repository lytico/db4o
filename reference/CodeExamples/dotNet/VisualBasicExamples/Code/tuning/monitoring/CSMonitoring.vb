Imports System.Collections.Generic
Imports Db4objects.Db4o
Imports Db4objects.Db4o.CS
Imports Db4objects.Db4o.CS.Config
Imports Db4objects.Db4o.CS.Monitoring

Namespace Db4oDoc.Code.Tuning.Monitoring
    Friend Class CSMonitoring
        Private Const PortNumber As Integer = 1337
        Private Const User As String = "sa"
        Private Const Password As String = "pwd"

        Public Shared Sub Main(ByVal args As String())
            Using server As IObjectServer = StartServer()
                RunClient()
            End Using
        End Sub

        Private Shared Sub RunClient()
            Dim configuration As IClientConfiguration = Db4oClientServer.NewClientConfiguration()
            configuration.Common.Add(New NetworkingMonitoringSupport())
            Using client As IObjectContainer = Db4oClientServer.OpenClient(configuration, "localhost", PortNumber, User, Password)
                DoOperationsOnClient(client)
            End Using
        End Sub

        Private Shared Sub DoOperationsOnClient(ByVal container As IObjectContainer)
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

        Private Shared Function StartServer() As IObjectServer
            Dim configuration As IServerConfiguration = Db4oClientServer.NewServerConfiguration()
            ' #example: Add the network monitoring support
            configuration.Common.Add(New NetworkingMonitoringSupport())
            ' #end example
            ' #example: Add the client connections monitoring support
            configuration.AddConfigurationItem(New ClientConnectionsMonitoringSupport())
            ' #end example
            Dim server As IObjectServer = Db4oClientServer.OpenServer(configuration, "database.db4o", PortNumber)
            server.GrantAccess(User, Password)
            Return server
        End Function
    End Class
End Namespace