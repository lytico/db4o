Imports Db4objects.Db4o
Imports Db4objects.Db4o.Config
Imports Db4objects.Db4o.CS
Imports Db4objects.Db4o.CS.Config

Namespace Db4oDoc.Code.Configuration.Basics
    Public Class ConfigurationBasics
        Public Shared Sub Main(ByVal args As String())
            EmbeddedConfiguration()
            ServerConfiguration()
            ClientConfiguration()
        End Sub

        Private Shared Sub EmbeddedConfiguration()
            ' #example: Configure embedded object container
            Dim configuration As IEmbeddedConfiguration = Db4oEmbedded.NewConfiguration()
            ' change the configuration...
            Dim container As IObjectContainer = Db4oEmbedded.OpenFile(configuration, "database.db4o")
            ' #end example
            container.Close()
        End Sub

        Private Shared Sub ServerConfiguration()
            ' #example: Configure the db4o-server
            Dim configuration As IServerConfiguration = Db4oClientServer.NewServerConfiguration()
            ' change the configuration...
            Dim server As IObjectServer = Db4oClientServer.OpenServer(configuration, "database.db4o", 1337)
            ' #end example
            server.Close()
        End Sub
        Private Shared Sub ClientConfiguration()
            ' #example: Configure a client object container
            Dim configuration As IClientConfiguration = Db4oClientServer.NewClientConfiguration()
            ' change the configuration...
            Dim container As IObjectContainer = Db4oClientServer.OpenClient(configuration, "localhost", 1337, "user", "pwd")
            ' #end example
            container.Close()
        End Sub
    End Class

End Namespace