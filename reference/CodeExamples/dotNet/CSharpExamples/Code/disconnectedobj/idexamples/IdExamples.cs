using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Linq;

namespace Db4oDoc.Code.DisconnectedObj.IdExamples
{
    public static class IdExamples
    {
        public static void Main(string[] args)
        {
            RunExamples(
                CreateRunner(Db4oInternalIdExample.Create()),
                CreateRunner(Db4oUuidExample.Create()),
                CreateRunner(UuidOnObject.Create()),
                CreateRunner(AutoIncrementExample.Create())
                );
        }

        private static void RunExamples(params IRunnable[] examplesToRun)
        {
            foreach (IRunnable toRun in examplesToRun)
            {
                toRun.Run();
            }
        }

        private static IdExamples<TId> CreateRunner<TId>(IIdExample<TId> toRun)
        {
            return new IdExamples<TId>(toRun);
        }
    }


    public interface IRunnable
    {
        void Run();
    }

    public class IdExamples<TId> : IRunnable
    {
        private const string DatabaseFileName = "database.db4o";
        private readonly IIdExample<TId> toRun;

        public IdExamples(IIdExample<TId> toRun)
        {
            this.toRun = toRun;
        }


        public void Run()
        {
            Console.WriteLine("Running: " + toRun.GetType().Name);
            CleanUp();
            StoreJoe();

            TId id = IDOfJoe();
            Pilot incomingChanges = new Pilot("Joe Junior");

            UpdateJoe(id, incomingChanges);

            AssertWasUpdated();
            ListAllPilots();

            CleanUp();
        }

        private void AssertWasUpdated()
        {
            using(IObjectContainer container = OpenDatabase()){
                IList<Pilot> pilots = container.Query<Pilot>();
                AssertEquals(1, pilots.Count);
                AssertEquals("Joe Junior", pilots[0].Name);
            }
        }

        private static void AssertEquals(object expected, object actual)
        {
            if (!expected.Equals(actual))
            {
                throw new InvalidOperationException("Expected to be " + expected + " but is " + actual);
            }
        }

        private void ListAllPilots()
        {
            using(IObjectContainer container = OpenDatabase())
            {
                IList<Pilot> pilots = container.Query<Pilot>();
                foreach (Pilot pilot in pilots)
                {
                    Console.WriteLine(pilot);
                }
            }
        }


        private void UpdateJoe(TId id, Pilot incomingChanges)
        {
            using(IObjectContainer container = OpenDatabase())
            {
                Pilot joe = (Pilot) toRun.ObjectForID(id, container);
                MergeChanges(joe, incomingChanges);
                container.Store(joe);
            }
        }

        private static void MergeChanges(Pilot toUpdate, Pilot incomingChanges)
        {
            toUpdate.Name = incomingChanges.Name;
        }

        private TId IDOfJoe()
        {
            using (IObjectContainer container = OpenDatabase())
            {
                Pilot joe = QueryByName(container, "Joe");
                TId id = toRun.IdForObject(joe, container);
                return id;
            }
        }


        private static Pilot QueryByName(IObjectContainer container, string name)
        {
            return (from Pilot p in container
                       where  p.Name.Equals(name)
                       select p).First();
        }

        private void StoreJoe()
        {
            using(IObjectContainer container = OpenDatabase())
            {
                Pilot joe = new Pilot("Joe");
                container.Store(joe);
            }
        }


        private static void CleanUp()
        {
            File.Delete(DatabaseFileName);
        }


        private IObjectContainer OpenDatabase()
        {
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            toRun.Configure(configuration);
            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, DatabaseFileName);
            toRun.RegisterEventOnContainer(container);
            return container;
        }
    }

    public static class ObjectIdPair
    {
        public static ObjectIdPair<TId, TObject> Create<TId, TObject>(TId id, TObject instance)
        {
            return new ObjectIdPair<TId, TObject>(id, instance);
        }
    }

    public struct ObjectIdPair<TId, TObject>
    {
        private readonly TId id;
        private readonly TObject instance;

        public ObjectIdPair(TId id, TObject instance)
        {
            this.id = id;
            this.instance = instance;
        }

        public TId ID
        {
            get { return id; }
        }

        public TObject Instance
        {
            get { return instance; }
        }
    }
}