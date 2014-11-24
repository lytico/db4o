/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Delete;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Typehandlers
{
	/// <summary>Typehandler that ignores all fields on a class</summary>
	public class IgnoreFieldsTypeHandler : IReferenceTypeHandler, ICascadingTypeHandler
	{
		public static readonly ITypeHandler4 Instance = new Db4objects.Db4o.Typehandlers.IgnoreFieldsTypeHandler
			();

		private IgnoreFieldsTypeHandler()
		{
		}

		public virtual void Defragment(IDefragmentContext context)
		{
		}

		// do nothing
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual void Delete(IDeleteContext context)
		{
		}

		// do nothing
		public virtual void Activate(IReferenceActivationContext context)
		{
		}

		public virtual void Write(IWriteContext context, object obj)
		{
		}

		public virtual IPreparedComparison PrepareComparison(IContext context, object obj
			)
		{
			return null;
		}

		public virtual void CascadeActivation(IActivationContext context)
		{
		}

		// do nothing
		public virtual void CollectIDs(QueryingReadContext context)
		{
		}

		// do nothing
		public virtual ITypeHandler4 ReadCandidateHandler(QueryingReadContext context)
		{
			return null;
		}
	}
}
