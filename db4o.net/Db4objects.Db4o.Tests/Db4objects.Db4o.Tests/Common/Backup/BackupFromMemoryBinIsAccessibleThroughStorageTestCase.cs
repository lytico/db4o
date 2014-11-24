/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.IO;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Backup;

namespace Db4objects.Db4o.Tests.Common.Backup
{
	public class BackupFromMemoryBinIsAccessibleThroughStorageTestCase : MemoryBackupTestCaseBase
	{
		protected MemoryStorage _storage = new MemoryStorage();

		protected override IStorage OrigStorage()
		{
			return _storage;
		}

		protected override IStorage BackupStorage()
		{
			return _storage;
		}

		protected override void Backup(LocalObjectContainer origDb, string backupPath)
		{
			origDb.Backup(backupPath);
		}
	}
}
