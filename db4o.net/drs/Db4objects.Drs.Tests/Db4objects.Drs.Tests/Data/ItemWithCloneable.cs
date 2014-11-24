/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;

namespace Db4objects.Drs.Tests.Data
{
	public sealed class ItemWithCloneable
	{
		public ICloneable value;

		public ItemWithCloneable()
		{
		}

		public ItemWithCloneable(ICloneable c)
		{
			value = c;
		}
	}
}
