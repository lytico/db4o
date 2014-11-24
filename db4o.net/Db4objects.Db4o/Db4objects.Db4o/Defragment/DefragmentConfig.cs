/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Defragment;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Config;

namespace Db4objects.Db4o.Defragment
{
	/// <summary>Configuration for a defragmentation run.</summary>
	/// <remarks>Configuration for a defragmentation run.</remarks>
	/// <seealso cref="Defragment">Defragment</seealso>
	public class DefragmentConfig
	{
		public const bool Debug = false;

		public static readonly string BackupSuffix = "backup";

		private string _origPath;

		private string _backupPath;

		private string _tempPath;

		private IIdMapping _mapping;

		private IConfiguration _config;

		private IStoredClassFilter _storedClassFilter = null;

		private bool _forceBackupDelete = false;

		private bool _readOnly = true;

		private int _objectCommitFrequency;

		private IStorage _backupStorage;

		/// <summary>Creates a configuration for a defragmentation run.</summary>
		/// <remarks>
		/// Creates a configuration for a defragmentation run. The backup and mapping
		/// file paths are generated from the original path by appending the default
		/// suffixes. All properties other than the provided paths are set to FALSE
		/// by default.
		/// </remarks>
		/// <param name="origPath">
		/// The path to the file to be defragmented. Must exist and must be
		/// a valid db4o file.
		/// </param>
		public DefragmentConfig(string origPath) : this(origPath, origPath + "." + BackupSuffix
			)
		{
		}

		/// <summary>Creates a configuration for a defragmentation run with in-memory mapping.
		/// 	</summary>
		/// <remarks>
		/// Creates a configuration for a defragmentation run with in-memory mapping.
		/// All properties other than the provided paths are set to FALSE by default.
		/// </remarks>
		/// <param name="origPath">
		/// The path to the file to be defragmented. Must exist and must be
		/// a valid db4o file.
		/// </param>
		/// <param name="backupPath">
		/// The path to the backup of the original file. No file should
		/// exist at this position, otherwise it will be OVERWRITTEN if forceBackupDelete()
		/// is set to true!
		/// </param>
		public DefragmentConfig(string origPath, string backupPath) : this(origPath, backupPath
			, new InMemoryIdMapping())
		{
		}

		/// <summary>Creates a configuration for a defragmentation run.</summary>
		/// <remarks>
		/// Creates a configuration for a defragmentation run. All properties other
		/// than the provided paths are set to FALSE by default.
		/// </remarks>
		/// <param name="origPath">
		/// The path to the file to be defragmented. Must exist and must be
		/// a valid db4o file.
		/// </param>
		/// <param name="backupPath">
		/// The path to the backup of the original file. No file should
		/// exist at this position, otherwise it will be OVERWRITTEN if forceBackupDelete()
		/// is set to true!
		/// </param>
		/// <param name="mapping">
		/// The Id mapping to be used internally. Pass either a
		/// <see cref="InMemoryIdMapping">InMemoryIdMapping</see>
		/// for fastest defragment or a
		/// <see cref="DatabaseIdMapping">DatabaseIdMapping</see>
		/// for low memory consumption.
		/// </param>
		public DefragmentConfig(string origPath, string backupPath, IIdMapping mapping)
		{
			_origPath = origPath;
			_backupPath = backupPath;
			_mapping = mapping;
		}

		/// <returns>The path to the file to be defragmented.</returns>
		public virtual string OrigPath()
		{
			return _origPath;
		}

		/// <returns>The path to the backup of the original file.</returns>
		public virtual string BackupPath()
		{
			return _backupPath;
		}

		/// <returns>The temporary ID mapping used internally. For internal use only.</returns>
		public virtual IIdMapping Mapping()
		{
			return _mapping;
		}

		/// <returns>
		/// The
		/// <see cref="IStoredClassFilter">IStoredClassFilter</see>
		/// used to select stored class extents to
		/// be included into the defragmented file.
		/// </returns>
		public virtual IStoredClassFilter StoredClassFilter()
		{
			return (_storedClassFilter == null ? Nullfilter : _storedClassFilter);
		}

		/// <param name="storedClassFilter">
		/// The
		/// <see cref="IStoredClassFilter">IStoredClassFilter</see>
		/// used to select stored class extents to
		/// be included into the defragmented file.
		/// </param>
		public virtual void StoredClassFilter(IStoredClassFilter storedClassFilter)
		{
			_storedClassFilter = storedClassFilter;
		}

		/// <returns>true, if an existing backup file should be deleted, false otherwise.</returns>
		public virtual bool ForceBackupDelete()
		{
			return _forceBackupDelete;
		}

		/// <param name="forceBackupDelete">true, if an existing backup file should be deleted, false otherwise.
		/// 	</param>
		public virtual void ForceBackupDelete(bool forceBackupDelete)
		{
			_forceBackupDelete = forceBackupDelete;
		}

		/// <summary>
		/// allows turning on and off readonly mode.<br /><br />
		/// When changed classes are likely to be detected defragment, it may be required
		/// to open the original database in read/write mode.
		/// </summary>
		/// <remarks>
		/// allows turning on and off readonly mode.<br /><br />
		/// When changed classes are likely to be detected defragment, it may be required
		/// to open the original database in read/write mode. <br /><br />
		/// Readonly mode is the default setting.
		/// </remarks>
		/// <param name="flag">false, to turn off readonly mode.</param>
		public virtual void ReadOnly(bool flag)
		{
			_readOnly = flag;
		}

		/// <returns>true, if the original database file is to be opened in readonly mode.</returns>
		public virtual bool ReadOnly()
		{
			return _readOnly;
		}

		/// <returns>
		/// The db4o
		/// <see cref="Db4objects.Db4o.Config.IConfiguration">IConfiguration</see>
		/// to be applied
		/// during the defragment process.
		/// </returns>
		public virtual IConfiguration Db4oConfig()
		{
			if (_config == null)
			{
				_config = VanillaDb4oConfig(1);
			}
			return _config;
		}

		/// <param name="config">
		/// The db4o
		/// <see cref="Db4objects.Db4o.Config.IConfiguration">IConfiguration</see>
		/// to be applied
		/// during the defragment process.
		/// </param>
		[System.ObsoleteAttribute(@"since 7.9: use Db4oConfig(Db4objects.Db4o.Config.IEmbeddedConfiguration) instead"
			)]
		public virtual void Db4oConfig(IConfiguration config)
		{
			_config = config;
		}

		/// <param name="config">
		/// The db4o
		/// <see cref="Db4objects.Db4o.Config.IEmbeddedConfiguration">IEmbeddedConfiguration</see>
		/// to be applied
		/// during the defragment process.
		/// </param>
		/// <since>7.9</since>
		public virtual void Db4oConfig(IEmbeddedConfiguration config)
		{
			_config = ((EmbeddedConfigurationImpl)config).Legacy();
		}

		public virtual int ObjectCommitFrequency()
		{
			return _objectCommitFrequency;
		}

		/// <param name="objectCommitFrequency">
		/// The number of processed object (slots) that should trigger an
		/// intermediate commit of the target file. Default: 0, meaning: never.
		/// </param>
		public virtual void ObjectCommitFrequency(int objectCommitFrequency)
		{
			_objectCommitFrequency = objectCommitFrequency;
		}

		/// <summary>
		/// Instruct the defragment process to upgrade the source file to the current db4o
		/// version prior to defragmenting it.
		/// </summary>
		/// <remarks>
		/// Instruct the defragment process to upgrade the source file to the current db4o
		/// version prior to defragmenting it. Use this option if your source file has been created
		/// with an older db4o version than the one you are using.
		/// </remarks>
		/// <param name="tempPath">The location for an intermediate, upgraded version of the source file.
		/// 	</param>
		public virtual void UpgradeFile(string tempPath)
		{
			_tempPath = tempPath;
		}

		public virtual bool FileNeedsUpgrade()
		{
			return _tempPath != null;
		}

		public virtual string TempPath()
		{
			return (_tempPath != null ? _tempPath : _backupPath);
		}

		public virtual int BlockSize()
		{
			return ((Config4Impl)Db4oConfig()).BlockSize();
		}

		protected class NullFilter : IStoredClassFilter
		{
			public virtual bool Accept(IStoredClass storedClass)
			{
				return true;
			}
		}

		private static readonly IStoredClassFilter Nullfilter = new DefragmentConfig.NullFilter
			();

		public static IConfiguration VanillaDb4oConfig(int blockSize)
		{
			IConfiguration config = Db4oFactory.NewConfiguration();
			config.WeakReferences(false);
			config.BlockSize(blockSize);
			return config;
		}

		public virtual IConfiguration ClonedDb4oConfig()
		{
			return (IConfiguration)((Config4Impl)Db4oConfig()).DeepClone(null);
		}

		public virtual void BackupStorage(IStorage backupStorage)
		{
			_backupStorage = backupStorage;
		}

		public virtual IStorage BackupStorage()
		{
			if (_backupStorage != null)
			{
				return _backupStorage;
			}
			return _config.Storage;
		}
	}
}
