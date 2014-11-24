/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Handlers.Array;

namespace Db4objects.Db4o.Internal.Handlers.Array
{
	/// <exclude></exclude>
	public class ArrayVersionHelper0 : ArrayVersionHelper3
	{
		public override bool IsPreVersion0Format(int elementCount)
		{
			return elementCount >= 0;
		}
	}
}
