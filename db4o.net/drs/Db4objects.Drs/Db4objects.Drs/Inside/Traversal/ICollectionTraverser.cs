/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Reflect;

namespace Db4objects.Drs.Inside.Traversal
{
	public interface ICollectionTraverser
	{
		bool CanHandleClass(IReflectClass claxx);

		IEnumerator IteratorFor(object collection);
	}
}
