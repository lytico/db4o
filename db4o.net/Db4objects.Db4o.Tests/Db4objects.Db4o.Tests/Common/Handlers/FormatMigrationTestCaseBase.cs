/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.IO;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Defragment;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Foundation.IO;
using Db4objects.Db4o.Tests.Common.Handlers;
using Db4objects.Db4o.Tests.Util;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	public abstract partial class FormatMigrationTestCaseBase : ITestLifeCycle, IOptOutNoFileSystemData
		, IOptOutMultiSession, IOptOutWorkspaceIssue
	{
		private static readonly string Username = "db4o";

		private static readonly string Password = Username;

		private string _db4oVersion;

		public virtual void Configure()
		{
			IConfiguration config = Db4oFactory.Configure();
			config.AllowVersionUpdates(true);
			ConfigureForTest(config);
		}

		private void Deconfigure()
		{
			IConfiguration config = Db4oFactory.Configure();
			config.AllowVersionUpdates(false);
			DeconfigureForTest(config);
		}

		private byte _db4oHeaderVersion;

		public virtual void CreateDatabase()
		{
			CreateDatabase(FileName());
		}

		public virtual void CreateDatabaseFor(string versionName)
		{
			_db4oVersion = versionName;
			IConfiguration config = Db4oFactory.Configure();
			try
			{
				ConfigureForStore(config);
			}
			catch
			{
			}
			// Some old database engines may throw NoSuchMethodError
			// for configuration methods they don't know yet. Ignore,
			// but tell the implementor:
			// System.out.println("Exception in configureForStore for " + versionName + " in " + getClass().getName());
			try
			{
				CreateDatabase(FileName(versionName));
			}
			finally
			{
				DeconfigureForStore(config);
			}
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void SetUp()
		{
			Configure();
			CreateDatabase();
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual void Test()
		{
			for (int i = 0; i < VersionNames().Length; i++)
			{
				string versionName = VersionNames()[i];
				Test(versionName);
			}
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual void Test(string versionName)
		{
			_db4oVersion = versionName;
			if (!IsApplicableForDb4oVersion())
			{
				return;
			}
			string testFileName = FileName(versionName);
			if (!System.IO.File.Exists(testFileName))
			{
				Sharpen.Runtime.Out.WriteLine("Version upgrade check failed. File not found:" + testFileName
					);
				// FIXME: The following fails the CC build since not all files are there on .NET.
				//        Change back when we have all files.
				// Assert.fail("Version upgrade check failed. File not found:" + testFileName);
				return;
			}
			//      System.out.println("Checking database file: " + testFileName);
			InvestigateFileHeaderVersion(testFileName);
			PrepareClientServerTest(testFileName);
			try
			{
				RunDeletionTests(testFileName);
				DefragmentSoloAndCS(testFileName);
				CheckDatabaseFile(testFileName);
				// Twice, to ensure everything is fine after opening, converting and closing.
				CheckDatabaseFile(testFileName);
				UpdateDatabaseFile(testFileName);
				CheckUpdatedDatabaseFile(testFileName);
				DefragmentSoloAndCS(testFileName);
				CheckUpdatedDatabaseFile(testFileName);
			}
			finally
			{
				TearDownClientServer(testFileName);
			}
		}

		/// <exception cref="System.IO.IOException"></exception>
		private void DefragmentSoloAndCS(string fileName)
		{
			RunDefrag(fileName);
			RunDefrag(ClientServerFileName(fileName));
		}

		private void TearDownClientServer(string testFileName)
		{
			File4.Delete(ClientServerFileName(testFileName));
		}

		/// <exception cref="System.IO.IOException"></exception>
		private void PrepareClientServerTest(string fileName)
		{
			File4.Copy(fileName, ClientServerFileName(fileName));
		}

		private string ClientServerFileName(string fileName)
		{
			return fileName + ".CS";
		}

		/// <exception cref="System.IO.IOException"></exception>
		private void RunDeletionTests(string testFileName)
		{
			WithDatabase(testFileName, new _IFunction4_152(this));
			CheckDatabaseFile(testFileName);
		}

		private sealed class _IFunction4_152 : IFunction4
		{
			public _IFunction4_152(FormatMigrationTestCaseBase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public object Apply(object db)
			{
				this._enclosing.AssertObjectDeletion(((IObjectContainer)db).Ext());
				return null;
			}

			private readonly FormatMigrationTestCaseBase _enclosing;
		}

		/// <summary>Override to provide tests for deletion.</summary>
		/// <remarks>Override to provide tests for deletion.</remarks>
		protected virtual void AssertObjectDeletion(IExtObjectContainer objectContainer)
		{
		}

		/// <summary>Can be overridden to disable the test for specific db4o versions.</summary>
		/// <remarks>Can be overridden to disable the test for specific db4o versions.</remarks>
		protected virtual bool IsApplicableForDb4oVersion()
		{
			return true;
		}

		private void CheckDatabaseFile(string testFile)
		{
			WithDatabase(testFile, new _IFunction4_174(this));
		}

		private sealed class _IFunction4_174 : IFunction4
		{
			public _IFunction4_174(FormatMigrationTestCaseBase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public object Apply(object objectContainer)
			{
				this._enclosing.AssertObjectsAreReadable((IExtObjectContainer)objectContainer);
				return null;
			}

			private readonly FormatMigrationTestCaseBase _enclosing;
		}

		private void CheckUpdatedDatabaseFile(string testFile)
		{
			WithDatabase(testFile, new _IFunction4_183(this));
		}

		private sealed class _IFunction4_183 : IFunction4
		{
			public _IFunction4_183(FormatMigrationTestCaseBase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public object Apply(object objectContainer)
			{
				this._enclosing.AssertObjectsAreUpdated((IExtObjectContainer)objectContainer);
				return null;
			}

			private readonly FormatMigrationTestCaseBase _enclosing;
		}

		private void CreateDatabase(string file)
		{
			if (!IsApplicableForDb4oVersion())
			{
				return;
			}
			System.IO.Directory.CreateDirectory(DatabasePath);
			if (System.IO.File.Exists(file))
			{
				File4.Delete(file);
			}
			IExtObjectContainer objectContainer = Db4oFactory.OpenFile(file).Ext();
			IObjectContainerAdapter adapter = ObjectContainerAdapterFactory.ForVersion(Db4oMajorVersion
				(), Db4oMinorVersion()).ForContainer(objectContainer);
			try
			{
				Store(adapter);
			}
			finally
			{
				objectContainer.Close();
			}
		}

		private string DatabasePath
		{
			get
			{
				return Path.Combine(GetTempPath(), "test/db4oVersions");
			}
		}

		/// <exception cref="System.IO.IOException"></exception>
		private void InvestigateFileHeaderVersion(string testFile)
		{
			_db4oHeaderVersion = VersionServices.FileHeaderVersion(testFile);
		}

		/// <exception cref="System.IO.IOException"></exception>
		private void RunDefrag(string testFileName)
		{
			IConfiguration config = Db4oFactory.NewConfiguration();
			config.AllowVersionUpdates(true);
			ConfigureForTest(config);
			IObjectContainer oc = Db4oFactory.OpenFile(config, testFileName);
			oc.Close();
			string backupFileName = Path.GetTempFileName();
			try
			{
				DefragmentConfig defragConfig = new DefragmentConfig(testFileName, backupFileName
					);
				defragConfig.ForceBackupDelete(true);
				ConfigureForTest(defragConfig.Db4oConfig());
				defragConfig.ReadOnly(!DefragmentInReadWriteMode());
				Db4objects.Db4o.Defragment.Defragment.Defrag(defragConfig);
			}
			finally
			{
				File4.Delete(backupFileName);
			}
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TearDown()
		{
			Deconfigure();
		}

		private void UpdateDatabaseFile(string testFile)
		{
			WithDatabase(testFile, new _IFunction4_247(this));
		}

		private sealed class _IFunction4_247 : IFunction4
		{
			public _IFunction4_247(FormatMigrationTestCaseBase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public object Apply(object objectContainer)
			{
				this._enclosing.Update((IExtObjectContainer)objectContainer);
				return null;
			}

			private readonly FormatMigrationTestCaseBase _enclosing;
		}

		private void WithDatabase(string file, IFunction4 function)
		{
			Configure();
			IExtObjectContainer objectContainer = Db4oFactory.OpenFile(file).Ext();
			try
			{
				function.Apply(objectContainer);
			}
			finally
			{
				objectContainer.Close();
			}
			IObjectServer server = Db4oFactory.OpenServer(ClientServerFileName(file), -1);
			server.GrantAccess(Username, Password);
			objectContainer = Db4oFactory.OpenClient("localhost", server.Ext().Port(), Username
				, Password).Ext();
			try
			{
				function.Apply(objectContainer);
			}
			finally
			{
				objectContainer.Close();
				server.Close();
			}
		}

		protected abstract void AssertObjectsAreReadable(IExtObjectContainer objectContainer
			);

		protected virtual void AssertObjectsAreUpdated(IExtObjectContainer objectContainer
			)
		{
		}

		// Override to check updates also
		protected virtual void ConfigureForStore(IConfiguration config)
		{
		}

		// Override for special storage configuration.
		protected virtual void ConfigureForTest(IConfiguration config)
		{
		}

		// Override for special testing configuration.
		protected virtual byte Db4oHeaderVersion()
		{
			return _db4oHeaderVersion;
		}

		protected virtual int Db4oMajorVersion()
		{
			if (_db4oVersion != null)
			{
				return System.Convert.ToInt32(Sharpen.Runtime.Substring(_db4oVersion, 0, 1));
			}
			return System.Convert.ToInt32(Sharpen.Runtime.Substring(Db4oFactory.Version(), 5, 
				6));
		}

		protected virtual int Db4oMinorVersion()
		{
			if (_db4oVersion != null)
			{
				return System.Convert.ToInt32(Sharpen.Runtime.Substring(_db4oVersion, 2, 3));
			}
			return System.Convert.ToInt32(Sharpen.Runtime.Substring(Db4oFactory.Version(), 7, 
				8));
		}

		/// <summary>override and return true for database updates that produce changed class metadata
		/// 	</summary>
		protected virtual bool DefragmentInReadWriteMode()
		{
			return false;
		}

		protected virtual string FileName()
		{
			_db4oVersion = Db4oVersion.Name;
			return FileName(_db4oVersion);
		}

		protected virtual string FileName(string versionName)
		{
			return OldVersionFileName(versionName) + ".db4o";
		}

		protected virtual void DeconfigureForStore(IConfiguration config)
		{
		}

		// Override for special storage deconfiguration.
		protected virtual void DeconfigureForTest(IConfiguration config)
		{
		}

		// Override for special storage deconfiguration.
		protected abstract string FileNamePrefix();

		protected virtual string OldVersionFileName(string versionName)
		{
			return Path.Combine(DatabasePath, FileNamePrefix() + versionName.Replace(' ', '_'
				));
		}

		protected abstract void Store(IObjectContainerAdapter objectContainer);

		protected virtual void Update(IExtObjectContainer objectContainer)
		{
		}

		// Override to do updates also
		protected virtual string[] VersionNames()
		{
			return new string[] { Sharpen.Runtime.Substring(Db4oFactory.Version(), 5) };
		}
	}
}
