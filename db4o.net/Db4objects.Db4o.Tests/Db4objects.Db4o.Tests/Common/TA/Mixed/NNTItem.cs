/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Tests.Common.TA.Mixed;

namespace Db4objects.Db4o.Tests.Common.TA.Mixed
{
	/// <exclude></exclude>
	public class NNTItem
	{
		public NTItem ntItem;

		public NNTItem()
		{
		}

		public NNTItem(int v)
		{
			ntItem = new NTItem(v);
		}
	}
}
