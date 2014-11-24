using Db4objects.Db4o;

namespace Db4oDoc.Code.Transactions
{
    public class Transactions
    {
        public static void Main(string[] args)
        {
            using (IObjectContainer container = Db4oEmbedded.OpenFile("database.db4o"))
            {
                CommitChanges(container);
                RollbackChanges(container);
                RefreshAfterRollback(container);
            }
        }

        private static void RollbackChanges(IObjectContainer container)
        {
            // #example: Commit changes
            container.Store(new Pilot("John"));
            container.Store(new Pilot("Joanna"));

            container.Commit();
            // #end example
        }

        private static void CommitChanges(IObjectContainer container)
        {
            // #example: Rollback changes
            container.Store(new Pilot("John"));
            container.Store(new Pilot("Joanna"));

            container.Rollback();
            // #end example
        }

        private static void RefreshAfterRollback(IObjectContainer container)
        {
            // #example: Refresh objects after rollback
            Pilot pilot = container.Query<Pilot>()[0];
            pilot.Name = "New Name";
            container.Store(pilot);
            container.Rollback();

            // use refresh to return the in memory objects back
            // to the state in the database.
            container.Ext().Refresh(pilot, int.MaxValue);
            // #end example
        }
    }


    internal class Pilot
    {
        private string name;

        public Pilot(string name)
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