/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Marshall;

namespace Db4objects.Db4o.Internal
{
	/// <summary>Interface for comparison support in queries.</summary>
	/// <remarks>Interface for comparison support in queries.</remarks>
	public interface IComparable4
	{
		/// <summary>
		/// creates a prepared comparison to compare multiple objects
		/// against one single object.
		/// </summary>
		/// <remarks>
		/// creates a prepared comparison to compare multiple objects
		/// against one single object.
		/// </remarks>
		/// <param name="context">the context of the comparison</param>
		/// <param name="obj">
		/// the object that is to be compared
		/// against multiple other objects
		/// </param>
		/// <returns>the prepared comparison</returns>
		IPreparedComparison PrepareComparison(IContext context, object obj);
	}
}
