using System;
using Db4objects.Db4o;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.Messaging;

namespace Db4odoc.Tutorial.F1.Chapter6
{
    /// <summary>
    /// stops the db4o Server started with StartServer.
    /// This is done by opening a client connection
    /// to the server and by sending a StopServer object as
    /// a message. StartServer will react in it's
    /// processMessage method.
    /// </summary>
    public class StopServer : ServerInfo
    {
        /// <summary>
        /// stops a db4o Server started with StartServer.
        /// </summary>
        /// <exception cref="Exception" />
        public static void Main(string[] args)
        {
            IObjectContainer IObjectContainer = null;
            try
            {
                // connect to the server
                IObjectContainer = Db4oClientServer.OpenClient(Db4oClientServer.NewClientConfiguration(),
                    HOST, PORT, USER, PASS);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            
            if (IObjectContainer != null)
            {
                // get the messageSender for the IObjectContainer
                IMessageSender messageSender = IObjectContainer.Ext()
                    .Configure().ClientServer().GetMessageSender();

                // send an instance of a StopServer object
                messageSender.Send(new StopServer());
                
                // close the IObjectContainer
                IObjectContainer.Close();
            }
        }
    }
}
