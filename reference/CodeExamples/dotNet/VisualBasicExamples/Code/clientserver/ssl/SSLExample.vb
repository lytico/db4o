Imports System.IO
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates
Imports Db4objects.Db4o
Imports Db4objects.Db4o.CS
Imports Db4objects.Db4o.CS.Config

Namespace Db4oDoc.Code.ClientServer.SSL
    Public Class SSLExample
        Private Const DatabaseFileName As String = "database.db4o"
        Private Const UserAndPassword As String = "sa"
        Private Const PortNumber As Integer = 1337

        Public Shared Sub Main(ByVal args As String())
            TheSSLExample()
        End Sub


        Private Shared Sub TheSSLExample()
            CleanUp()
            ' #example: Add SSL-support to the server
            Dim configuration As IServerConfiguration = Db4oClientServer.NewServerConfiguration()
            ' For the server you need a certificate. For example using a certificate from a file
            Dim certificate As New X509Certificate2("cert.cer")
            configuration.AddConfigurationItem(New ServerSslSupport(certificate))
            ' #end example

            Using server As IObjectServer = OpenServer(configuration)
                SSLClient()
            End Using
            CleanUp()
        End Sub


        Private Shared Function OpenServer(ByVal configuration As IServerConfiguration) As IObjectServer
            Dim server As IObjectServer = Db4oClientServer.OpenServer(configuration, DatabaseFileName, PortNumber)
            server.GrantAccess(UserAndPassword, UserAndPassword)
            Return server
        End Function

        Private Shared Sub SSLClient()
            ' #example: Add SSL-support to the client
            Dim configuration As IClientConfiguration = Db4oClientServer.NewClientConfiguration()
            configuration.AddConfigurationItem(New ClientSslSupport(AddressOf CheckCertificate))
            ' #end example
            Using container As IObjectContainer = OpenClient(configuration)
                container.Store(New Person())
                Console.Out.WriteLine("Stored person")
            End Using
        End Sub

        ' #example: callback for validating the certificate
        Private Shared Function CheckCertificate(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslpolicyerrors As SslPolicyErrors) As Boolean
            ' here you can check the certificates of the server and accept it if it's ok.
            Return True
        End Function
        ' #end example

        Private Shared Function OpenClient(ByVal configuration As IClientConfiguration) As IObjectContainer
            Return Db4oClientServer.OpenClient(configuration, "localhost", PortNumber, UserAndPassword, UserAndPassword)
        End Function

        Private Shared Sub CleanUp()
            File.Delete(DatabaseFileName)
        End Sub


        Private Class Person
            Private name As String

            Public Sub New()
                name = "Joe"
            End Sub
        End Class
    End Class
End Namespace