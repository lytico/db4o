/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Handlers.Array;
using Db4objects.Db4o.Marshall;

namespace Db4objects.Db4o.Internal.Handlers.Array
{
	/// <exclude></exclude>
	public class ArrayHandler1 : ArrayHandler3
	{
		protected override bool HandleAsByteArray(IBufferContext context)
		{
			return false;
		}

		protected override bool HandleAsByteArray(object obj)
		{
			return false;
		}
	}
}
