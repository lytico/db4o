/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit.Extensions;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Freespace;
using Db4objects.Db4o.Internal.Slots;

namespace Db4oUnit.Extensions
{
	public class FreespaceManagerForDebug : IFreespaceManager
	{
		private readonly ISlotListener _listener;

		public FreespaceManagerForDebug(ISlotListener listener)
		{
			_listener = listener;
		}

		public virtual Slot AllocateSafeSlot(int length)
		{
			return null;
		}

		public virtual void FreeSafeSlot(Slot slot)
		{
		}

		public virtual void BeginCommit()
		{
		}

		public virtual void Commit()
		{
		}

		public virtual void EndCommit()
		{
		}

		public virtual int SlotCount()
		{
			return 0;
		}

		public virtual void Free(Slot slot)
		{
			_listener.OnFree(slot);
		}

		public virtual void FreeSelf()
		{
		}

		public virtual Slot AllocateSlot(int length)
		{
			return null;
		}

		public virtual void Start(int id)
		{
		}

		public virtual byte SystemType()
		{
			return AbstractFreespaceManager.FmDebug;
		}

		public virtual void Traverse(IVisitor4 visitor)
		{
		}

		public virtual void Write(LocalObjectContainer container)
		{
		}

		public virtual void Listener(IFreespaceListener listener)
		{
		}

		public virtual void MigrateTo(IFreespaceManager fm)
		{
		}

		// TODO Auto-generated method stub
		public virtual int TotalFreespace()
		{
			// TODO Auto-generated method stub
			return 0;
		}

		public virtual void SlotFreed(Slot slot)
		{
		}

		// TODO Auto-generated method stub
		public virtual bool IsStarted()
		{
			return false;
		}

		public virtual Slot AllocateTransactionLogSlot(int length)
		{
			return null;
		}

		public virtual void Read(LocalObjectContainer container, Slot slot)
		{
		}
	}
}
