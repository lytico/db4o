using System;
using System.Threading;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;

namespace Db4oDoc.Code.Concurrency.Locking
{
    public class ReadWriteLockingOperations
    {
        private readonly ReaderWriterLockSlim dataLock = new ReaderWriterLockSlim();

        public static void Main(string[] args)
        {
            new ReadWriteLockingOperations().Main();
        }

        public void Main()
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile("database.db4o"))
            {
                StoreInitialObjects(container);

                // Schedule back-ground tasks
                Action<IObjectContainer> toRun = UpdateSomePeople;
                var waitHandle = toRun.BeginInvoke(container, null, null);

                // While doing other work
                ListAllPeople(container);

                // Wait for the tasks to finish
                toRun.EndInvoke(waitHandle);
            }
        }

        // #example: Grab the read-lock to show the data
        private void ListAllPeople(IObjectContainer container)
        {
            dataLock.EnterReadLock();
            try
            {
                foreach (Person person in from Person p in container select p)
                {
                    Console.WriteLine(person.Name);
                }
            }
            finally
            {
                dataLock.ExitReadLock();
            }
        }
        // #end example

        // #example: Grab the write-lock to change the data
        private void UpdateSomePeople(IObjectContainer container)
        {
            dataLock.EnterWriteLock();
            try
            {
                var people = from Person p in container
                             where p.Name.Equals("Joe")
                             select p;
                foreach (Person joe in people)
                {
                    joe.Name = "New Joe";
                    container.Store(joe);
                }
            }
            finally
            {
                dataLock.ExitWriteLock();
            }
        }
        // #end example:

        private void StoreInitialObjects(IObjectContainer container)
        {
            dataLock.EnterWriteLock();
            try
            {
                container.Store(new Person("Joe"));
                container.Store(new Person("Jan"));
                container.Store(new Person("Joanna"));
                container.Store(new Person("Phil"));
            }
            finally
            {
                dataLock.ExitWriteLock();
            }
        }
    }
}