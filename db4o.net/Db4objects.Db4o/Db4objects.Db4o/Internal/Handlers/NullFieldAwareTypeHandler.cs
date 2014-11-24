/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Delete;
using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Internal.Handlers
{
	/// <exclude></exclude>
	public class NullFieldAwareTypeHandler : IFieldAwareTypeHandler
	{
		public static readonly IFieldAwareTypeHandler Instance = new NullFieldAwareTypeHandler
			();

		public virtual void AddFieldIndices(ObjectIdContextImpl context)
		{
		}

		public virtual void ClassMetadata(Db4objects.Db4o.Internal.ClassMetadata classMetadata
			)
		{
		}

		public virtual void CollectIDs(CollectIdContext context, IPredicate4 predicate)
		{
		}

		public virtual void DeleteMembers(DeleteContextImpl deleteContext, bool isUpdate)
		{
		}

		public virtual void ReadVirtualAttributes(ObjectReferenceContext context)
		{
		}

		public virtual bool SeekToField(ObjectHeaderContext context, ClassAspect aspect)
		{
			return false;
		}

		public virtual void Defragment(IDefragmentContext context)
		{
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual void Delete(IDeleteContext context)
		{
		}

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

		public virtual ITypeHandler4 UnversionedTemplate()
		{
			return null;
		}

		public virtual object DeepClone(object context)
		{
			return null;
		}

		public virtual void CascadeActivation(IActivationContext context)
		{
		}

		public virtual void CollectIDs(QueryingReadContext context)
		{
		}

		public virtual ITypeHandler4 ReadCandidateHandler(QueryingReadContext context)
		{
			return null;
		}
	}
}
