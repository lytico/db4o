/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Ids;
using Db4objects.Db4o.Internal.Slots;
using Db4objects.Db4o.Internal.Transactionlog;

namespace Db4objects.Db4o.Internal.Ids
{
	/// <exclude></exclude>
	public sealed class PointerBasedIdSystem : IIdSystem
	{
		internal readonly TransactionLogHandler _transactionLogHandler;

		private readonly LocalObjectContainer _container;

		public PointerBasedIdSystem(LocalObjectContainer container)
		{
			_container = container;
			_transactionLogHandler = NewTransactionLogHandler(container);
		}

		public int NewId()
		{
			return _container.AllocatePointerSlot();
		}

		public Slot CommittedSlot(int id)
		{
			return _container.ReadPointerSlot(id);
		}

		public void Commit(IVisitable slotChanges, FreespaceCommitter freespaceCommitter)
		{
			Slot reservedSlot = _transactionLogHandler.AllocateSlot(false, CountSlotChanges(slotChanges
				));
			freespaceCommitter.Commit();
			_transactionLogHandler.ApplySlotChanges(slotChanges, CountSlotChanges(slotChanges
				), reservedSlot);
		}

		private int CountSlotChanges(IVisitable slotChanges)
		{
			IntByRef slotChangeCount = new IntByRef();
			slotChanges.Accept(new _IVisitor4_40(slotChangeCount));
			return slotChangeCount.value;
		}

		private sealed class _IVisitor4_40 : IVisitor4
		{
			public _IVisitor4_40(IntByRef slotChangeCount)
			{
				this.slotChangeCount = slotChangeCount;
			}

			public void Visit(object slotChange)
			{
				if (((SlotChange)slotChange).SlotModified())
				{
					slotChangeCount.value++;
				}
			}

			private readonly IntByRef slotChangeCount;
		}

		public void ReturnUnusedIds(IVisitable visitable)
		{
			visitable.Accept(new _IVisitor4_51(this));
		}

		private sealed class _IVisitor4_51 : IVisitor4
		{
			public _IVisitor4_51(PointerBasedIdSystem _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Visit(object id)
			{
				this._enclosing._container.Free((((int)id)), Const4.PointerLength);
			}

			private readonly PointerBasedIdSystem _enclosing;
		}

		private TransactionLogHandler NewTransactionLogHandler(LocalObjectContainer container
			)
		{
			bool fileBased = container.Config().FileBasedTransactionLog() && container is IoAdaptedObjectContainer;
			if (!fileBased)
			{
				return new EmbeddedTransactionLogHandler(container);
			}
			string fileName = ((IoAdaptedObjectContainer)container).FileName();
			return new FileBasedTransactionLogHandler(container, fileName);
		}

		public void Close()
		{
			_transactionLogHandler.Close();
		}

		public void CompleteInterruptedTransaction(int transactionId1, int transactionId2
			)
		{
			_transactionLogHandler.CompleteInterruptedTransaction(transactionId1, transactionId2
				);
		}

		public void TraverseOwnSlots(IProcedure4 block)
		{
		}
	}
}
