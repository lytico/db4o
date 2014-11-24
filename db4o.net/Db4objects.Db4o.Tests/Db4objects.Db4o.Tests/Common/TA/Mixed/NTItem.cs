/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Tests.Common.TA.Mixed;

namespace Db4objects.Db4o.Tests.Common.TA.Mixed
{
	/// <exclude></exclude>
	public class NTItem
	{
		public TItem tItem;

		public NTItem()
		{
		}

		public NTItem(int value)
		{
			tItem = new TItem(value);
		}
	}
}
