/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Tests.Common.Api;

namespace Db4objects.Db4o.Tests.Common.IO
{
	public class RandomAccessFileStorageFactoryTestCase : TestWithTempFile
	{
		private readonly IStorage subject = Db4oUnitPlatform.NewPersistentStorage();

		public virtual void TestExistsWithUnexistentFile()
		{
			Assert.IsFalse(subject.Exists(TempFile()));
		}

		public virtual void TestExistsWithZeroLengthFile()
		{
			IBin storage = subject.Open(new BinConfiguration(TempFile(), false, 0, false));
			storage.Close();
			Assert.IsFalse(subject.Exists(TempFile()));
		}
	}
}
