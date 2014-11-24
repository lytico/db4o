/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Types.Arrays;

namespace Db4objects.Db4o.Tests.Common.Types.Arrays
{
	public class SimpleTypeArrayInUntypedVariableTestCase : AbstractDb4oTestCase
	{
		private static readonly int[] Array = new int[] { 1, 2, 3 };

		public class Data
		{
			public object _arr;

			public Data(object arr)
			{
				this._arr = arr;
			}
		}

		protected override void Store()
		{
			Db().Store(new SimpleTypeArrayInUntypedVariableTestCase.Data(Array));
		}

		public virtual void TestRetrieval()
		{
			SimpleTypeArrayInUntypedVariableTestCase.Data data = (SimpleTypeArrayInUntypedVariableTestCase.Data
				)((SimpleTypeArrayInUntypedVariableTestCase.Data)RetrieveOnlyInstance(typeof(SimpleTypeArrayInUntypedVariableTestCase.Data
				)));
			Assert.IsTrue(data._arr is int[]);
			int[] arri = (int[])data._arr;
			ArrayAssert.AreEqual(Array, arri);
		}
	}
}
