/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Handlers.Array;
using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Internal.Handlers.Array
{
	/// <exclude></exclude>
	public class ArrayVersionHelper5 : ArrayVersionHelper
	{
		public override bool HasNullBitmap(ArrayInfo info)
		{
			return !info.Primitive();
		}
	}
}
