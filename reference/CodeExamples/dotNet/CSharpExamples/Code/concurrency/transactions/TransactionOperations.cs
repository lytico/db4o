using System;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;

namespace Db4oDoc.Code.Concurrency.Transactions
{
    public class TransactionOperations
    {
        public static void Main(string[] args)
        {
            new TransactionOperations().Main();
        }

        public void Main()
        {
            using (var database = new DatabaseSupport(Db4oEmbedded.OpenFile("database.db4o")))
            {
                storeInitialObjects(database);

                // Schedule back-ground tasks
                Action<DatabaseSupport> toRun = UpdateAllJoes;
                var waitHandle = toRun.BeginInvoke(database, null, null);

                // While doing other work
                ListAllPeople(database);

                // Wait for the task to finish
                toRun.EndInvoke(waitHandle);
            }
        }

        // #example: Use transaction for reading objects
        private void ListAllPeople(DatabaseSupport dbSupport)
        {
            dbSupport.InTransaction(
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

        // #example: Use transaction to update objects
        private void UpdateAllJoes(DatabaseSupport dbSupport)
        {
            dbSupport.InTransaction(
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

        private void storeInitialObjects(DatabaseSupport dbSupport)
        {
            dbSupport.InTransaction(
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