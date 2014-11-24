/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Store;

namespace Db4objects.Db4o.Tests.Common.Store
{
	public class DeepSetTestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] args)
		{
			new DeepSetTestCase().RunSolo();
		}

		public class Item
		{
			public DeepSetTestCase.Item child;

			public string name;
		}

		private DeepSetTestCase.Item _item;

		protected override void Store()
		{
			_item = new DeepSetTestCase.Item();
			_item.name = "1";
			_item.child = new DeepSetTestCase.Item();
			_item.child.name = "2";
			_item.child.child = new DeepSetTestCase.Item();
			_item.child.child.name = "3";
			Store(_item);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Test()
		{
			IExtObjectContainer oc = Db();
			_item.name = "1";
			DeepSetTestCase.Item item = (DeepSetTestCase.Item)oc.QueryByExample(_item).Next();
			item.name = "11";
			item.child.name = "12";
			oc.Store(item, 2);
			oc.Deactivate(item, int.MaxValue);
			item.name = "11";
			item = (DeepSetTestCase.Item)oc.QueryByExample(item).Next();
			Assert.AreEqual("12", item.child.name);
			Assert.AreEqual("3", item.child.child.name);
		}
	}
}
