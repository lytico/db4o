/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Marshall;

namespace Db4objects.Db4o.Internal.Handlers
{
	public class LongHandler0 : LongHandler
	{
		public override object Read(IReadContext context)
		{
			long value = (long)base.Read(context);
			if (value == long.MaxValue)
			{
				return null;
			}
			return value;
		}
	}
}
