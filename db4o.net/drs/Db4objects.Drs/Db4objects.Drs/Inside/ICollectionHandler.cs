/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Reflect;
using Db4objects.Drs.Inside;
using Db4objects.Drs.Inside.Traversal;

namespace Db4objects.Drs.Inside
{
	public interface ICollectionHandler : ICollectionFlattener
	{
		object EmptyClone(ICollectionSource sourceProvider, object originalCollection, IReflectClass
			 originalCollectionClass);

		void CopyState(object original, object dest, ICounterpartFinder finder);

		object CloneWithCounterparts(ICollectionSource sourceProvider, object original, IReflectClass
			 claxx, ICounterpartFinder elementCloner);
	}
}
