/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Tests.Common.Handlers;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	internal class Post7_1ObjectContainerAdapter : AbstractObjectContainerAdapter
	{
		public override void Store(object obj)
		{
			db.Store(obj);
		}

		public override void Store(object obj, int depth)
		{
			db.Store(obj, depth);
		}
	}
}
