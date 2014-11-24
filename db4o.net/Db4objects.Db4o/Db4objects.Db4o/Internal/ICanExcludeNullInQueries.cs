/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Internal
{
	/// <summary>
	/// marker interface for Indexable4, if a check for null is necessary
	/// in BTreeRangeSingle#firstBTreePointer()
	/// </summary>
	/// <exclude></exclude>
	public interface ICanExcludeNullInQueries
	{
		bool ExcludeNull();
	}
}
