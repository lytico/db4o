/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Foundation
{
	/// <exclude></exclude>
	public interface IComparison4
	{
		/// <summary>
		/// Returns negative number if x &lt; y
		/// Returns zero if x == y
		/// Returns positive number if x &gt; y
		/// </summary>
		int Compare(object x, object y);
	}
}
