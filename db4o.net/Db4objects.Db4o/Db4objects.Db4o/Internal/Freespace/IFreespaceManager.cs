/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Freespace;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Internal.Freespace
{
	/// <exclude></exclude>
	public interface IFreespaceManager
	{
		void BeginCommit();

		void EndCommit();

		int SlotCount();

		void Free(Slot slot);

		void FreeSelf();

		int TotalFreespace();

		Slot AllocateTransactionLogSlot(int length);

		Slot AllocateSlot(int length);

		void MigrateTo(IFreespaceManager fm);

		void Read(LocalObjectContainer container, Slot slot);

		void Start(int id);

		byte SystemType();

		void Traverse(IVisitor4 visitor);

		void Write(LocalObjectContainer container);

		void Commit();

		Slot AllocateSafeSlot(int length);

		void FreeSafeSlot(Slot slot);

		void Listener(IFreespaceListener listener);

		void SlotFreed(Slot slot);

		bool IsStarted();
	}
}
