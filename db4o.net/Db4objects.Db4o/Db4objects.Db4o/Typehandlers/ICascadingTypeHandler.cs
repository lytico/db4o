/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Typehandlers
{
	/// <summary>TypeHandler for objects with members.</summary>
	/// <remarks>TypeHandler for objects with members.</remarks>
	public interface ICascadingTypeHandler : ITypeHandler4
	{
		/// <summary>
		/// will be called during activation if the handled
		/// object is already active
		/// </summary>
		/// <param name="context"></param>
		void CascadeActivation(IActivationContext context);

		/// <summary>
		/// will be called during querying to ask for the handler
		/// to be used to collect children of the handled object
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		ITypeHandler4 ReadCandidateHandler(QueryingReadContext context);

		/// <summary>
		/// will be called during querying to ask for IDs of member
		/// objects of the handled object.
		/// </summary>
		/// <remarks>
		/// will be called during querying to ask for IDs of member
		/// objects of the handled object.
		/// </remarks>
		/// <param name="context"></param>
		void CollectIDs(QueryingReadContext context);
	}
}
