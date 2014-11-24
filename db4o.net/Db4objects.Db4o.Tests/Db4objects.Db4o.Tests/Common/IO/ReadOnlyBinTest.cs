/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Tests.Common.IO;

namespace Db4objects.Db4o.Tests.Common.IO
{
	public class ReadOnlyBinTest : StorageTestUnitBase
	{
		public virtual void Test()
		{
			ReopenAsReadOnly();
			AssertReadOnly(_bin);
		}

		private void ReopenAsReadOnly()
		{
			Close();
			Open(true);
		}

		private void AssertReadOnly(IBin adapter)
		{
			Assert.Expect(typeof(Db4oIOException), new _ICodeBlock_21(adapter));
		}

		private sealed class _ICodeBlock_21 : ICodeBlock
		{
			public _ICodeBlock_21(IBin adapter)
			{
				this.adapter = adapter;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				adapter.Write(0, new byte[] { 0 }, 1);
			}

			private readonly IBin adapter;
		}
	}
}
