/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Delete;
using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Internal.Handlers
{
	/// <exclude></exclude>
	public interface IFieldAwareTypeHandler : IReferenceTypeHandler, IVersionedTypeHandler
		, ICascadingTypeHandler, IVirtualAttributeHandler
	{
		void AddFieldIndices(ObjectIdContextImpl context);

		void CollectIDs(CollectIdContext context, IPredicate4 predicate);

		void DeleteMembers(DeleteContextImpl deleteContext, bool isUpdate);

		void ReadVirtualAttributes(ObjectReferenceContext context);

		void ClassMetadata(Db4objects.Db4o.Internal.ClassMetadata classMetadata);

		bool SeekToField(ObjectHeaderContext context, ClassAspect aspect);
	}
}
