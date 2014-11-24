/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Foundation
{
	/// <summary>
	/// a prepared comparison, to compare multiple objects
	/// with one single object.
	/// </summary>
	/// <remarks>
	/// a prepared comparison, to compare multiple objects
	/// with one single object.
	/// </remarks>
	public interface IPreparedComparison
	{
		/// <summary>
		/// return a negative int, zero or a positive int if
		/// the object being held in 'this' is smaller, equal
		/// or greater than the passed object.<br /><br />
		/// Typical implementation: return this.object - obj;
		/// </summary>
		int CompareTo(object obj);
	}
}
