using System;
using System.Threading;
using Db4objects.Db4o;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.Messaging;

namespace Db4oDoc.Code.ClientServer.Messaging
{
    public class MessagingExample
    {
        private const string DatabaseFile = "database.db4o";
        private const int PortNumber = 1337;
        private const string UserAndPassword = "sa";

        public static void Main(string[] args)
        {
            SimpleMessagingExample();
        }

        private static void SimpleMessagingExample()
        {
            IObjectServer server = StartUpServer();

            // #example: configure a message receiver for a client
            IClientConfiguration configuration = Db4oClientServer.NewClientConfiguration();
            configuration.Networking.MessageRecipient = new ClientMessageReceiver();
            // #end example

            // #example: Get the message sender and use it
            IMessageSender sender = configuration.MessageSender;
            using (IObjectContainer container = Db4oClientServer.OpenClient(configuration, "localhost",
                                                                            PortNumber, UserAndPassword,
                                                                            UserAndPassword))
            {
                sender.Send(new HelloMessage("Hi Server!"));
                WaitForAWhile();
            }
            // #end example


            server.Close();
        }

        private static void WaitForAWhile()
        {
            Thread.Sleep(2000);
        }

        private static IObjectServer StartUpServer()
        {
            // #example: configure a message receiver for the server
            IServerConfiguration configuration = Db4oClientServer.NewServerConfiguration();
            configuration.Networking.MessageRecipient = new ServerMessageReceiver();
            IObjectServer server = Db4oClientServer.OpenServer(configuration, DatabaseFile, PortNumber);
            // #end example
            server.GrantAccess(UserAndPassword, UserAndPassword);
            return server;
        }

        // #example: The message receiver for the client
        class ClientMessageReceiver : IMessageRecipient
        {
            public void ProcessMessage(IMessageContext context, object message)
            {
                Console.WriteLine("The client received a '{0}' message",message);
            }
        }
        // #end example

        // #example: The message receiver for the server
        class ServerMessageReceiver : IMessageRecipient
        {
            public void ProcessMessage(IMessageContext context, object message)
            {
                Console.WriteLine("The server received a '{0}' message", message);
                // you can respond to the client
                context.Sender.Send(new HelloMessage("Hi Client!"));
            }
        }
        // #end example
    }


    // #example: The message class
    public class HelloMessage
    {
        private readonly string message;

        public HelloMessage(string message)
        {
            this.message = message;
        }

        public override string ToString()
        {
            return message;
        }
    }

    // #end example
}