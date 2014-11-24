using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.IO;

namespace Db4oDoc.Code.AcidProperties
{
    public class DangerousPractises
    {
        public static void DangerousStorage()
        {
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            // #example: Using the non-flushing storage weakens the ACID-properties
            IStorage fileStorage = new FileStorage();
            configuration.File.Storage = new NonFlushingStorage(fileStorage);
            // #end example
            Db4oEmbedded.OpenFile(configuration, "database.db4o");
        }
        public static void DangerousNonRecovering()
        {
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            // #example: Disabling commit-recovery weakens the ACID-properties
            configuration.File.DisableCommitRecovery();
            // #end example
            Db4oEmbedded.OpenFile(configuration, "database.db4o");
        }    
    }
}