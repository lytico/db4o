/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class GetSingleSimpleArrayTestCase : AbstractDb4oTestCase
	{
		public virtual void Test()
		{
			IObjectSet result = Db().QueryByExample(new double[] { 0.6, 0.4 });
			Assert.IsFalse(result.HasNext());
			Assert.IsFalse(result.HasNext());
			Assert.Expect(typeof(InvalidOperationException), new _ICodeBlock_17(result));
		}

		private sealed class _ICodeBlock_17 : ICodeBlock
		{
			public _ICodeBlock_17(IObjectSet result)
			{
				this.result = result;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				result.Next();
			}

			private readonly IObjectSet result;
		}
	}
}
