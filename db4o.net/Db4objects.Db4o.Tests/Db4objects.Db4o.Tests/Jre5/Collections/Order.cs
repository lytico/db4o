/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4objects.Db4o.Activation;
using Db4objects.Db4o.Collections;
using Db4objects.Db4o.Tests.Common.TA;
using Db4objects.Db4o.Tests.Jre5.Collections;

namespace Db4objects.Db4o.Tests.Jre5.Collections
{
	public class Order : ActivatableImpl
	{
		private ArrayList4<OrderItem> _items;

		public Order()
		{
			_items = new ArrayList4<OrderItem>();
		}

		public virtual void AddItem(OrderItem item)
		{
			Activate(ActivationPurpose.Read);
			_items.Add(item);
		}

		public virtual OrderItem Item(int i)
		{
			Activate(ActivationPurpose.Read);
			return _items[i];
		}

		public virtual int Size()
		{
			Activate(ActivationPurpose.Read);
			return _items.Count;
		}
	}
}
#endif // !SILVERLIGHT
