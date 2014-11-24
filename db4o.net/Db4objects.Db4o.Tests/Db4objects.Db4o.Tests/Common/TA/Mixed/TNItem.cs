/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Activation;
using Db4objects.Db4o.Tests.Common.TA;

namespace Db4objects.Db4o.Tests.Common.TA.Mixed
{
	/// <exclude></exclude>
	public class TNItem : ActivatableImpl
	{
		public LinkedList list;

		public TNItem()
		{
		}

		public TNItem(int v)
		{
			list = LinkedList.NewList(v);
		}

		public virtual LinkedList Value()
		{
			Activate(ActivationPurpose.Read);
			return list;
		}
	}
}
