/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Freespace;
using Db4objects.Db4o.Internal.Ids;
using Db4objects.Db4o.Internal.Slots;
using Sharpen.Lang;

namespace Db4objects.Db4o.Internal.Ids
{
	/// <exclude></exclude>
	public class InMemoryIdSystem : IStackableIdSystem
	{
		private readonly LocalObjectContainer _container;

		private IdSlotTree _ids;

		private Slot _slot;

		private readonly SequentialIdGenerator _idGenerator;

		private int _childId;

		/// <summary>for testing purposes only.</summary>
		/// <remarks>for testing purposes only.</remarks>
		public InMemoryIdSystem(LocalObjectContainer container, int maxValidId)
		{
			_container = container;
			_idGenerator = new SequentialIdGenerator(new _IFunction4_32(this, maxValidId), _container
				.Handlers.LowestValidId(), maxValidId);
		}

		private sealed class _IFunction4_32 : IFunction4
		{
			public _IFunction4_32(InMemoryIdSystem _enclosing, int maxValidId)
			{
				this._enclosing = _enclosing;
				this.maxValidId = maxValidId;
			}

			public object Apply(object start)
			{
				return this._enclosing.FindFreeId((((int)start)), maxValidId);
			}

			private readonly InMemoryIdSystem _enclosing;

			private readonly int maxValidId;
		}

		public InMemoryIdSystem(LocalObjectContainer container) : this(container, int.MaxValue
			)
		{
			ReadThis();
		}

		private void ReadThis()
		{
			SystemData systemData = _container.SystemData();
			_slot = systemData.IdSystemSlot();
			if (!Slot.IsNull(_slot))
			{
				ByteArrayBuffer buffer = _container.ReadBufferBySlot(_slot);
				_childId = buffer.ReadInt();
				_idGenerator.Read(buffer);
				_ids = (IdSlotTree)new TreeReader(buffer, new IdSlotTree(0, null)).Read();
			}
		}

		public virtual void Close()
		{
		}

		// do nothing
		public virtual void Commit(IVisitable slotChanges, FreespaceCommitter freespaceCommitter
			)
		{
			Slot oldSlot = _slot;
			Slot reservedSlot = AllocateSlot(false, EstimatedSlotLength(EstimateMappingCount(
				slotChanges)));
			// No more operations against the FreespaceManager.
			// Time to free old slots.
			freespaceCommitter.Commit();
			slotChanges.Accept(new _IVisitor4_69(this));
			WriteThis(reservedSlot);
			FreeSlot(oldSlot);
		}

		private sealed class _IVisitor4_69 : IVisitor4
		{
			public _IVisitor4_69(InMemoryIdSystem _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Visit(object slotChange)
			{
				if (!((SlotChange)slotChange).SlotModified())
				{
					return;
				}
				if (((SlotChange)slotChange).RemoveId())
				{
					this._enclosing._ids = (IdSlotTree)Tree.RemoveLike(this._enclosing._ids, new TreeInt
						(((TreeInt)slotChange)._key));
					return;
				}
				if (DTrace.enabled)
				{
					DTrace.SlotCommitted.LogLength(((TreeInt)slotChange)._key, ((SlotChange)slotChange
						).NewSlot());
				}
				this._enclosing._ids = ((IdSlotTree)Tree.Add(this._enclosing._ids, new IdSlotTree
					(((TreeInt)slotChange)._key, ((SlotChange)slotChange).NewSlot())));
			}

			private readonly InMemoryIdSystem _enclosing;
		}

		private Slot AllocateSlot(bool appendToFile, int slotLength)
		{
			if (!appendToFile)
			{
				Slot slot = _container.FreespaceManager().AllocateSafeSlot(slotLength);
				if (slot != null)
				{
					return slot;
				}
			}
			return _container.AppendBytes(slotLength);
		}

		private int EstimateMappingCount(IVisitable slotChanges)
		{
			IntByRef count = new IntByRef();
			count.value = _ids == null ? 0 : _ids.Size();
			slotChanges.Accept(new _IVisitor4_103(count));
			return count.value;
		}

		private sealed class _IVisitor4_103 : IVisitor4
		{
			public _IVisitor4_103(IntByRef count)
			{
				this.count = count;
			}

			public void Visit(object slotChange)
			{
				if (!((SlotChange)slotChange).SlotModified() || ((SlotChange)slotChange).RemoveId
					())
				{
					return;
				}
				count.value++;
			}

			private readonly IntByRef count;
		}

		private void WriteThis(Slot reservedSlot)
		{
			// We need a little dance here to keep filling free slots
			// with X bytes. The FreespaceManager would do it immediately
			// upon the free call, but then our CrashSimulatingTestCase
			// fails because we have the Xses in the file before flushing.
			Slot xByteSlot = null;
			int slotLength = SlotLength();
			if (reservedSlot.Length() >= slotLength)
			{
				_slot = reservedSlot;
				reservedSlot = null;
			}
			else
			{
				_slot = AllocateSlot(true, slotLength);
			}
			ByteArrayBuffer buffer = new ByteArrayBuffer(_slot.Length());
			buffer.WriteInt(_childId);
			_idGenerator.Write(buffer);
			TreeInt.Write(buffer, _ids);
			_container.WriteBytes(buffer, _slot.Address(), 0);
			_container.SystemData().IdSystemSlot(_slot);
			IRunnable commitHook = _container.CommitHook();
			_container.SyncFiles(commitHook);
			FreeSlot(reservedSlot);
		}

		private void FreeSlot(Slot slot)
		{
			if (Slot.IsNull(slot))
			{
				return;
			}
			IFreespaceManager freespaceManager = _container.FreespaceManager();
			if (freespaceManager == null)
			{
				return;
			}
			freespaceManager.FreeSafeSlot(slot);
		}

		private int SlotLength()
		{
			return TreeInt.MarshalledLength(_ids) + _idGenerator.MarshalledLength() + Const4.
				IdLength;
		}

		private int EstimatedSlotLength(int estimatedCount)
		{
			IdSlotTree template = _ids;
			if (template == null)
			{
				template = new IdSlotTree(0, new Slot(0, 0));
			}
			return template.MarshalledLength(estimatedCount) + _idGenerator.MarshalledLength(
				) + Const4.IdLength;
		}

		public virtual Slot CommittedSlot(int id)
		{
			IdSlotTree idSlotMapping = (IdSlotTree)Tree.Find(_ids, new TreeInt(id));
			if (idSlotMapping == null)
			{
				throw new InvalidIDException(id);
			}
			return idSlotMapping.Slot();
		}

		public virtual void CompleteInterruptedTransaction(int address, int length)
		{
		}

		// do nothing
		public virtual int NewId()
		{
			int id = _idGenerator.NewId();
			_ids = ((IdSlotTree)Tree.Add(_ids, new IdSlotTree(id, Slot.Zero)));
			return id;
		}

		private int FindFreeId(int start, int end)
		{
			if (_ids == null)
			{
				return start;
			}
			IntByRef lastId = new IntByRef();
			IntByRef freeId = new IntByRef();
			Tree.Traverse(_ids, new TreeInt(start), new _ICancellableVisitor4_204(lastId, start
				, freeId));
			if (freeId.value > 0)
			{
				return freeId.value;
			}
			if (lastId.value < end)
			{
				return Math.Max(start, lastId.value + 1);
			}
			return 0;
		}

		private sealed class _ICancellableVisitor4_204 : ICancellableVisitor4
		{
			public _ICancellableVisitor4_204(IntByRef lastId, int start, IntByRef freeId)
			{
				this.lastId = lastId;
				this.start = start;
				this.freeId = freeId;
			}

			public bool Visit(object node)
			{
				int id = ((TreeInt)node)._key;
				if (lastId.value == 0)
				{
					if (id > start)
					{
						freeId.value = start;
						return false;
					}
					lastId.value = id;
					return true;
				}
				if (id > lastId.value + 1)
				{
					freeId.value = lastId.value + 1;
					return false;
				}
				lastId.value = id;
				return true;
			}

			private readonly IntByRef lastId;

			private readonly int start;

			private readonly IntByRef freeId;
		}

		public virtual void ReturnUnusedIds(IVisitable visitable)
		{
			visitable.Accept(new _IVisitor4_233(this));
		}

		private sealed class _IVisitor4_233 : IVisitor4
		{
			public _IVisitor4_233(InMemoryIdSystem _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Visit(object obj)
			{
				this._enclosing._ids = (IdSlotTree)Tree.RemoveLike(this._enclosing._ids, new TreeInt
					((((int)obj))));
			}

			private readonly InMemoryIdSystem _enclosing;
		}

		public virtual int ChildId()
		{
			return _childId;
		}

		public virtual void ChildId(int id)
		{
			_childId = id;
		}

		public virtual void TraverseOwnSlots(IProcedure4 block)
		{
			block.Apply(Pair.Of(0, _slot));
		}
	}
}
