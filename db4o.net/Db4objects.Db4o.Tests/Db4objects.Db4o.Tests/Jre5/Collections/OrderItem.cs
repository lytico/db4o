/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Activation;
using Db4objects.Db4o.Tests.Common.TA;

namespace Db4objects.Db4o.Tests.Jre5.Collections
{
	public class OrderItem : ActivatableImpl
	{
		private Db4objects.Db4o.Tests.Jre5.Collections.Product _product;

		private int _quantity;

		public OrderItem(Db4objects.Db4o.Tests.Jre5.Collections.Product product, int quantity
			)
		{
			_product = product;
			_quantity = quantity;
		}

		public virtual Db4objects.Db4o.Tests.Jre5.Collections.Product Product()
		{
			Activate(ActivationPurpose.Read);
			return _product;
		}

		public virtual int Quantity()
		{
			Activate(ActivationPurpose.Read);
			return _quantity;
		}
	}
}
