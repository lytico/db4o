using System;
using System.IO;
using Db4objects.Db4o;
using Db4objects.Db4o.Activation;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.TA;

namespace Db4oDoc.Code.TA.Collections
{
    public class CollectionsExample
    {
        private const string DatabaseFileName = "database.db4o";

        public static void Main(string[] args)
        {
            CleanUp();
            using (IObjectContainer container = openDatabaseWithTA())
            {
                Team team = new Team();
                team.Add(new Pilot("John"));
                team.Add(new Pilot("Max"));
                container.Store(team);
            }
            using (IObjectContainer container = openDatabaseWithTA())
            {
                Team team = container.Query<Team>()[0];
                foreach (Pilot pilot in team.Pilots)
                {
                    Console.WriteLine(pilot);
                }
            }
            CleanUp();
        }

        private static IObjectContainer openDatabaseWithTA()
        {
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.Add(new TransparentActivationSupport());
            return Db4oEmbedded.OpenFile(configuration, DatabaseFileName);
        }

        private static void CleanUp()
        {
            File.Delete(DatabaseFileName);
        }
    }


    public abstract class ActivatableBase : IActivatable
    {
        [NonSerialized] private IActivator activator;

        public void Bind(IActivator activator)
        {
            if (this.activator == activator)
            {
                return;
            }
            if (activator != null && null != this.activator)
            {
                throw new InvalidOperationException("Object can only be bound to one activator");
            }
            this.activator = activator;
        }

        public void Activate(ActivationPurpose activationPurpose)
        {
            if (null != activator)
            {
                activator.Activate(activationPurpose);
            }
        }
    }




    public class Pilot : ActivatableBase
    {
        private string name;

        public Pilot(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get
            {
                Activate(ActivationPurpose.Read);
                return name;
            }
            set
            {
                Activate(ActivationPurpose.Write);
                name = value;
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}