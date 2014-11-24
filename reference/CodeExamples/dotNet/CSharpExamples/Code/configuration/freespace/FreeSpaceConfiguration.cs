using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.IO;

namespace Db4oDoc.Code.Configuration.FreeSpace
{
    public class FreeSpaceConfiguration
    {
        public static void DiscardSettings()
        {
            // #example: Discard settings
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            // discard smaller than 256 bytes
            configuration.File.Freespace.DiscardSmallerThan(256);
            // #end example
            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, "database.db4o");
            container.Dispose();

        }
        public static void UseBTreeSystem()
        {
            // #example: Use BTree system
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.File.Freespace.UseBTreeSystem();
            // #end example
            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, "database.db4o");
            container.Dispose();
        }
        public static void UseInMemorySystem()
        {
            // #example: Use the in memory system
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.File.Freespace.UseRamSystem();
            // #end example
            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, "database.db4o");
            container.Dispose();
        }
        public static void FreespaceFiller()
        {
            // #example: Using a freespace filler
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.File.Freespace.FreespaceFiller(new MyFreeSpaceFiller());
            // #end example
            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, "database.db4o");
            container.Dispose();
        }
        
    }

    
    // #example: The freespace filler
    class MyFreeSpaceFiller : IFreespaceFiller {
        public void Fill(BlockAwareBinWindow block) {
            byte[] emptyBytes = new byte[block.Length()];
            block.Write(0,emptyBytes);
        }
    }
    // #end example
}