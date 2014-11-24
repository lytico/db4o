using System;
using Db4objects.Db4o;
using Db4objects.Db4o.Linq;

namespace Db4oDoc.Code.Concurrency.UnitsOfWork
{
    public class UnitsOfWork
    {
        public static void Main(string[] args)
        {
            new UnitsOfWork().Main();
        }

        public void Main()
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile("database.db4o"))
            {
                StoreInitialObjects(container);

                // #example: Schedule back-ground tasks
                // Schedule back-ground tasks
                Action<IObjectContainer> toRun = UpdateSomePeople;
                var waitHandle = toRun.BeginInvoke(container, null, null);

                // While doing other work
                ListAllPeople(container);
                // #end example

                // Wait for the tasks to finish
                toRun.EndInvoke(waitHandle);
            }
        }

        // #example: An object container for this unit of work
        private void ListAllPeople(IObjectContainer rootContainer)
        {
            using (IObjectContainer container = rootContainer.Ext().OpenSession())
            {
                foreach (Person person in from Person p in container select p)
                {
                    Console.WriteLine(person.Name);
                }
            }
        }
        // #end example

        // #example: An object container for the background task
        private void UpdateSomePeople(IObjectContainer rootContainer)
        {
            using (IObjectContainer container = rootContainer.Ext().OpenSession())
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

        private void StoreInitialObjects(IObjectContainer rootContainer)
        {
            using (IObjectContainer container = rootContainer.Ext().OpenSession())
            {
                container.Store(new Person("Joe"));
                container.Store(new Person("Jan"));
                container.Store(new Person("Joanna"));
                container.Store(new Person("Phil"));
            }
        }
    }


    internal class Person
    {
        private string name;

        public Person(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}