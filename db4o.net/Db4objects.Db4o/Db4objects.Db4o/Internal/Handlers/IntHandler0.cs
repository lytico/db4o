/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Marshall;

namespace Db4objects.Db4o.Internal.Handlers
{
	/// <exclude></exclude>
	public class IntHandler0 : IntHandler
	{
		public override object Read(IReadContext context)
		{
			int i = context.ReadInt();
			if (i == int.MaxValue)
			{
				return null;
			}
			return i;
		}
	}
}
