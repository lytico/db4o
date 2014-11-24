/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Api;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public partial class DbPathDoesNotExistTestCase : Db4oTestWithTempFile
	{
		public virtual void Test()
		{
			Assert.Expect(typeof(Db4oIOException), new _ICodeBlock_18(this));
		}

		private sealed class _ICodeBlock_18 : ICodeBlock
		{
			public _ICodeBlock_18(DbPathDoesNotExistTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				Db4oEmbedded.OpenFile(this._enclosing.NewConfiguration(), this._enclosing.NonExistingFilePath
					());
			}

			private readonly DbPathDoesNotExistTestCase _enclosing;
		}
	}
}
