/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */
#if SILVERLIGHT

using Db4objects.Db4o.IO;
using Db4objects.Db4o.Tests.Common.Api;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.Silverlight.IO
{
	public class IsolatedStorageStorageTestCase : TestWithTempFile
	{
		private readonly IsolatedStorageStorage _storage = new IsolatedStorageStorage();

		public void TestDeletingNonExistingFileDontThrows()
		{
			_storage.Delete("WhoWouldNameAFileLikeThis.question-mark");
		}

		public void TestRetrievingSizeOnAOpenFileDoenstThrow()
		{
			IBin bin = _storage.Open(new BinConfiguration(TempFile(), true, 0, false));
			try
			{
				byte[] bytes = System.Text.Encoding.UTF8.GetBytes("Test String");
				bin.Write(0, bytes, bytes.Length);

				Assert.AreEqual(bytes.Length, IsolatedStorageStorage.FileSize(TempFile()));
			}
			finally
			{
				bin.Close();
			}
		}
	}
}

#endif