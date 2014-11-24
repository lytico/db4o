Imports System.Threading
Imports Db4objects.Db4o
Imports Db4objects.Db4o.CS
Imports Db4objects.Db4o.CS.Config
Imports Db4objects.Db4o.Messaging

Namespace Db4oDoc.Code.ClientServer.Messaging
    Public Class MessagingExample
        Private Const DatabaseFile As String = "database.db4o"
        Private Const PortNumber As Integer = 1337
        Private Const UserAndPassword As String = "sa"

        Public Shared Sub Main(ByVal args As String())
            SimpleMessagingExample()
        End Sub

        Private Shared Sub SimpleMessagingExample()
            Dim server As IObjectServer = StartUpServer()

            ' #example: configure a message receiver for a client
            Dim configuration As IClientConfiguration = Db4oClientServer.NewClientConfiguration()
            configuration.Networking.MessageRecipient = New ClientMessageReceiver()
            ' #end example

            ' #example: Get the message sender and use it
            Dim sender As IMessageSender = configuration.MessageSender
            Using container As IObjectContainer = Db4oClientServer.OpenClient(configuration, _
                        "localhost", PortNumber, UserAndPassword, UserAndPassword)
                sender.Send(New HelloMessage("Hi Server!"))
                WaitForAWhile()
            End Using
            ' #end example


            server.Close()
        End Sub

        Private Shared Sub WaitForAWhile()
            Thread.Sleep(2000)
        End Sub

        Private Shared Function StartUpServer() As IObjectServer
            ' #example: configure a message receiver for the server
            Dim configuration As IServerConfiguration = Db4oClientServer.NewServerConfiguration()
            configuration.Networking.MessageRecipient = New ServerMessageReceiver()
            Dim server As IObjectServer = Db4oClientServer.OpenServer(configuration, DatabaseFile, PortNumber)
            ' #end example
            server.GrantAccess(UserAndPassword, UserAndPassword)
            Return server
        End Function

        ' #example: The message receiver for the client
        Private Class ClientMessageReceiver
            Implements IMessageRecipient
            Public Sub ProcessMessage(ByVal context As IMessageContext, ByVal message As Object) _
                Implements IMessageRecipient.ProcessMessage
                Console.WriteLine("The client received a '{0}' message", message)
            End Sub
        End Class
        ' #end example

        ' #example: The message receiver for the server
        Private Class ServerMessageReceiver
            Implements IMessageRecipient
            Public Sub ProcessMessage(ByVal context As IMessageContext, ByVal message As Object) _
                Implements IMessageRecipient.ProcessMessage
                Console.WriteLine("The server received a '{0}' message", message)
                ' you can respond to the client
                context.Sender.Send(New HelloMessage("Hi Client!"))
            End Sub
        End Class
        ' #end example
    End Class


    ' #example: The message class
    Public Class HelloMessage
        Private ReadOnly message As String

        Public Sub New(ByVal message As String)
            Me.message = message
        End Sub

        Public Overrides Function ToString() As String
            Return message
        End Function
    End Class

    ' #end example
End Namespace