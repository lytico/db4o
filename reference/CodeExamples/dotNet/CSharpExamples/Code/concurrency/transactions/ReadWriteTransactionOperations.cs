using System;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;

namespace Db4oDoc.Code.Concurrency.Transactions
{
    public class ReadWriteTransactionOperations
    {
        public static void Main(string[] args)
        {
            new ReadWriteTransactionOperations().Main();
        }

        public void Main()
        {
            using (var database = new DatabaseSupportWithReadWriteLock(Db4oEmbedded.OpenFile("database.db4o")))
            {
                storeInitialObjects(database);

                // Schedule back-ground tasks
                Action<DatabaseSupportWithReadWriteLock> toRun = UpdateAllJoes;
                var waitHandle = toRun.BeginInvoke(database, null, null);

                // While doing other work
                ListAllPeople(database);

                // Wait for the task to finish
                toRun.EndInvoke(waitHandle);
            }
        }

        // #example: Use a read transaction for reading objects
        private void ListAllPeople(DatabaseSupportWithReadWriteLock dbSupport)
        {
            dbSupport.InReadTransaction(
                container =>
                    {
                        var result = from Person p in container select p;
                        foreach (Person person in result)
                        {
                            Console.WriteLine(person.Name);
                        }
                    });
        }

        // #end example

        // #example: Use a write transaction to update objects
        private void UpdateAllJoes(DatabaseSupportWithReadWriteLock dbSupport)
        {
            dbSupport.InWriteTransaction(
                container =>
                    {
                        var allJoes = from Person p in container
                                      where p.Name == "Joe"
                                      select p;
                        foreach (Person joe in allJoes)
                        {
                            joe.Name = "New Joe";
                            container.Store(joe);
                        }
                    });
        }

        // #end example

        private void storeInitialObjects(DatabaseSupportWithReadWriteLock dbSupport)
        {
            dbSupport.InWriteTransaction(
                container =>
                    {
                        container.Store(new Person("Joe"));
                        container.Store(new Person("Jan"));
                        container.Store(new Person("Joanna"));
                        container.Store(new Person("Phil"));
                    });
        }
    }
}