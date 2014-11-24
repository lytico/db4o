Imports Db4objects.Db4o
Imports Db4objects.Db4o.CS

Namespace Db4oDoc.Code.ClientServer.Pooling
    Friend Class ConnectionPoolExamples
        Private Const Port As Integer = 1337
        Public Const UserAndPassword As String = "sa"


        Public Shared Sub Main(ByVal args As String())
            Dim server As IObjectServer = StartServer()

            Dim connectionPool As New ConnectionPool(AddressOf CreateClientConnection)

            UseThePool(connectionPool)

            server.Close()
        End Sub

        Private Shared Function CreateClientConnection() As IObjectContainer
            ' #example: Open clients for the pool
            Dim client As IObjectContainer = Db4oClientServer.OpenClient("localhost", _
                         Port, UserAndPassword, UserAndPassword)
            ' #end example
            Return client
        End Function

        Private Shared Sub UseThePool(ByVal connectionPool As ConnectionPool)
            Dim session As IObjectContainer = connectionPool.Acquire()
            Try
                session.Store(New Person("Joe"))
            Finally
                connectionPool.CloseAndRelease(session)
            End Try
        End Sub

        Private Shared Function StartServer() As IObjectServer
            Dim server As IObjectServer = Db4oClientServer.OpenServer("In:Memory", Port)
            server.GrantAccess(UserAndPassword, UserAndPassword)
            Return server
        End Function

        Private Class Person
            Private m_name As String

            Public Sub New(ByVal name As String)
                Me.m_name = name
            End Sub

            Public Property Name() As String
                Get
                    Return m_name
                End Get
                Set(ByVal value As String)
                    m_name = value
                End Set
            End Property
        End Class
    End Class
End Namespace