using System;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Defragment;
using Db4objects.Db4o.IO;

namespace Db4oDoc.Code.Deframentation
{
    public class DefragmentationConfigurationExamples
    {
        public void ConfigureFile()
        {
            // #example: Configure the file
            DefragmentConfig config = new DefragmentConfig("database.db4o");

            Defragment.Defrag(config);
            // #end example
        }

        public void ConfigureBackupFile()
        {
            // #example: Configure the file and backup file
            DefragmentConfig config = new DefragmentConfig("database.db4o", "database.db4o.back");

            Defragment.Defrag(config);
            // #end example
        }

        public void SetMappingImplementation()
        {
            // #example: Choose a id mapping system
            IIdMapping mapping = new InMemoryIdMapping();
            DefragmentConfig config = new DefragmentConfig("database.db4o", "database.db4o.back", mapping);

            Defragment.Defrag(config);
            // #end example
        }

        public void SetDb4OConfiguration()
        {
            // #example: Use the database-configuration
            DefragmentConfig config = new DefragmentConfig("database.db4o");
            // It's best to use the very same configuration you use for the regular database
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            config.Db4oConfig(configuration);

            Defragment.Defrag(config);
            // #end example
        }

        public void SetCommitFrequency()
        {
            // #example: Set the commit frequency
            DefragmentConfig config = new DefragmentConfig("database.db4o");
            config.ObjectCommitFrequency(10000);

            Defragment.Defrag(config);
            // #end example
        }

        public void ChangeBackupStorage()
        {
            // #example: Use a separate storage for the backup
            DefragmentConfig config = new DefragmentConfig("database.db4o");
            config.BackupStorage(new FileStorage());

            Defragment.Defrag(config);
            // #end example
        }

        public void DeleteBackup()
        {
            // #example: Delete the backup after the defragmentation process
            DefragmentConfig config = new DefragmentConfig("database.db4o");
            config.ForceBackupDelete(true);

            Defragment.Defrag(config);
            // #end example
        }

        public void DisableReadOnlyForBackup()
        {
            // #example: Disable readonly on backup
            DefragmentConfig config = new DefragmentConfig("database.db4o");
            config.ReadOnly(false);

            Defragment.Defrag(config);
            // #end example
        }

        public void UseAClassFilter()
        {
            // #example: Use class filter
            DefragmentConfig config = new DefragmentConfig("database.db4o");
            config.StoredClassFilter(new AvailableTypeFilter());

            Defragment.Defrag(config);
            // #end example
        }

        public void UpgradeDb4OFile()
        {
            // #example: Upgrade database version
            DefragmentConfig config = new DefragmentConfig("database.db4o");
            config.UpgradeFile(Environment.GetEnvironmentVariable("TEMP"));

            Defragment.Defrag(config);
            // #end example
        }
    }
}