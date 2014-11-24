/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Ext;

namespace Db4objects.Db4o.Tests.Common.Concurrency
{
	public class NestedArraysTestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.Concurrency.NestedArraysTestCase().RunConcurrency
				();
		}

		public object _object;

		public object[] _objectArray;

		private const int Depth = 5;

		private const int Elements = 3;

		public NestedArraysTestCase()
		{
		}

		protected override void Store()
		{
			_object = new object[Elements];
			Fill((object[])_object, Depth);
			_objectArray = new object[Elements];
			Fill(_objectArray, Depth);
			Store(this);
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

		public virtual void Conc(IExtObjectContainer oc)
		{
			Db4objects.Db4o.Tests.Common.Concurrency.NestedArraysTestCase nr = (Db4objects.Db4o.Tests.Common.Concurrency.NestedArraysTestCase
				)((Db4objects.Db4o.Tests.Common.Concurrency.NestedArraysTestCase)RetrieveOnlyInstance
				(oc, typeof(Db4objects.Db4o.Tests.Common.Concurrency.NestedArraysTestCase)));
			Check((object[])nr._object, Depth);
			Check((object[])nr._objectArray, Depth);
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
#endif // !SILVERLIGHT
