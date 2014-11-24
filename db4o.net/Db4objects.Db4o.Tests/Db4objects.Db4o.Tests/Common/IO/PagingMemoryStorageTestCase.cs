/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Tests.Common.IO;

namespace Db4objects.Db4o.Tests.Common.IO
{
	public class PagingMemoryStorageTestCase : ITestCase
	{
		private static readonly byte[] Data = new byte[] { 1, 2, 3, 4 };

		public virtual void Test()
		{
			PagingMemoryStorage storage = new _PagingMemoryStorage_13();
			IBin testBin = storage.Open(new BinConfiguration(string.Empty, true, 0, false));
			Assert.AreEqual(Data.Length, testBin.Length());
			int actualLength = (int)testBin.Length();
			byte[] read = new byte[actualLength];
			testBin.Read(0, read, actualLength);
			ArrayAssert.AreEqual(Data, read);
		}

		private sealed class _PagingMemoryStorage_13 : PagingMemoryStorage
		{
			public _PagingMemoryStorage_13()
			{
			}

			protected override IBin ProduceBin(BinConfiguration config, int pageSize)
			{
				IBin bin = base.ProduceBin(config, pageSize);
				bin.Write(0, PagingMemoryStorageTestCase.Data, PagingMemoryStorageTestCase.Data.Length
					);
				return bin;
			}
		}
	}
}
