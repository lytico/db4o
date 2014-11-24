using System;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;

namespace Db4oDoc.Code.Concurrency.Locking
{
    public class LockingOperations
    {
        private readonly object dataLock = new object();

        public static void Main(string[] args)
        {
            new LockingOperations().Main();
        }

        public void Main()
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile("database.db4o"))
            {
                StoreInitialObjects(container);

                // #example: Schedule back-ground tasks
                // Schedule back-ground tasks
                Action<IObjectContainer> toRun =  UpdateSomePeople;
                var waitHandle = toRun.BeginInvoke(container, null, null);

                // While doing other work
                ListAllPeople(container);
                // #end example

                // Wait for the tasks to finish
                toRun.EndInvoke(waitHandle);
            }
        }

        // #example: Grab the lock to show the data
        private void ListAllPeople(IObjectContainer container)
        {
            lock (dataLock)
            {
                foreach (Person person in from Person p in container select p)
                {
                    Console.WriteLine(person.Name);
                }
            }
        }
        // #end example

        // #example: Grab the lock protecting the data
        private void UpdateSomePeople(IObjectContainer container)
        {
            lock (dataLock)
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
        }
        // #end example:

        private void StoreInitialObjects(IObjectContainer container)
        {
            lock (dataLock)
            {
                container.Store(new Person("Joe"));
                container.Store(new Person("Jan"));
                container.Store(new Person("Joanna"));
                container.Store(new Person("Phil"));
            }
        }
    }
}