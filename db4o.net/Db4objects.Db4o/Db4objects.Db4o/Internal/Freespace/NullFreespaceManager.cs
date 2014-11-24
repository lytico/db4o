/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Freespace;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Internal.Freespace
{
	/// <exclude></exclude>
	public class NullFreespaceManager : IFreespaceManager
	{
		public static readonly IFreespaceManager Instance = new Db4objects.Db4o.Internal.Freespace.NullFreespaceManager
			();

		private NullFreespaceManager()
		{
		}

		public virtual Slot AllocateSlot(int length)
		{
			return null;
		}

		public virtual Slot AllocateSafeSlot(int length)
		{
			return null;
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

		public virtual void Free(Slot slot)
		{
		}

		public virtual void FreeSelf()
		{
		}

		public virtual void FreeSafeSlot(Slot slot)
		{
		}

		public virtual void Listener(IFreespaceListener listener)
		{
		}

		public virtual void MigrateTo(IFreespaceManager fm)
		{
		}

		public virtual int SlotCount()
		{
			return 0;
		}

		public virtual void SlotFreed(Slot slot)
		{
		}

		public virtual void Start(int id)
		{
		}

		public virtual byte SystemType()
		{
			return 0;
		}

		public virtual int TotalFreespace()
		{
			return 0;
		}

		public virtual void Traverse(IVisitor4 visitor)
		{
		}

		public virtual void Write(LocalObjectContainer container)
		{
		}

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
