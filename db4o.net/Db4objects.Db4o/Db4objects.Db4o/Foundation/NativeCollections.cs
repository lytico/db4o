/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	public class NativeCollections
	{
		public static IList Filter(IList items, IPredicate4 predicate)
		{
			IList filtered = new ArrayList();
			for (IEnumerator itemIter = items.GetEnumerator(); itemIter.MoveNext(); )
			{
				object item = itemIter.Current;
				if (predicate.Match(item))
				{
					filtered.Add(item);
				}
			}
			return filtered;
		}
	}
}
