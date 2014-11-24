/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Delete;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Internal.Query.Processor;
using Db4objects.Db4o.Internal.Replication;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Internal
{
	/// <summary>
	/// TODO: refactor for symmetric inheritance - don't inherit from YapField and override,
	/// instead extract an abstract superclass from YapField and let both YapField and this class implement
	/// </summary>
	/// <exclude></exclude>
	public abstract class VirtualFieldMetadata : FieldMetadata
	{
		private static readonly object AnyObject = new object();

		private IReflectClass _classReflector;

		private IBuiltinTypeHandler _handler;

		internal VirtualFieldMetadata(int fieldTypeID, IBuiltinTypeHandler handler) : base
			(fieldTypeID)
		{
			_handler = handler;
		}

		public override ITypeHandler4 GetHandler()
		{
			return _handler;
		}

		/// <exception cref="Db4objects.Db4o.Internal.FieldIndexException"></exception>
		public abstract override void AddFieldIndex(ObjectIdContextImpl context);

		public override bool Alive()
		{
			return true;
		}

		internal override bool CanAddToQuery(string fieldName)
		{
			return fieldName.Equals(GetName());
		}

		public override bool CanBeDisabled()
		{
			return false;
		}

		public override bool CanUseNullBitmap()
		{
			return false;
		}

		public virtual IReflectClass ClassReflector(IReflector reflector)
		{
			if (_classReflector == null)
			{
				_classReflector = ((IBuiltinTypeHandler)GetHandler()).ClassReflector();
			}
			return _classReflector;
		}

		internal override void CollectConstraints(Transaction a_trans, QConObject a_parent
			, object a_template, IVisitor4 a_visitor)
		{
		}

		// QBE constraint collection call
		// There isn't anything useful to do here, since virtual fields
		// are not on the actual object.
		public override void Deactivate(IActivationContext context)
		{
		}

		// do nothing
		public abstract override void Delete(DeleteContextImpl context, bool isUpdate);

		public override object GetOrCreate(Transaction a_trans, object a_OnObject)
		{
			// This is the first part of marshalling
			// Virtual fields do it all in #marshall(), the object is never used.
			// Returning any object here prevents triggering null handling.
			return AnyObject;
		}

		public override bool NeedsArrayAndPrimitiveInfo()
		{
			return false;
		}

		public override void Activate(UnmarshallingContext context)
		{
			context.ObjectReference().ProduceVirtualAttributes();
			Instantiate1(context);
		}

		internal abstract void Instantiate1(ObjectReferenceContext context);

		public override void LoadFieldTypeById()
		{
		}

		// do nothing
		public override void Marshall(MarshallingContext context, object obj)
		{
			Marshall(context.Transaction(), context.Reference(), context, context.IsNew());
		}

		private void Marshall(Transaction trans, ObjectReference @ref, IWriteBuffer buffer
			, bool isNew)
		{
			if (!trans.SupportsVirtualFields())
			{
				MarshallIgnore(buffer);
				return;
			}
			ObjectContainerBase stream = trans.Container();
			HandlerRegistry handlers = stream._handlers;
			bool migrating = false;
			if (stream._replicationCallState == Const4.New)
			{
				IDb4oReplicationReferenceProvider provider = handlers._replicationReferenceProvider;
				object parentObject = @ref.GetObject();
				IDb4oReplicationReference replicationReference = provider.ReferenceFor(parentObject
					);
				if (replicationReference != null)
				{
					migrating = true;
					VirtualAttributes va = @ref.ProduceVirtualAttributes();
					va.i_version = replicationReference.Version();
					va.i_uuid = replicationReference.LongPart();
					va.i_database = replicationReference.SignaturePart();
				}
			}
			if (@ref.VirtualAttributes() == null)
			{
				@ref.ProduceVirtualAttributes();
				migrating = false;
			}
			Marshall(trans, @ref, buffer, migrating, isNew);
		}

		internal abstract void Marshall(Transaction trans, ObjectReference @ref, IWriteBuffer
			 buffer, bool migrating, bool isNew);

		internal abstract void MarshallIgnore(IWriteBuffer writer);

		public virtual void ReadVirtualAttribute(ObjectReferenceContext context)
		{
			if (!context.Transaction().SupportsVirtualFields())
			{
				IncrementOffset(context, context);
				return;
			}
			Instantiate1(context);
		}

		public override bool IsVirtual()
		{
			return true;
		}

		protected override object IndexEntryFor(object indexEntry)
		{
			return indexEntry;
		}

		protected override IIndexable4 IndexHandler(ObjectContainerBase stream)
		{
			return (IIndexable4)GetHandler();
		}
	}
}
