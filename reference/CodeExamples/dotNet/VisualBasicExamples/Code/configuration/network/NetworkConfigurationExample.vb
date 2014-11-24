Imports Db4objects.Db4o
Imports Db4objects.Db4o.CS
Imports Db4objects.Db4o.CS.Config
Imports Db4objects.Db4o.CS.Internal.Config
Imports Db4objects.Db4o.CS.Foundation

Namespace Db4oDoc.Code.Configuration.Network
    Public Class NetworkConfigurationExample

        Private Shared Sub EnableBatchMode()
            ' #example: enable or disable batch mode
            Dim configuration As IClientConfiguration = Db4oClientServer.NewClientConfiguration()
            configuration.Networking.BatchMessages = True
            ' #end example
            Dim container As IObjectContainer = Db4oClientServer.OpenClient(configuration, "localhost", 1337, "sa", "sa")
        End Sub

        Private Shared Sub ReplaceClientServerFactory()
            ' #example: exchange the way a client or server is created
            Dim configuration As IClientConfiguration = Db4oClientServer.NewClientConfiguration()
            configuration.Networking.ClientServerFactory = New StandardClientServerFactory()
            ' #end example
            Dim container As IObjectContainer = Db4oClientServer.OpenClient(configuration, "localhost", 1337, "sa", "sa")
        End Sub

        Private Shared Sub MaxBatchQueueSize()
            ' #example: change the maximum batch queue size
            Dim configuration As IClientConfiguration = Db4oClientServer.NewClientConfiguration()
            configuration.Networking.MaxBatchQueueSize = 1024
            ' #end example
            Dim container As IObjectContainer = Db4oClientServer.OpenClient(configuration, "localhost", 1337, "sa", "sa")
        End Sub

        Private Shared Sub SingleThreadedClient()
            ' #example: single threaded client
            Dim configuration As IClientConfiguration = Db4oClientServer.NewClientConfiguration()
            configuration.Networking.SingleThreadedClient = True
            ' #end example
            Dim container As IObjectContainer = Db4oClientServer.OpenClient(configuration, "localhost", 1337, "sa", "sa")
        End Sub

        Private Shared Sub PluggableSocket()
            ' #example: Exchange the socket-factory
            Dim configuration As IClientConfiguration = Db4oClientServer.NewClientConfiguration()
            configuration.Networking.SocketFactory = New StandardSocket4Factory()
            ' #end example
            Dim container As IObjectContainer = Db4oClientServer.OpenClient(configuration, "localhost", 1337, "sa", "sa")
        End Sub



    End Class
End Namespace