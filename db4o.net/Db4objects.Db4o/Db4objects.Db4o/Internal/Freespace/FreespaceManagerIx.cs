/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Freespace;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Internal.Freespace
{
	/// <summary>Old freespacemanager, before version 7.0.</summary>
	/// <remarks>
	/// Old freespacemanager, before version 7.0.
	/// If it is still in use freespace is dropped.
	/// <see cref="BTreeFreespaceManager">BTreeFreespaceManager</see>
	/// should be used instead.
	/// </remarks>
	public class FreespaceManagerIx : AbstractFreespaceManager
	{
		public FreespaceManagerIx(int discardLimit, int remainderSizeLimit) : base(null, 
			discardLimit, 0)
		{
		}

		public override Slot AllocateSafeSlot(int length)
		{
			return null;
		}

		public override void FreeSafeSlot(Slot slot)
		{
		}

		// do nothing
		public override void BeginCommit()
		{
		}

		public override void EndCommit()
		{
		}

		public override int SlotCount()
		{
			throw new InvalidOperationException();
		}

		public override void Free(Slot slot)
		{
		}

		// Should no longer be used: Should not happen.
		public override void FreeSelf()
		{
		}

		// do nothing, freespace is dropped.
		public override Slot AllocateSlot(int length)
		{
			// implementation is no longer present, no freespace returned.
			return null;
		}

		public override void MigrateTo(IFreespaceManager fm)
		{
		}

		// do nothing, freespace is dropped.
		public override void Traverse(IVisitor4 visitor)
		{
			throw new InvalidOperationException();
		}

		public override void Start(int id)
		{
		}

		public override byte SystemType()
		{
			return FmIx;
		}

		public override void Write(LocalObjectContainer container)
		{
		}

		public override void Commit()
		{
		}

		public override void Listener(IFreespaceListener listener)
		{
		}

		public override bool IsStarted()
		{
			return false;
		}

		public override Slot AllocateTransactionLogSlot(int length)
		{
			return null;
		}

		public override void Read(LocalObjectContainer container, Slot slot)
		{
		}
	}
}
