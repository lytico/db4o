/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Handlers.Array;

namespace Db4objects.Db4o.Internal.Handlers.Array
{
	/// <exclude></exclude>
	public class ArrayHandler5 : ArrayHandler
	{
		protected override ArrayVersionHelper CreateVersionHelper()
		{
			return new ArrayVersionHelper5();
		}
	}
}
