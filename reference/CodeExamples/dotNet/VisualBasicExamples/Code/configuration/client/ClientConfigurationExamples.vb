Imports Db4objects.Db4o
Imports Db4objects.Db4o.CS
Imports Db4objects.Db4o.CS.Config

Namespace Db4oDoc.Code.Configuration.Client

    Public Class ClientConfigurationExamples


        Public Shared Sub PrefetchDepth()
            ' #example: Configure the prefetch depth
            Dim configuration As IClientConfiguration = Db4oClientServer.NewClientConfiguration()
            configuration.PrefetchDepth = 5
            ' #end example
            Dim container As IObjectContainer = Db4oClientServer.OpenClient(configuration, "localhost", 1337, "user", "password")
            container.Close()

        End Sub
        Public Shared Sub PrefetchObjectCount()
            ' #example: Configure the prefetch object count
            Dim configuration As IClientConfiguration = Db4oClientServer.NewClientConfiguration()
            configuration.PrefetchObjectCount = 500
            ' #end example
            Dim container As IObjectContainer = Db4oClientServer.OpenClient(configuration, "localhost", 1337, "user", "password")
            container.Close()

        End Sub
        Public Shared Sub PrefetchSlotCacheSize()
            ' #example: Configure the slot cache
            Dim configuration As IClientConfiguration = Db4oClientServer.NewClientConfiguration()
            configuration.PrefetchSlotCacheSize = 1024
            ' #end example
            Dim container As IObjectContainer = Db4oClientServer.OpenClient(configuration, "localhost", 1337, "user", "password")
            container.Close()

        End Sub

        Public Shared Sub PrefetchIDCount()
            ' #example: Configure the prefetch id count
            Dim configuration As IClientConfiguration = Db4oClientServer.NewClientConfiguration()
            configuration.PrefetchSlotCacheSize = 128
            ' #end example
            Dim container As IObjectContainer = Db4oClientServer.OpenClient(configuration, "localhost", 1337, "user", "password")
            container.Close()

        End Sub

        Public Shared Sub ConnectionTimeOut()
            ' #example: Configure the timeout
            Dim configuration As IClientConfiguration = Db4oClientServer.NewClientConfiguration()
            configuration.TimeoutClientSocket = (1 * 60 * 1000)
            ' #end example
            Dim container As IObjectContainer = Db4oClientServer.OpenClient(configuration, "localhost", 1337, "user", "password")
            container.Close()

        End Sub
    End Class
End Namespace