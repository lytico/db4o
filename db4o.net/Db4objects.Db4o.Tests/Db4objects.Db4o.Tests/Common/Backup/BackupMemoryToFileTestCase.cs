/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit.Extensions;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Backup;

namespace Db4objects.Db4o.Tests.Common.Backup
{
	public class BackupMemoryToFileTestCase : MemoryBackupTestCaseBase
	{
		protected override void Backup(LocalObjectContainer origDb, string backupPath)
		{
			origDb.Backup(BackupStorage(), backupPath);
		}

		protected override IStorage BackupStorage()
		{
			return Db4oUnitPlatform.NewPersistentStorage();
		}

		protected override IStorage OrigStorage()
		{
			return new MemoryStorage();
		}
	}
}
