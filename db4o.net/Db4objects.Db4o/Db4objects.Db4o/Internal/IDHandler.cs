/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Handlers;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public class IDHandler : IntHandler
	{
		public override void DefragIndexEntry(DefragmentContextImpl context)
		{
			int sourceId = context.CopyIDReturnOriginalID(true);
			context.CurrentParentSourceID(sourceId);
		}
	}
}
