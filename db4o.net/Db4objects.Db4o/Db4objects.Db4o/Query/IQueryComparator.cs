/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Query
{
	/// <summary>
	/// This interface is not used in .NET.
	/// </summary>
	public interface IQueryComparator
	{
		/// <summary>Implement to compare two arguments for sorting.</summary>
		/// <remarks>
		/// Implement to compare two arguments for sorting.
		/// Return a negative value, zero, or a positive value if
		/// the first argument is smaller, equal or greater than
		/// the second.
		/// </remarks>
		int Compare(object first, object second);
	}
}
