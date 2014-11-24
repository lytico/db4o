using System;
using Db4objects.Db4o;
using Db4objects.Db4o.CS;

namespace Db4oDoc.Code.Semaphores
{
    public class SemaphoreExamples
    {
        private const int Port = 1337;
        private const string UserAndPassword = "sa";

        public static void Main(string[] args)
        {
            using (IObjectServer server = Db4oClientServer.OpenServer("database.db4o", Port))
            {
                server.GrantAccess(UserAndPassword, UserAndPassword);
                GrabSemaphore();
                TryGrabSemaphore();
            }
        }

        private static void TryGrabSemaphore()
        {
            using (IObjectContainer container = OpenClient())
            {
                bool hasLock = container.Ext().SetSemaphore("LockName", 1000);
                if (hasLock)
                {
                    Console.WriteLine("Could get lock");
                }
                else
                {
                    Console.WriteLine("Couldn't get lock");
                }
                GrabSemaphore();
            }
        }

        private static void GrabSemaphore()
        {
            IObjectContainer container = OpenClient();

            // #example: Grab a semaphore
            // Grab the lock. Specify the name and a timeout in milliseconds
            bool hasLock = container.Ext().SetSemaphore("LockName", 1000);
            try
            {
                // you need to check the lock
                if (hasLock)
                {
                    Console.WriteLine("Could get lock");
                }
                else
                {
                    Console.WriteLine("Couldn't get lock");
                }
            }
            finally
            {
                // release the lock
                container.Ext().ReleaseSemaphore("LockName");
            }
            // #end example
        }

        private static IObjectContainer OpenClient()
        {
            return Db4oClientServer.OpenClient("localhost", Port, UserAndPassword, UserAndPassword);
        }
    }
}