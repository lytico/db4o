/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Tests.Common.TA;
using Db4objects.Db4o.Tests.Common.TA.Mixed;

namespace Db4objects.Db4o.Tests.Common.TA.Mixed
{
	public class MixedArrayItem
	{
		public object[] objects;

		public MixedArrayItem()
		{
		}

		public MixedArrayItem(int v)
		{
			objects = new object[4];
			objects[0] = LinkedList.NewList(v);
			objects[1] = new TItem(v);
			objects[2] = LinkedList.NewList(v);
			objects[3] = new TItem(v);
		}
	}
}
