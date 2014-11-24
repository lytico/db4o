/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Types.Arrays;

namespace Db4objects.Db4o.Tests.Common.Types.Arrays
{
	public class NestedArraysTestCase : AbstractDb4oTestCase
	{
		private const int Depth = 5;

		private const int Elements = 3;

		public class Data
		{
			public object _obj;

			public object[] _arr;

			public Data(object obj, object[] arr)
			{
				this._obj = obj;
				_arr = arr;
			}
		}

		protected override void Store()
		{
			object[] obj = new object[Elements];
			Fill(obj, Depth);
			object[] arr = new object[Elements];
			Fill(arr, Depth);
			Db().Store(new NestedArraysTestCase.Data(obj, arr));
		}

		private void Fill(object[] arr, int depth)
		{
			if (depth <= 0)
			{
				arr[0] = "somestring";
				arr[1] = 10;
				return;
			}
			depth--;
			for (int i = 0; i < Elements; i++)
			{
				arr[i] = new object[Elements];
				Fill((object[])arr[i], depth);
			}
		}

		public virtual void TestOne()
		{
			NestedArraysTestCase.Data data = (NestedArraysTestCase.Data)((NestedArraysTestCase.Data
				)RetrieveOnlyInstance(typeof(NestedArraysTestCase.Data)));
			Db().Activate(data, int.MaxValue);
			Check((object[])data._obj, Depth);
			Check(data._arr, Depth);
		}

		private void Check(object[] arr, int depth)
		{
			if (depth <= 0)
			{
				Assert.AreEqual("somestring", arr[0]);
				Assert.AreEqual(10, arr[1]);
				return;
			}
			depth--;
			for (int i = 0; i < Elements; i++)
			{
				Check((object[])arr[i], depth);
			}
		}
	}
}
