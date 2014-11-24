using Db4objects.Db4o;
using Db4objects.Db4o.Events;

namespace Db4oDoc.Code.Practises.Deletion
{
    public class DeletionStrategies
    {
        public static void Main(string[] args)
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile("database.db4o"))
            {
                InstallDeletionFlagSupport(container);
            }
        }

        private static void InstallDeletionFlagSupport(IObjectContainer container)
        {
            // #example: Deletion-Flag
            IEventRegistry events = EventRegistryFactory.ForObjectContainer(container);
            events.Deleting +=
                (sender, args) =>
                    {
                        object obj = args.Object;
                        // if the object has a deletion-flag:
                        // set the flag instead of deleting the object
                        if (obj is Deletable)
                        {
                            ((Deletable) obj).Delete();
                            args.ObjectContainer().Store(obj);
                            args.Cancel();
                        }
                    };
            // #end example
        }
    }

    internal abstract class Deletable
    {
        private bool deleted = false;

        public void Delete()
        {
            deleted = true;
        }

        public bool Deleted
        {
            get { return deleted; }
        }
    }

    internal class Person : Deletable
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