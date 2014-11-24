/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Tests.Common.TA;
using Db4objects.Db4o.Tests.Common.TA.Mixed;

namespace Db4objects.Db4o.Tests.Common.TA.Mixed
{
	public class MixedArrayTestCase : ItemTestCaseBase
	{
		public static void Main(string[] args)
		{
			new MixedArrayTestCase().RunAll();
		}

		/// <exception cref="System.Exception"></exception>
		protected override object CreateItem()
		{
			return new MixedArrayItem(Depth());
		}

		#if !CF
		private int Depth()
		{
			return 42;
		}
		#endif // !CF

		#if CF
		private int Depth()
		{
			return 10;
		}
		#endif // CF

		/// <exception cref="System.Exception"></exception>
		protected override void AssertItemValue(object obj)
		{
			MixedArrayItem item = (MixedArrayItem)obj;
			object[] objects = item.objects;
			Assert.AreEqual(Depth(), ((TItem)objects[1]).Value());
			Assert.AreEqual(Depth(), ((TItem)objects[3]).Value());
		}

		/// <exception cref="System.Exception"></exception>
		protected override void AssertRetrievedItem(object obj)
		{
			MixedArrayItem item = (MixedArrayItem)obj;
			object[] objects = item.objects;
			Assert.IsNotNull(objects);
			for (int i = 0; i < objects.Length; ++i)
			{
				Assert.IsNotNull(objects[i]);
			}
			Assert.AreEqual(LinkedList.NewList(Depth()), objects[0]);
			Assert.AreEqual(0, ((TItem)objects[1]).value);
			Assert.AreEqual(LinkedList.NewList(Depth()), objects[2]);
			Assert.AreEqual(0, ((TItem)objects[3]).value);
		}
	}
}
