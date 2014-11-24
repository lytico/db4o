/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Fixtures;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Tests.Common.Api;

namespace Db4objects.Db4o.Tests.Common.IO
{
	public class StorageTest : TestWithTempFile
	{
		#if !SILVERLIGHT
		public virtual void TestInitialLength()
		{
			Storage().Open(new BinConfiguration(TempFile(), false, 1000, false)).Close();
			IBin bin = Storage().Open(new BinConfiguration(TempFile(), false, 0, false));
			try
			{
				Assert.AreEqual(1000, bin.Length());
			}
			finally
			{
				bin.Close();
			}
		}
		#endif // !SILVERLIGHT

		private IStorage Storage()
		{
			return ((IStorage)SubjectFixtureProvider.Value());
		}
	}
}
