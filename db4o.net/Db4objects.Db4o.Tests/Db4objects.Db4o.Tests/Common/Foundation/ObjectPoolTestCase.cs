/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Tests.Common.Foundation
{
	public class ObjectPoolTestCase : ITestCase
	{
		public virtual void Test()
		{
			object o1 = new object();
			object o2 = new object();
			object o3 = new object();
			IObjectPool pool = new SimpleObjectPool(new object[] { o1, o2, o3 });
			Assert.AreSame(o1, pool.BorrowObject());
			Assert.AreSame(o2, pool.BorrowObject());
			Assert.AreSame(o3, pool.BorrowObject());
			Assert.Expect(typeof(InvalidOperationException), new _ICodeBlock_20(pool));
			pool.ReturnObject(o2);
			Assert.AreSame(o2, pool.BorrowObject());
		}

		private sealed class _ICodeBlock_20 : ICodeBlock
		{
			public _ICodeBlock_20(IObjectPool pool)
			{
				this.pool = pool;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				pool.BorrowObject();
			}

			private readonly IObjectPool pool;
		}
	}
}
