using System;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;

namespace Db4oDoc.Code.Configuration.File
{
    public class FileConfiguration
    {
        public static void AsynchronousSync()
        {
            // #example: Allow asynchronous synchronisation of the file-flushes
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.File.AsynchronousSync = true;
            // #end example
            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, "database.db4o");
            container.Close();

        }

        public static void ChangeBlobPath()
        {
            // #example: Configure the blob-path
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.File.BlobPath = "myBlobDirectory";
            // #end example
            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, "database.db4o");
            container.Close();

        }

        public static void IncreaseBlockSize()
        {
            // #example: Increase block size for larger databases
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.File.BlockSize = 8;
            // #end example
            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, "database.db4o");
            container.Close();
        }

        public static void GlobalUUID()
        {
            // #example: Enable db4o uuids globally
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.File.GenerateUUIDs = ConfigScope.Globally;
            // #end example
            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, "database.db4o");
            container.Close();
        }

        public static void IndividualUUID()
        {
            // #example: Enable db4o uuids for certain classes
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.File.GenerateUUIDs = ConfigScope.Individually;
            configuration.Common.ObjectClass(typeof (SpecialClass)).GenerateUUIDs(true);
            // #end example
            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, "database.db4o");

            SpecialClass withUUID = new SpecialClass();
            container.Store(withUUID);
            NormalClass withoutUUID = new NormalClass();
            container.Store(withoutUUID);

            AssertNotNull(container.Ext().GetObjectInfo(withUUID).GetUUID());
            AssertNull(container.Ext().GetObjectInfo(withoutUUID).GetUUID());

            container.Close();
        }


        public static void CommitTimestamps()
        {
            // #example: Enable db4o commit timestamps
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.File.GenerateCommitTimestamps = true;
            // #end example
            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, "database.db4o");
            container.Close();
        }


        public static void ReserveSpace()
        {
            // #example: Configure the growth size
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.File.DatabaseGrowthSize = 4096;
            // #end example
            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, "database.db4o");
            container.Close();

        }
        public static void DisableCommitRecovers()
        {
            // #example: Disable commit recovery
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.File.DisableCommitRecovery();
            // #end example
            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, "database.db4o");
            container.Close();

        }

        public static void DoNotLockDatabaseFile()
        {
            // #example: Disable the database file lock
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.File.LockDatabaseFile = false;
            // #end example
            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, "database.db4o");
            container.Close();
        }

        public static void ReadOnlyMode()
        {
            // #example: Set read only mode
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.File.ReadOnly = true;
            // #end example
            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, "database.db4o");
            container.Close();

        }
        public static void RecoveryMode()
        {
            // #example: Enable recovery mode to open a corrupted database
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.File.RecoveryMode = true;
            // #end example
            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, "database.db4o");
            container.Close();
        }
        public static void ReserveStorageSpace()
        {
            // #example: Reserve storage space
            IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
            configuration.File.ReserveStorageSpace = 1024 * 1024;
            // #end example
            IObjectContainer container = Db4oEmbedded.OpenFile(configuration, "database.db4o");
            container.Close();
        }

        private static void AssertNotNull(object uuid)
        {
            if (null == uuid)
            {
                throw new Exception("Expected not null");
            }
        }
        private static void AssertNull(object uuid)
        {
            if (null != uuid)
            {
                throw new Exception("Expected null");
            }
        }
        private static void AssertTrue(bool value)
        {
            if (!value)
            {
                throw new Exception("Expected true");
            }
        }
    }

    class SpecialClass{
        
    }
    class NormalClass{

    }
}