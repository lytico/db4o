/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Activation;

namespace Db4objects.Db4o.Internal.Activation
{
	public class TPUpdateDepthProvider : IUpdateDepthProvider
	{
		public virtual FixedUpdateDepth ForDepth(int depth)
		{
			return new TPFixedUpdateDepth(depth, NullModifiedObjectQuery.Instance);
		}

		public virtual UnspecifiedUpdateDepth Unspecified(IModifiedObjectQuery query)
		{
			return new TPUnspecifiedUpdateDepth(query);
		}
	}
}
