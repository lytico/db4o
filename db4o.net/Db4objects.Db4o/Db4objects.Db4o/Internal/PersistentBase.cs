/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Ids;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public abstract class PersistentBase : Identifiable, IPersistent, ILinkLengthAware
	{
		internal virtual void CacheDirty(Collection4 col)
		{
			if (!BitIsTrue(Const4.CachedDirty))
			{
				BitTrue(Const4.CachedDirty);
				col.Add(this);
			}
		}

		public virtual void Free(LocalTransaction trans)
		{
			IdSystem(trans.SystemTransaction()).NotifySlotDeleted(GetID(), SlotChangeFactory(
				));
		}

		public int LinkLength()
		{
			return Const4.IdLength;
		}

		internal void NotCachedDirty()
		{
			BitFalse(Const4.CachedDirty);
		}

		public virtual void Read(Transaction trans)
		{
			if (!BeginProcessing())
			{
				return;
			}
			try
			{
				Read(trans, ProduceReadBuffer(trans));
			}
			finally
			{
				EndProcessing();
			}
		}

		protected virtual void Read(Transaction trans, ByteArrayBuffer reader)
		{
			ReadThis(trans, reader);
			SetStateOnRead(reader);
		}

		protected ByteArrayBuffer ProduceReadBuffer(Transaction trans)
		{
			return ReadBufferById(trans);
		}

		protected virtual ByteArrayBuffer ReadBufferById(Transaction trans)
		{
			return trans.Container().ReadBufferById(trans, GetID());
		}

		internal virtual void SetStateOnRead(ByteArrayBuffer reader)
		{
			if (BitIsTrue(Const4.CachedDirty))
			{
				SetStateDirty();
			}
			else
			{
				SetStateClean();
			}
		}

		public virtual void Write(Transaction trans)
		{
			if (!WriteObjectBegin())
			{
				return;
			}
			try
			{
				LocalObjectContainer container = (LocalObjectContainer)trans.Container();
				if (DTrace.enabled)
				{
					DTrace.PersistentOwnLength.Log(GetID());
				}
				int length = OwnLength();
				length = container.BlockConverter().BlockAlignedBytes(length);
				Slot slot = container.AllocateSlot(length);
				if (IsNew())
				{
					SetID(IdSystem(trans).NewId(SlotChangeFactory()));
					IdSystem(trans).NotifySlotCreated(_id, slot, SlotChangeFactory());
				}
				else
				{
					IdSystem(trans).NotifySlotUpdated(_id, slot, SlotChangeFactory());
				}
				if (DTrace.enabled)
				{
					DTrace.PersistentBaseNewSlot.LogLength(GetID(), slot);
				}
				ByteArrayBuffer writer = ProduceWriteBuffer(trans, length);
				WriteToFile(trans, writer, slot);
			}
			finally
			{
				EndProcessing();
			}
		}

		public virtual ITransactionalIdSystem IdSystem(Transaction trans)
		{
			return trans.IdSystem();
		}

		protected virtual ByteArrayBuffer ProduceWriteBuffer(Transaction trans, int length
			)
		{
			return NewWriteBuffer(length);
		}

		protected virtual ByteArrayBuffer NewWriteBuffer(int length)
		{
			return new ByteArrayBuffer(length);
		}

		private void WriteToFile(Transaction trans, ByteArrayBuffer writer, Slot slot)
		{
			if (DTrace.enabled)
			{
				DTrace.PersistentbaseWrite.Log(GetID());
			}
			LocalObjectContainer container = (LocalObjectContainer)trans.Container();
			WriteThis(trans, writer);
			container.WriteEncrypt(writer, slot.Address(), 0);
			if (IsActive())
			{
				SetStateClean();
			}
		}

		public virtual bool WriteObjectBegin()
		{
			if (IsDirty())
			{
				return BeginProcessing();
			}
			return false;
		}

		public virtual void WriteOwnID(Transaction trans, ByteArrayBuffer writer)
		{
			Write(trans);
			writer.WriteInt(GetID());
		}

		public virtual Db4objects.Db4o.Internal.Slots.SlotChangeFactory SlotChangeFactory
			()
		{
			return Db4objects.Db4o.Internal.Slots.SlotChangeFactory.SystemObjects;
		}

		public abstract byte GetIdentifier();

		public abstract int OwnLength();

		public abstract void ReadThis(Transaction arg1, ByteArrayBuffer arg2);

		public abstract void WriteThis(Transaction arg1, ByteArrayBuffer arg2);
	}
}
