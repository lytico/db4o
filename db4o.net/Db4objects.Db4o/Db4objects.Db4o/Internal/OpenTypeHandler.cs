/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Delete;
using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Internal.Handlers.Versions;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Internal
{
	public class OpenTypeHandler : IReferenceTypeHandler, IValueTypeHandler, IBuiltinTypeHandler
		, ICascadingTypeHandler, ILinkLengthAware
	{
		private const int Hashcode = 1003303143;

		private ObjectContainerBase _container;

		public OpenTypeHandler(ObjectContainerBase container)
		{
			_container = container;
		}

		internal virtual ObjectContainerBase Container()
		{
			return _container;
		}

		public virtual IReflectClass ClassReflector()
		{
			return Container().Handlers.IclassObject;
		}

		public virtual void CascadeActivation(IActivationContext context)
		{
			object targetObject = context.TargetObject();
			if (IsPlainObject(targetObject))
			{
				return;
			}
			ITypeHandler4 typeHandler = TypeHandlerForObject(targetObject);
			Handlers4.CascadeActivation(context, typeHandler);
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual void Delete(IDeleteContext context)
		{
			int payLoadOffset = context.ReadInt();
			if (context.IsLegacyHandlerVersion())
			{
				context.DefragmentRecommended();
				return;
			}
			if (payLoadOffset <= 0)
			{
				return;
			}
			int linkOffset = context.Offset();
			context.Seek(payLoadOffset);
			int classMetadataID = context.ReadInt();
			ITypeHandler4 typeHandler = Container().ClassMetadataForID(classMetadataID).TypeHandler
				();
			if (typeHandler != null)
			{
				if (!IsPlainObject(typeHandler))
				{
					context.Delete(typeHandler);
				}
			}
			context.Seek(linkOffset);
		}

		public virtual int GetID()
		{
			return Handlers4.UntypedId;
		}

		public virtual bool HasField(ObjectContainerBase a_stream, string a_path)
		{
			return a_stream.ClassCollection().FieldExists(a_path);
		}

		public virtual ITypeHandler4 ReadCandidateHandler(QueryingReadContext context)
		{
			int payLoadOffSet = context.ReadInt();
			if (payLoadOffSet == 0)
			{
				return null;
			}
			context.Seek(payLoadOffSet);
			int classMetadataID = context.ReadInt();
			ClassMetadata classMetadata = context.Container().ClassMetadataForID(classMetadataID
				);
			if (classMetadata == null)
			{
				return null;
			}
			return classMetadata.ReadCandidateHandler(context);
		}

		public virtual ObjectID ReadObjectID(IInternalReadContext context)
		{
			int payloadOffset = context.ReadInt();
			if (payloadOffset == 0)
			{
				return ObjectID.IsNull;
			}
			int savedOffset = context.Offset();
			ITypeHandler4 typeHandler = ReadTypeHandler(context, payloadOffset);
			if (typeHandler == null)
			{
				context.Seek(savedOffset);
				return ObjectID.IsNull;
			}
			SeekSecondaryOffset(context, typeHandler);
			if (typeHandler is IReadsObjectIds)
			{
				ObjectID readObjectID = ((IReadsObjectIds)typeHandler).ReadObjectID(context);
				context.Seek(savedOffset);
				return readObjectID;
			}
			context.Seek(savedOffset);
			return ObjectID.NotPossible;
		}

		public virtual void Defragment(IDefragmentContext context)
		{
			int payLoadOffSet = context.ReadInt();
			if (payLoadOffSet == 0)
			{
				return;
			}
			int savedOffSet = context.Offset();
			context.Seek(payLoadOffSet);
			try
			{
				int classMetadataId = context.CopyIDReturnOriginalID();
				ITypeHandler4 typeHandler = CorrectTypeHandlerVersionFor(context, classMetadataId
					);
				if (typeHandler == null)
				{
					return;
				}
				SeekSecondaryOffset(context, typeHandler);
				if (IsPlainObject(typeHandler))
				{
					context.Defragment(new PlainObjectHandler());
				}
				else
				{
					context.Defragment(typeHandler);
				}
			}
			finally
			{
				context.Seek(savedOffSet);
			}
		}

		protected virtual ITypeHandler4 CorrectTypeHandlerVersionFor(IDefragmentContext context
			, int classMetadataId)
		{
			ITypeHandler4 typeHandler = context.TypeHandlerForId(classMetadataId);
			if (null == typeHandler)
			{
				return null;
			}
			ClassMetadata classMetadata = Container(context).ClassMetadataForID(classMetadataId
				);
			return HandlerRegistry.CorrectHandlerVersion(context, typeHandler, classMetadata);
		}

		protected virtual ObjectContainerBase Container(IDefragmentContext context)
		{
			return context.Transaction().Container();
		}

		protected virtual ITypeHandler4 ReadTypeHandler(IInternalReadContext context, int
			 payloadOffset)
		{
			context.Seek(payloadOffset);
			ITypeHandler4 typeHandler = Container().TypeHandlerForClassMetadataID(context.ReadInt
				());
			return HandlerRegistry.CorrectHandlerVersion(context, typeHandler);
		}

		/// <param name="buffer"></param>
		/// <param name="typeHandler"></param>
		protected virtual void SeekSecondaryOffset(IReadBuffer buffer, ITypeHandler4 typeHandler
			)
		{
		}

		// do nothing, no longer needed in current implementation.
		public virtual object Read(IReadContext readContext)
		{
			IInternalReadContext context = (IInternalReadContext)readContext;
			int payloadOffset = context.ReadInt();
			if (payloadOffset == 0)
			{
				context.NotifyNullReferenceSkipped();
				return null;
			}
			int savedOffSet = context.Offset();
			try
			{
				ITypeHandler4 typeHandler = ReadTypeHandler(context, payloadOffset);
				if (typeHandler == null)
				{
					return null;
				}
				SeekSecondaryOffset(context, typeHandler);
				if (IsPlainObject(typeHandler))
				{
					return context.ReadAtCurrentSeekPosition(new PlainObjectHandler());
				}
				return context.ReadAtCurrentSeekPosition(typeHandler);
			}
			finally
			{
				context.Seek(savedOffSet);
			}
		}

		public virtual void Activate(IReferenceActivationContext context)
		{
		}

		//    	throw new IllegalStateException();
		public virtual void CollectIDs(QueryingReadContext readContext)
		{
			IInternalReadContext context = (IInternalReadContext)readContext;
			int payloadOffset = context.ReadInt();
			if (payloadOffset == 0)
			{
				return;
			}
			int savedOffSet = context.Offset();
			try
			{
				ITypeHandler4 typeHandler = ReadTypeHandler(context, payloadOffset);
				if (typeHandler == null)
				{
					return;
				}
				SeekSecondaryOffset(context, typeHandler);
				if (IsPlainObject(typeHandler))
				{
					readContext.Collector().AddId(readContext.ReadInt());
					return;
				}
				CollectIdContext collectIdContext = new _CollectIdContext_203(readContext, readContext
					.Transaction(), readContext.Collector(), null, readContext.Buffer());
				Handlers4.CollectIdsInternal(collectIdContext, context.Container().Handlers.CorrectHandlerVersion
					(typeHandler, context.HandlerVersion()), 0, false);
			}
			finally
			{
				context.Seek(savedOffSet);
			}
		}

		private sealed class _CollectIdContext_203 : CollectIdContext
		{
			public _CollectIdContext_203(QueryingReadContext readContext, Transaction baseArg1
				, IdObjectCollector baseArg2, ObjectHeader baseArg3, IReadBuffer baseArg4) : base
				(baseArg1, baseArg2, baseArg3, baseArg4)
			{
				this.readContext = readContext;
			}

			public override int HandlerVersion()
			{
				return readContext.HandlerVersion();
			}

			public override SlotFormat SlotFormat()
			{
				return new _SlotFormatCurrent_209();
			}

			private sealed class _SlotFormatCurrent_209 : SlotFormatCurrent
			{
				public _SlotFormatCurrent_209()
				{
				}

				public override bool IsIndirectedWithinSlot(ITypeHandler4 handler)
				{
					return false;
				}
			}

			private readonly QueryingReadContext readContext;
		}

		public virtual ITypeHandler4 ReadTypeHandlerRestoreOffset(IInternalReadContext context
			)
		{
			int savedOffset = context.Offset();
			int payloadOffset = context.ReadInt();
			ITypeHandler4 typeHandler = payloadOffset == 0 ? null : ReadTypeHandler(context, 
				payloadOffset);
			context.Seek(savedOffset);
			return typeHandler;
		}

		public virtual void Write(IWriteContext context, object obj)
		{
			if (obj == null)
			{
				context.WriteInt(0);
				return;
			}
			MarshallingContext marshallingContext = (MarshallingContext)context;
			ClassMetadata classMetadata = ClassMetadataFor(obj);
			if (classMetadata == null)
			{
				context.WriteInt(0);
				return;
			}
			MarshallingContextState state = marshallingContext.CurrentState();
			marshallingContext.CreateChildBuffer(false);
			context.WriteInt(classMetadata.GetID());
			WriteObject(context, classMetadata.TypeHandler(), obj);
			marshallingContext.RestoreState(state);
		}

		private ClassMetadata ClassMetadataFor(object obj)
		{
			return Container().ClassMetadataForObject(obj);
		}

		private void WriteObject(IWriteContext context, ITypeHandler4 typeHandler, object
			 obj)
		{
			if (IsPlainObject(obj))
			{
				context.WriteObject(new PlainObjectHandler(), obj);
				return;
			}
			if (Handlers4.UseDedicatedSlot(context, typeHandler))
			{
				context.WriteObject(obj);
			}
			else
			{
				typeHandler.Write(context, obj);
			}
		}

		private bool IsPlainObject(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			return obj.GetType() == Const4.ClassObject;
		}

		public static bool IsPlainObject(ITypeHandler4 typeHandler)
		{
			return typeHandler.GetType() == typeof(Db4objects.Db4o.Internal.OpenTypeHandler) 
				|| typeHandler.GetType() == typeof(OpenTypeHandler0) || typeHandler.GetType() ==
				 typeof(OpenTypeHandler2) || typeHandler.GetType() == typeof(OpenTypeHandler7);
		}

		public virtual ITypeHandler4 TypeHandlerForObject(object obj)
		{
			return ClassMetadataFor(obj).TypeHandler();
		}

		public override bool Equals(object obj)
		{
			return obj is Db4objects.Db4o.Internal.OpenTypeHandler && !(obj is InterfaceTypeHandler
				);
		}

		public override int GetHashCode()
		{
			return Hashcode;
		}

		public virtual void RegisterReflector(IReflector reflector)
		{
		}

		// nothing to do
		public virtual int LinkLength()
		{
			return Const4.IdLength;
		}
	}
}
