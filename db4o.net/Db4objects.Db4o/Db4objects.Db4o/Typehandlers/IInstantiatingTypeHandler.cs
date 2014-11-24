/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Typehandlers
{
	public interface IInstantiatingTypeHandler : IReferenceTypeHandler
	{
		object Instantiate(IReadContext context);

		/// <summary>gets called when an object is to be written to the database.</summary>
		/// <remarks>
		/// gets called when an object is to be written to the database.
		/// The method must only write data necessary to re instantiate the object, usually
		/// the immutable bits of information held by the object. For value
		/// types that means their complete state.
		/// Mutable state (only allowed in reference types) must be handled
		/// during
		/// <see cref="IReferenceTypeHandler.Activate(Db4objects.Db4o.Marshall.IReferenceActivationContext)
		/// 	">IReferenceTypeHandler.Activate(Db4objects.Db4o.Marshall.IReferenceActivationContext)
		/// 	</see>
		/// </remarks>
		/// <param name="context"></param>
		/// <param name="obj">the object</param>
		void WriteInstantiation(IWriteContext context, object obj);
	}
}
