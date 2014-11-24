Imports Db4objects.Db4o
Imports Db4objects.Db4o.CS
Imports Db4objects.Db4o.CS.Config

Namespace Db4oDoc.Code.Configuration.Server
    Public Class ServerConfigurationExamples

        Private Shared Sub SocketTimeout()
            ' #example: configure the socket-timeout
            Dim configuration As IServerConfiguration = Db4oClientServer.NewServerConfiguration()
            configuration.TimeoutServerSocket = (10 * 60 * 1000)
            ' #end example

            Dim container As IObjectServer = Db4oClientServer.OpenServer(configuration, "database.db4o", 1337)

            container.Close()
        End Sub
    End Class

End Namespace