/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Foundation
{
	/// <summary>
	/// Non boxing/unboxing version of
	/// <see cref="System.Collections.IComparer{T}">System.Collections.IComparer&lt;T&gt;
	/// 	</see>
	/// for
	/// faster id comparisons.
	/// </summary>
	public interface IIntComparator
	{
		int Compare(int x, int y);
	}
}
