/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.TA;
using Db4objects.Db4o.Tests.Jre5.Collections;

namespace Db4objects.Db4o.Tests.Jre5.Collections
{
	public class ArrayList4SODATestCase : TransparentActivationTestCaseBase
	{
		private static readonly Product ProductBatery = new Product("BATE", "Batery 9v");

		private static readonly Product ProductKeyboard = new Product("KEYB", "Wireless keyboard"
			);

		private static readonly Product ProductChocolate = new Product("CHOC", "Chocolate"
			);

		private static readonly Product ProductMouse = new Product("MOUS", "Wireless Mouse"
			);

		private static readonly Product ProductNote = new Product("NOTE", "Core Quad notebook with 1 Tb memory"
			);

		private static readonly Product[] products = new Product[] { ProductBatery, ProductChocolate
			, ProductKeyboard, ProductMouse, ProductNote };

		public virtual void TestSODAAutodescend()
		{
			for (int i = 0; i < products.Length; i++)
			{
				AssertCount(i);
			}
		}

		private void AssertCount(int index)
		{
			IQuery query = Db().Query();
			query.Constrain(typeof(Order));
			query.Descend("_items").Descend("_product").Descend("_code").Constrain(products[index
				].Code());
			IObjectSet results = query.Execute();
			Assert.AreEqual(products.Length - index, results.Count);
			foreach (object item in results)
			{
				Order order = (Order)item;
				for (int j = 0; j < order.Size(); j++)
				{
					Assert.AreEqual(products[j].Code(), order.Item(j).Product().Code());
				}
			}
		}

		protected override void Store()
		{
			for (int i = 0; i < products.Length; i++)
			{
				Store(CreateOrder(i));
			}
		}

		private Order CreateOrder(int itemIndex)
		{
			Order o = new Order();
			for (int i = 0; i <= itemIndex; i++)
			{
				o.AddItem(new OrderItem(products[i], i));
			}
			return o;
		}
	}
}
#endif // !SILVERLIGHT
