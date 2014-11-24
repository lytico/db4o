/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Marshall;

namespace Db4objects.Db4o.Internal.Handlers
{
	public class FloatHandler0 : FloatHandler
	{
		public override object Read(IReadContext context)
		{
			float value = (float)base.Read(context);
			if (float.IsNaN(value))
			{
				return null;
			}
			return value;
		}
	}
}
