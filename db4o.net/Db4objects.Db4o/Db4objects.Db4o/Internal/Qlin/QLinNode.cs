/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;
using Db4objects.Db4o.Qlin;

namespace Db4objects.Db4o.Internal.Qlin
{
	/// <exclude></exclude>
	public abstract class QLinNode : IQLin
	{
		public virtual IQLin Equal(object obj)
		{
			throw new QLinException("#equal() is not supported on this node");
		}

		public virtual IQLin StartsWith(string @string)
		{
			throw new QLinException("#startsWith() is not supported on this node");
		}

		public virtual IQLin Smaller(object obj)
		{
			throw new QLinException("#smaller() is not supported on this node");
		}

		public virtual IQLin Greater(object obj)
		{
			throw new QLinException("#greater() is not supported on this node");
		}

		public virtual object SingleOrDefault(object defaultValue)
		{
			IObjectSet collection = Select();
			// TODO: Change to #isEmpty here after decafs, so the size doesn#t need to be calculated
			if (collection.Count == 0)
			{
				return defaultValue;
			}
			if (collection.Count > 1)
			{
				// Consider: Use a more specific exception if a query does not return
				//           the expected result
				throw new QLinException("Expected one or none. Found: " + collection.Count);
			}
			// The following would be the right way to work against
			// a collection but for now it won't decaf.
			// return collection.iterator().next();
			// This is the ugly old db4o interface, where a Collection is
			// an iterator directly. For now it's convenient but we don't
			// really want to use this in the future.
			// Update #single() in the same way.
			return collection.Next();
		}

		public virtual object Single()
		{
			IObjectSet collection = Select();
			if (collection.Count != 1)
			{
				throw new QLinException("Expected exactly one. Found: " + collection.Count);
			}
			return collection.Next();
		}

		public abstract IQLin Limit(int arg1);

		public abstract IQLin OrderBy(object arg1, QLinOrderByDirection arg2);

		public abstract IObjectSet Select();

		public abstract IQLin Where(object arg1);
	}
}
