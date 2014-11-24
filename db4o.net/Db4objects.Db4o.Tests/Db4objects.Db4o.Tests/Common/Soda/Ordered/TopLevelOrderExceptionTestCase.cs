/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda.Ordered;

namespace Db4objects.Db4o.Tests.Common.Soda.Ordered
{
	public class TopLevelOrderExceptionTestCase : AbstractDb4oTestCase
	{
		public class Item
		{
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new TopLevelOrderExceptionTestCase.Item());
			Store(new TopLevelOrderExceptionTestCase.Item());
		}

		public virtual void TestDescending()
		{
			IQuery query = NewQuery(typeof(TopLevelOrderExceptionTestCase.Item));
			Assert.Expect(typeof(InvalidOperationException), new _ICodeBlock_20(query));
		}

		private sealed class _ICodeBlock_20 : ICodeBlock
		{
			public _ICodeBlock_20(IQuery query)
			{
				this.query = query;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				query.OrderDescending();
			}

			private readonly IQuery query;
		}

		public virtual void TestAscending()
		{
			IQuery query = NewQuery(typeof(TopLevelOrderExceptionTestCase.Item));
			Assert.Expect(typeof(InvalidOperationException), new _ICodeBlock_29(query));
		}

		private sealed class _ICodeBlock_29 : ICodeBlock
		{
			public _ICodeBlock_29(IQuery query)
			{
				this.query = query;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				query.OrderAscending();
			}

			private readonly IQuery query;
		}
	}
}
