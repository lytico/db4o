/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Internal.Slots;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Internal.Handlers.Versions
{
	public class OpenTypeHandler7 : OpenTypeHandler
	{
		public OpenTypeHandler7(ObjectContainerBase container) : base(container)
		{
		}

		public override object Read(IReadContext readContext)
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
				if (IsPlainObject(typeHandler))
				{
					return ReadPlainObject(readContext);
				}
				SeekSecondaryOffset(context, typeHandler);
				return context.ReadAtCurrentSeekPosition(typeHandler);
			}
			finally
			{
				context.Seek(savedOffSet);
			}
		}

		public override void Defragment(IDefragmentContext context)
		{
			int payLoadOffSet = context.ReadInt();
			if (payLoadOffSet == 0)
			{
				return;
			}
			int savedOffSet = context.Offset();
			context.Seek(payLoadOffSet);
			int classMetadataId = context.CopyIDReturnOriginalID();
			ITypeHandler4 typeHandler = CorrectTypeHandlerVersionFor(context, classMetadataId
				);
			if (typeHandler != null)
			{
				if (IsPlainObject(typeHandler))
				{
					context.CopySlotlessID();
				}
				else
				{
					SeekSecondaryOffset(context, typeHandler);
					context.Defragment(typeHandler);
				}
			}
			context.Seek(savedOffSet);
		}

		private object ReadPlainObject(IReadContext context)
		{
			int id = context.ReadInt();
			Transaction transaction = context.Transaction();
			object obj = transaction.ObjectForIdFromCache(id);
			if (obj != null)
			{
				return obj;
			}
			obj = new object();
			AddReference(context, obj, id);
			return obj;
		}

		private void AddReference(IContext context, object obj, int id)
		{
			Transaction transaction = context.Transaction();
			ObjectReference @ref = new _ObjectReference_74(id);
			@ref.ClassMetadata(transaction.Container().ClassMetadataForID(Handlers4.UntypedId
				));
			@ref.SetObjectWeak(transaction.Container(), obj);
			transaction.AddNewReference(@ref);
		}

		private sealed class _ObjectReference_74 : ObjectReference
		{
			public _ObjectReference_74(int baseArg1) : base(baseArg1)
			{
				this._firstUpdate = true;
			}

			internal bool _firstUpdate;

			public override void WriteUpdate(Transaction transaction, IUpdateDepth updatedepth
				)
			{
				if (!this._firstUpdate)
				{
					base.WriteUpdate(transaction, updatedepth);
					return;
				}
				this._firstUpdate = false;
				ObjectContainerBase container = transaction.Container();
				this.SetStateClean();
				MarshallingContext context = new MarshallingContext(transaction, this, updatedepth
					, false);
				Handlers4.Write(this.ClassMetadata().TypeHandler(), context, this.GetObject());
				int length = this.Container().BlockConverter().BlockAlignedBytes(context.MarshalledLength
					());
				Slot slot = context.AllocateNewSlot(length);
				Pointer4 pointer = new Pointer4(this.GetID(), slot);
				ByteArrayBuffer buffer = context.ToWriteBuffer(pointer);
				container.WriteUpdate(transaction, pointer, this.ClassMetadata(), ArrayType.None, 
					buffer);
				if (this.IsActive())
				{
					this.SetStateClean();
				}
			}
		}
	}
}
