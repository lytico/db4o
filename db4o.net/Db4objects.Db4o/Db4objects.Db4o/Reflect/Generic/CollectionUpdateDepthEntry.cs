/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Reflect.Generic
{
	internal class CollectionUpdateDepthEntry
	{
		internal readonly IReflectClassPredicate _predicate;

		internal readonly int _depth;

		internal CollectionUpdateDepthEntry(IReflectClassPredicate predicate, int depth)
		{
			_predicate = predicate;
			_depth = depth;
		}
	}
}
