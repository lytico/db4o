using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Ids;

namespace Db4oDoc.Code.Configuration.IdSystem
{
    public class IdSystemConfigurationExamples
    {

        private static void StackedBTreeIdSystem()
        {
            // #example: Use stacked B-trees for storing the ids
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.IdSystem.UseStackedBTreeSystem();
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, "database.db4o");
            container.Close();
        }
        private static void BTreeIdSystem()
        {
            // #example: Use a single B-tree for storing the ids.
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.IdSystem.UseSingleBTreeSystem();
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, "database.db4o");
            container.Close();
        }
        private static void UseMemoryIDSystem()
        {
            // #example: Use a in-memory id system
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.IdSystem.UseInMemorySystem();
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, "database.db4o");
            container.Close();
        }

        private static void PointerIdSystem()
        {
            // #example: Use pointers for storing the ids
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.IdSystem.UsePointerBasedSystem();
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, "database.db4o");
            container.Close();
        }


        private static void CustomIdSystem()
        {
            // #example: use a custom id system
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.IdSystem.UseCustomSystem(new CustomIdSystemFactory());
            // #end example

            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, "database.db4o");
            container.Close();
        }
    }

    
    class CustomIdSystemFactory : IIdSystemFactory {
        public IIdSystem NewInstance(LocalObjectContainer localObjectContainer) {
            return new InMemoryIdSystem(localObjectContainer);
        }
    }

}