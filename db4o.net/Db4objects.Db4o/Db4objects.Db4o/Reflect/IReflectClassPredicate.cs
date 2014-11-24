/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Reflect
{
	/// <summary>Predicate representation.</summary>
	/// <remarks>Predicate representation.</remarks>
	/// <seealso cref="Db4objects.Db4o.Query.Predicate">Db4objects.Db4o.Query.Predicate</seealso>
	/// <seealso cref="IReflector">IReflector</seealso>
	public interface IReflectClassPredicate
	{
		/// <summary>Match method definition.</summary>
		/// <remarks>
		/// Match method definition. Used to select correct
		/// results from an object set.
		/// </remarks>
		/// <param name="item">item to be matched to the criteria</param>
		/// <returns>true, if the requirements are met</returns>
		bool Match(IReflectClass item);
	}
}
