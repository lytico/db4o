/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.IO;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Defragment;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Internal.Classindex;
using Db4objects.Db4o.Internal.Mapping;

namespace Db4objects.Db4o.Defragment
{
	/// <summary>defragments database files.</summary>
	/// <remarks>
	/// defragments database files.
	/// <br/>
	/// <br/>
	/// db4o structures storage inside database files as free and occupied
	/// slots, very much like a file system - and just like a file system it
	/// can be fragmented.
	/// <br/>
	/// <br/>
	/// The simplest way to defragment a database file:
	/// <br/>
	/// <br/>
	/// <code>Defragment.Defrag("sample.yap");
	/// </code>
	/// <br/>
	/// <br/>
	/// This will move the file to "sample.yap.backup", then create a
	/// defragmented version of this file in the original position, using a
	/// temporary file "sample.yap.mapping". If the backup file already
	/// exists, this will throw an exception and no action will be taken.
	/// <br/>
	/// <br/>
	/// For more detailed configuration of the defragmentation process,
	/// provide a DefragmentConfig instance:
	/// <br/>
	/// <br/>
	/// <code>
	/// DefragmentConfig config=new DefragmentConfig("sample.yap","sample.bap",new BTreeIDMapping("sample.map"));
	/// <br/>
	/// config.ForceBackupDelete(true);
	/// <br/>
	/// config.StoredClassFilter(new AvailableClassFilter());
	/// <br/>
	/// config.Db4oConfig(db4oConfig);
	/// <br/>
	/// Defragment.Defrag(config);
	/// </code>
	/// <br/>
	/// <br/>
	/// This will move the file to "sample.bap", then create a defragmented
	/// version of this file in the original position, using a temporary
	/// file "sample.map" for BTree mapping. If the backup file already
	/// exists, it will be deleted. The defragmentation process will skip
	/// all classes that have instances stored within the yap file, but that
	/// are not available on the class path (through the current
	/// classloader). Custom db4o configuration options are read from the
	/// <see cref="IConfiguration">IConfiguration</see>
	/// passed as db4oConfig.
	/// <strong>Note:</strong>
	/// For some specific, non-default configuration settings like UUID
	/// generation, etc., you
	/// <strong>must</strong>
	/// pass an appropriate db4o configuration, just like you'd use it
	/// within your application for normal database operation.
	/// </remarks>
	public class Defragment
	{
		/// <summary>
		/// Renames the file at the given original path to a backup file and then
		/// builds a defragmented version of the file in the original place.
		/// </summary>
		/// <remarks>
		/// Renames the file at the given original path to a backup file and then
		/// builds a defragmented version of the file in the original place.
		/// </remarks>
		/// <param name="origPath">The path to the file to be defragmented.</param>
		/// <exception cref="System.IO.IOException">if the original file cannot be moved to the backup location
		/// 	</exception>
		public static void Defrag(string origPath)
		{
			Defrag(new DefragmentConfig(origPath), new Defragment.NullListener());
		}

		/// <summary>
		/// Renames the file at the given original path to the given backup file and
		/// then builds a defragmented version of the file in the original place.
		/// </summary>
		/// <remarks>
		/// Renames the file at the given original path to the given backup file and
		/// then builds a defragmented version of the file in the original place.
		/// </remarks>
		/// <param name="origPath">The path to the file to be defragmented.</param>
		/// <param name="backupPath">The path to the backup file to be created.</param>
		/// <exception cref="System.IO.IOException">if the original file cannot be moved to the backup location
		/// 	</exception>
		public static void Defrag(string origPath, string backupPath)
		{
			Defrag(new DefragmentConfig(origPath, backupPath), new Defragment.NullListener());
		}

		/// <summary>
		/// Renames the file at the configured original path to the configured backup
		/// path and then builds a defragmented version of the file in the original
		/// place.
		/// </summary>
		/// <remarks>
		/// Renames the file at the configured original path to the configured backup
		/// path and then builds a defragmented version of the file in the original
		/// place.
		/// </remarks>
		/// <param name="config">The configuration for this defragmentation run.</param>
		/// <exception cref="System.IO.IOException">if the original file cannot be moved to the backup location
		/// 	</exception>
		public static void Defrag(DefragmentConfig config)
		{
			Defrag(config, new Defragment.NullListener());
		}

		/// <summary>
		/// Renames the file at the configured original path to the configured backup
		/// path and then builds a defragmented version of the file in the original
		/// place.
		/// </summary>
		/// <remarks>
		/// Renames the file at the configured original path to the configured backup
		/// path and then builds a defragmented version of the file in the original
		/// place.
		/// </remarks>
		/// <param name="config">The configuration for this defragmentation run.</param>
		/// <param name="listener">
		/// A listener for status notifications during the defragmentation
		/// process.
		/// </param>
		/// <exception cref="System.IO.IOException">if the original file cannot be moved to the backup location
		/// 	</exception>
		public static void Defrag(DefragmentConfig config, IDefragmentListener listener)
		{
			IStorage storage = config.Db4oConfig().Storage;
			EnsureFileExists(storage, config.OrigPath());
			IStorage backupStorage = config.BackupStorage();
			if (backupStorage.Exists(config.BackupPath()))
			{
				if (!config.ForceBackupDelete())
				{
					throw new IOException("Could not use '" + config.BackupPath() + "' as backup path - file exists."
						);
				}
			}
			// Always delete, because !exists can indicate length == 0
			backupStorage.Delete(config.BackupPath());
			MoveToBackup(config);
			if (config.FileNeedsUpgrade())
			{
				UpgradeFile(config);
			}
			DefragmentServicesImpl services = new DefragmentServicesImpl(config, listener);
			try
			{
				FirstPass(services, config);
				services.CommitIds();
				SecondPass(services, config);
				services.CommitIds();
				DefragUnindexed(services);
				services.CommitIds();
				services.DefragIdToTimestampBtree();
				services.ReplaceClassMetadataRepository();
			}
			catch (CorruptionException exc)
			{
				Sharpen.Runtime.PrintStackTrace(exc);
			}
			finally
			{
				services.Close();
			}
		}

		/// <exception cref="System.IO.IOException"></exception>
		private static void MoveToBackup(DefragmentConfig config)
		{
			IStorage origStorage = config.Db4oConfig().Storage;
			if (origStorage == config.BackupStorage())
			{
				origStorage.Rename(config.OrigPath(), config.BackupPath());
				return;
			}
			CopyBin(origStorage, config.BackupStorage(), config.OrigPath(), config.BackupPath
				());
			origStorage.Delete(config.OrigPath());
		}

		/// <exception cref="System.IO.IOException"></exception>
		private static void CopyBin(IStorage sourceStorage, IStorage targetStorage, string
			 sourcePath, string targetPath)
		{
			IBin origBin = sourceStorage.Open(new BinConfiguration(sourcePath, true, 0, true)
				);
			try
			{
				IBin backupBin = targetStorage.Open(new BinConfiguration(targetPath, true, origBin
					.Length(), false));
				try
				{
					byte[] buffer = new byte[4096];
					int bytesRead = -1;
					int pos = 0;
					while ((bytesRead = origBin.Read(pos, buffer, buffer.Length)) >= 0)
					{
						backupBin.Write(pos, buffer, bytesRead);
						pos += bytesRead;
					}
				}
				finally
				{
					SyncAndClose(backupBin);
				}
			}
			finally
			{
				SyncAndClose(origBin);
			}
		}

		private static void SyncAndClose(IBin bin)
		{
			try
			{
				bin.Sync();
			}
			finally
			{
				bin.Close();
			}
		}

		/// <exception cref="System.IO.IOException"></exception>
		private static void EnsureFileExists(IStorage storage, string origPath)
		{
			if (!storage.Exists(origPath))
			{
				throw new IOException("Source database file '" + origPath + "' does not exist.");
			}
		}

		/// <exception cref="System.IO.IOException"></exception>
		private static void UpgradeFile(DefragmentConfig config)
		{
			CopyBin(config.BackupStorage(), config.BackupStorage(), config.BackupPath(), config
				.TempPath());
			IConfiguration db4oConfig = (IConfiguration)((Config4Impl)config.Db4oConfig()).DeepClone
				(null);
			db4oConfig.Storage = config.BackupStorage();
			db4oConfig.AllowVersionUpdates(true);
			IObjectContainer db = Db4oFactory.OpenFile(db4oConfig, config.TempPath());
			db.Close();
		}

		private static void DefragUnindexed(DefragmentServicesImpl services)
		{
			IdSource unindexedIDs = services.UnindexedIDs();
			while (unindexedIDs.HasMoreIds())
			{
				int origID = unindexedIDs.NextId();
				DefragmentContextImpl.ProcessCopy(services, origID, new _ISlotCopyHandler_208());
			}
		}

		private sealed class _ISlotCopyHandler_208 : ISlotCopyHandler
		{
			public _ISlotCopyHandler_208()
			{
			}

			public void ProcessCopy(DefragmentContextImpl context)
			{
				ClassMetadata.DefragObject(context);
			}
		}

		/// <exception cref="Db4objects.Db4o.CorruptionException"></exception>
		/// <exception cref="System.IO.IOException"></exception>
		private static void FirstPass(DefragmentServicesImpl context, DefragmentConfig config
			)
		{
			// System.out.println("FIRST");
			Pass(context, config, new FirstPassCommand());
		}

		/// <exception cref="Db4objects.Db4o.CorruptionException"></exception>
		/// <exception cref="System.IO.IOException"></exception>
		private static void SecondPass(DefragmentServicesImpl context, DefragmentConfig config
			)
		{
			// System.out.println("SECOND");
			Pass(context, config, new SecondPassCommand(config.ObjectCommitFrequency()));
		}

		/// <exception cref="Db4objects.Db4o.CorruptionException"></exception>
		/// <exception cref="System.IO.IOException"></exception>
		private static void Pass(DefragmentServicesImpl context, DefragmentConfig config, 
			IPassCommand command)
		{
			command.ProcessClassCollection(context);
			IStoredClass[] classes = context.StoredClasses(DefragmentServicesImpl.Sourcedb);
			for (int classIdx = 0; classIdx < classes.Length; classIdx++)
			{
				ClassMetadata classMetadata = (ClassMetadata)classes[classIdx];
				if (!config.StoredClassFilter().Accept(classMetadata))
				{
					continue;
				}
				ProcessClass(context, classMetadata, command);
				command.Flush(context);
				if (config.ObjectCommitFrequency() > 0)
				{
					context.TargetCommit();
				}
			}
			BTree uuidIndex = context.SourceUuidIndex();
			if (uuidIndex != null)
			{
				command.ProcessBTree(context, uuidIndex);
			}
			command.Flush(context);
			context.TargetCommit();
		}

		// TODO order of class index/object slot processing is crucial:
		// - object slots before field indices (object slots register addresses for
		// use by string indices)
		// - class index before object slots, otherwise phantom btree entries from
		// deletions appear in the source class index?!?
		// reproducable with SelectiveCascadingDeleteTestCase and ObjectSetTestCase
		// - investigate.
		/// <exception cref="Db4objects.Db4o.CorruptionException"></exception>
		/// <exception cref="System.IO.IOException"></exception>
		private static void ProcessClass(DefragmentServicesImpl context, ClassMetadata curClass
			, IPassCommand command)
		{
			ProcessClassIndex(context, curClass, command);
			if (!ParentHasIndex(curClass))
			{
				ProcessObjectsForClass(context, curClass, command);
			}
			ProcessClassAndFieldIndices(context, curClass, command);
		}

		private static bool ParentHasIndex(ClassMetadata curClass)
		{
			ClassMetadata parentClass = curClass.GetAncestor();
			while (parentClass != null)
			{
				if (parentClass.HasClassIndex())
				{
					return true;
				}
				parentClass = parentClass.GetAncestor();
			}
			return false;
		}

		private static void ProcessObjectsForClass(DefragmentServicesImpl context, ClassMetadata
			 curClass, IPassCommand command)
		{
			context.TraverseAll(curClass, new _IVisitor4_284(command, context, curClass));
		}

		private sealed class _IVisitor4_284 : IVisitor4
		{
			public _IVisitor4_284(IPassCommand command, DefragmentServicesImpl context, ClassMetadata
				 curClass)
			{
				this.command = command;
				this.context = context;
				this.curClass = curClass;
			}

			public void Visit(object obj)
			{
				int id = ((int)obj);
				try
				{
					// FIXME bubble up exceptions
					command.ProcessObjectSlot(context, curClass, id);
				}
				catch (CorruptionException e)
				{
					Sharpen.Runtime.PrintStackTrace(e);
				}
				catch (IOException e)
				{
					Sharpen.Runtime.PrintStackTrace(e);
				}
			}

			private readonly IPassCommand command;

			private readonly DefragmentServicesImpl context;

			private readonly ClassMetadata curClass;
		}

		/// <exception cref="Db4objects.Db4o.CorruptionException"></exception>
		/// <exception cref="System.IO.IOException"></exception>
		private static void ProcessClassAndFieldIndices(DefragmentServicesImpl context, ClassMetadata
			 curClass, IPassCommand command)
		{
			int sourceClassIndexID = 0;
			int targetClassIndexID = 0;
			if (curClass.HasClassIndex())
			{
				sourceClassIndexID = curClass.Index().Id();
				targetClassIndexID = context.MappedID(sourceClassIndexID, -1);
			}
			command.ProcessClass(context, curClass, curClass.GetID(), targetClassIndexID);
		}

		/// <exception cref="Db4objects.Db4o.CorruptionException"></exception>
		/// <exception cref="System.IO.IOException"></exception>
		private static void ProcessClassIndex(DefragmentServicesImpl context, ClassMetadata
			 curClass, IPassCommand command)
		{
			if (curClass.HasClassIndex())
			{
				BTreeClassIndexStrategy indexStrategy = (BTreeClassIndexStrategy)curClass.Index();
				BTree btree = indexStrategy.Btree();
				command.ProcessBTree(context, btree);
			}
		}

		internal class NullListener : IDefragmentListener
		{
			public virtual void NotifyDefragmentInfo(DefragmentInfo info)
			{
			}
		}
	}
}
