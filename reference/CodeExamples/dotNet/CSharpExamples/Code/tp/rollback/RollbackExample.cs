using System;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.TA;

namespace Db4oDoc.Code.Tp.Rollback
{
    public class RollbackExample
    {
        public static void Main(string[] args)
        {
            // #example: Configure rollback strategy
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common
                .Add(new TransparentPersistenceSupport(new DeactivatingRollbackStrategy()));
            // #end example
            using (IObjectContainer container = Db4oEmbedded.OpenFile(configuration, "database.db4o"))
            {
                StorePilot(container);

                // #example: Rollback with rollback strategy
                Pilot pilot = container.Query<Pilot>()[0];
                pilot.Name = "NewName";
                // Rollback
                container.Rollback();
                // Now the pilot has the old name again
                Console.Out.WriteLine(pilot.Name);
                // #end example
            }
        }

        private static void StorePilot(IObjectContainer container)
        {
            container.Store(new Pilot("John"));
            container.Commit();
        }
    }

    [PersistanceAware]
    class Pilot
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