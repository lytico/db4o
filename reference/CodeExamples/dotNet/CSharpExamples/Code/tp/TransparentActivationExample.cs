using System;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.TA;

namespace Db4oDoc.Code.Tp
{
    public class TransparentActivationExample
    {
        public static void Main(string[] args)
        {
            // #example: Add transparent persistence support
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.Common.Add(new TransparentPersistenceSupport());
            // #end example
            using(IObjectContainer container = Db4oEmbedded.OpenFile(configuration, "database.db4o"))
            {
                container.Store(new Pilot("Joe"));
            }
            
            // #example: Verify that pilot support transparent activation
            bool supportsTransparentPersistence = typeof (Pilot) is IActivatable;
            if(supportsTransparentPersistence)
            {
                Console.Out.WriteLine("The Pilot-class supports transparent persistence."
                    +" The enhancement worked");
            } else
            {
                Console.Out.WriteLine("Oups, not transperent persistence support."
                    +" Something went wrong with the enhancement");
            }
            // #end example
        }   
    }

    [PersistanceAware]
    // #example: Our domain model
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
    // #end example
}