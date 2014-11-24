/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Concurrency;

namespace Db4objects.Db4o.Tests.Common.Concurrency
{
	public class ArrayNOrderTestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new ArrayNOrderTestCase().RunConcurrency();
		}

		public class Item
		{
			public string[][][] s1;

			public object[][] o1;
		}

		protected override void Store()
		{
			ArrayNOrderTestCase.Item item = new ArrayNOrderTestCase.Item();
			item.s1 = new string[][][] { new string[][] { new string[3], new string[3] }, new 
				string[][] { new string[3], new string[3] } };
			item.s1[0][0][0] = "000";
			item.s1[0][0][1] = "001";
			item.s1[0][0][2] = "002";
			item.s1[0][1][0] = "010";
			item.s1[0][1][1] = "011";
			item.s1[0][1][2] = "012";
			item.s1[1][0][0] = "100";
			item.s1[1][0][1] = "101";
			item.s1[1][0][2] = "102";
			item.s1[1][1][0] = "110";
			item.s1[1][1][1] = "111";
			item.s1[1][1][2] = "112";
			item.o1 = new object[][] { new object[2], new object[2] };
			item.o1[0][0] = 0;
			item.o1[0][1] = "01";
			item.o1[1][0] = System.Convert.ToSingle(10);
			item.o1[1][1] = 1.1;
			Store(item);
		}

		public virtual void Conc(IExtObjectContainer oc)
		{
			ArrayNOrderTestCase.Item item = (ArrayNOrderTestCase.Item)((ArrayNOrderTestCase.Item
				)RetrieveOnlyInstance(oc, typeof(ArrayNOrderTestCase.Item)));
			AssertItem(item);
		}

		public virtual void AssertItem(ArrayNOrderTestCase.Item item)
		{
			Assert.AreEqual(item.s1[0][0][0], "000");
			Assert.AreEqual(item.s1[0][0][1], "001");
			Assert.AreEqual(item.s1[0][0][2], "002");
			Assert.AreEqual(item.s1[0][1][0], "010");
			Assert.AreEqual(item.s1[0][1][1], "011");
			Assert.AreEqual(item.s1[0][1][2], "012");
			Assert.AreEqual(item.s1[1][0][0], "100");
			Assert.AreEqual(item.s1[1][0][1], "101");
			Assert.AreEqual(item.s1[1][0][2], "102");
			Assert.AreEqual(item.s1[1][1][0], "110");
			Assert.AreEqual(item.s1[1][1][1], "111");
			Assert.AreEqual(item.s1[1][1][2], "112");
			Assert.AreEqual(item.o1[0][0], 0);
			Assert.AreEqual(item.o1[0][1], "01");
			Assert.AreEqual(item.o1[1][0], System.Convert.ToSingle(10));
			Assert.AreEqual(item.o1[1][1], 1.1);
		}
	}
}
#endif // !SILVERLIGHT
