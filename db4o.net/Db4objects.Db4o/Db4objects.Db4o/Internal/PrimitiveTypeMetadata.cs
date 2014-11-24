/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Delete;
using Db4objects.Db4o.Internal.Handlers.Array;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Internal.Metadata;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public class PrimitiveTypeMetadata : ClassMetadata
	{
		private const int HashcodeForNull = 283636383;

		public PrimitiveTypeMetadata(ObjectContainerBase container, ITypeHandler4 handler
			, int id, IReflectClass classReflector) : base(container, classReflector)
		{
			_aspects = FieldMetadata.EmptyArray;
			_typeHandler = handler;
			_id = id;
		}

		public PrimitiveTypeMetadata(ObjectContainerBase container) : base(container)
		{
			_typeHandler = null;
		}

		public override void CascadeActivation(IActivationContext context)
		{
		}

		// Override
		// do nothing
		internal sealed override void AddToIndex(Transaction trans, int id)
		{
		}

		// Override
		// Primitive Indices will be created later.
		internal override bool AllowsQueries()
		{
			return false;
		}

		internal override void CacheDirty(Collection4 col)
		{
		}

		// do nothing
		public override bool DescendOnCascadingActivation()
		{
			return false;
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override void Delete(IDeleteContext context)
		{
			if (context.IsLegacyHandlerVersion())
			{
				context.ReadInt();
				context.DefragmentRecommended();
			}
		}

		internal override void DeleteMembers(DeleteContextImpl context, ArrayType arrayType
			, bool isUpdate)
		{
			if (arrayType == ArrayType.PlainArray)
			{
				new ArrayHandler(TypeHandler(), true).DeletePrimitiveEmbedded((StatefulBuffer)context
					.Buffer(), this);
			}
			else
			{
				if (arrayType == ArrayType.MultidimensionalArray)
				{
					new MultidimensionalArrayHandler(TypeHandler(), true).DeletePrimitiveEmbedded((StatefulBuffer
						)context.Buffer(), this);
				}
			}
		}

		public override bool HasClassIndex()
		{
			return false;
		}

		public override object Instantiate(UnmarshallingContext context)
		{
			object obj = context.PersistentObject();
			if (obj == null)
			{
				obj = context.Read(TypeHandler());
				context.SetObjectWeak(obj);
			}
			context.SetStateClean();
			return obj;
		}

		public override object InstantiateTransient(UnmarshallingContext context)
		{
			return Handlers4.ReadValueType(context, CorrectHandlerVersion(context));
		}

		internal override void InstantiateFields(UnmarshallingContext context)
		{
			throw new NotImplementedException();
		}

		public override bool IsArray()
		{
			return _id == Handlers4.AnyArrayId || _id == Handlers4.AnyArrayNId;
		}

		public override bool HasIdentity()
		{
			return false;
		}

		public override bool IsStronglyTyped()
		{
			return false;
		}

		public override IPreparedComparison PrepareComparison(IContext context, object source
			)
		{
			return Handlers4.PrepareComparisonFor(TypeHandler(), context, source);
		}

		public override ITypeHandler4 ReadCandidateHandler(QueryingReadContext context)
		{
			if (IsArray())
			{
				return TypeHandler();
			}
			return null;
		}

		//	@Override
		//    public ObjectID readObjectID(InternalReadContext context){
		//        if(_handler instanceof ClassMetadata){
		//            return ((ClassMetadata)_handler).readObjectID(context);
		//        }
		//        if(Handlers4.handlesArray(_handler)){
		//            // TODO: Here we should theoretically read through the array and collect candidates.
		//            // The respective construct is wild: "Contains query through an array in an array."
		//            // Ignore for now.
		//            return ObjectID.IGNORE;
		//        }
		//        return ObjectID.NOT_POSSIBLE;
		//    }
		internal override void RemoveFromIndex(Transaction ta, int id)
		{
		}

		// do nothing
		public sealed override bool WriteObjectBegin()
		{
			return false;
		}

		public override string ToString()
		{
			return GetType().FullName + "(" + TypeHandler() + ")";
		}

		public override void Defragment(IDefragmentContext context)
		{
			CorrectHandlerVersion(context).Defragment(context);
		}

		public override object WrapWithTransactionContext(Transaction transaction, object
			 value)
		{
			return value;
		}

		public override ITypeHandler4 DelegateTypeHandler(IContext context)
		{
			return TypeHandler();
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Db4objects.Db4o.Internal.PrimitiveTypeMetadata))
			{
				return false;
			}
			Db4objects.Db4o.Internal.PrimitiveTypeMetadata other = (Db4objects.Db4o.Internal.PrimitiveTypeMetadata
				)obj;
			if (TypeHandler() == null)
			{
				return other.TypeHandler() == null;
			}
			return TypeHandler().Equals(other.TypeHandler());
		}

		public override int GetHashCode()
		{
			if (TypeHandler() == null)
			{
				return HashcodeForNull;
			}
			return TypeHandler().GetHashCode();
		}

		public virtual object DeepClone(object context)
		{
			throw new InvalidOperationException();
		}

		protected override IAspectTraversalStrategy DetectAspectTraversalStrategy()
		{
			return new _IAspectTraversalStrategy_178();
		}

		private sealed class _IAspectTraversalStrategy_178 : IAspectTraversalStrategy
		{
			public _IAspectTraversalStrategy_178()
			{
			}

			public void TraverseAllAspects(ITraverseAspectCommand command)
			{
			}
		}
		// do nothing
	}
}
