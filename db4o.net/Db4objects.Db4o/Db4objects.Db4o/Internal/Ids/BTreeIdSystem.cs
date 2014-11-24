/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Internal.Ids;
using Db4objects.Db4o.Internal.Slots;
using Db4objects.Db4o.Marshall;

namespace Db4objects.Db4o.Internal.Ids
{
	/// <exclude></exclude>
	public class BTreeIdSystem : IStackableIdSystem
	{
		private const int BtreeIdIndex = 0;

		private const int IdGeneratorIndex = 1;

		private const int ChildIdIndex = 2;

		private readonly LocalObjectContainer _container;

		private readonly IStackableIdSystem _parentIdSystem;

		private readonly ITransactionalIdSystem _transactionalIdSystem;

		private readonly SequentialIdGenerator _idGenerator;

		private BTree _bTree;

		private PersistentIntegerArray _persistentState;

		public BTreeIdSystem(LocalObjectContainer container, IStackableIdSystem parentIdSystem
			, int maxValidId)
		{
			_container = container;
			_parentIdSystem = parentIdSystem;
			_transactionalIdSystem = container.NewTransactionalIdSystem(null, new _IClosure4_40
				(parentIdSystem));
			int persistentArrayId = parentIdSystem.ChildId();
			if (persistentArrayId == 0)
			{
				InitializeNew();
			}
			else
			{
				InitializeExisting(persistentArrayId);
			}
			_idGenerator = new SequentialIdGenerator(new _IFunction4_52(this), IdGeneratorValue
				(), _container.Handlers.LowestValidId(), maxValidId);
		}

		private sealed class _IClosure4_40 : IClosure4
		{
			public _IClosure4_40(IStackableIdSystem parentIdSystem)
			{
				this.parentIdSystem = parentIdSystem;
			}

			public object Run()
			{
				return parentIdSystem;
			}

			private readonly IStackableIdSystem parentIdSystem;
		}

		private sealed class _IFunction4_52 : IFunction4
		{
			public _IFunction4_52(BTreeIdSystem _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public object Apply(object start)
			{
				return this._enclosing.FindFreeId((((int)start)));
			}

			private readonly BTreeIdSystem _enclosing;
		}

		public BTreeIdSystem(LocalObjectContainer container, IStackableIdSystem idSystem)
			 : this(container, idSystem, int.MaxValue)
		{
		}

		private void InitializeExisting(int persistentArrayId)
		{
			_persistentState = new PersistentIntegerArray(SlotChangeFactory.IdSystem, _transactionalIdSystem
				, persistentArrayId);
			_persistentState.Read(Transaction());
			_bTree = new BTree(Transaction(), BTreeConfiguration(), BTreeId(), new BTreeIdSystem.IdSlotMappingHandler
				());
		}

		private Db4objects.Db4o.Internal.Btree.BTreeConfiguration BTreeConfiguration()
		{
			return new Db4objects.Db4o.Internal.Btree.BTreeConfiguration(_transactionalIdSystem
				, SlotChangeFactory.IdSystem, 64, false);
		}

		private int IdGeneratorValue()
		{
			return _persistentState.Array()[IdGeneratorIndex];
		}

		private void IdGeneratorValue(int value)
		{
			_persistentState.Array()[IdGeneratorIndex] = value;
		}

		private int BTreeId()
		{
			return _persistentState.Array()[BtreeIdIndex];
		}

		private void InitializeNew()
		{
			_bTree = new BTree(Transaction(), BTreeConfiguration(), new BTreeIdSystem.IdSlotMappingHandler
				());
			int idGeneratorValue = _container.Handlers.LowestValidId() - 1;
			_persistentState = new PersistentIntegerArray(SlotChangeFactory.IdSystem, _transactionalIdSystem
				, new int[] { _bTree.GetID(), idGeneratorValue, 0 });
			_persistentState.Write(Transaction());
			_parentIdSystem.ChildId(_persistentState.GetID());
		}

		private int FindFreeId(int start)
		{
			throw new NotImplementedException();
		}

		public virtual void Close()
		{
		}

		public virtual Slot CommittedSlot(int id)
		{
			IdSlotMapping mapping = (IdSlotMapping)_bTree.Search(Transaction(), new IdSlotMapping
				(id, 0, 0));
			if (mapping == null)
			{
				throw new InvalidIDException(id);
			}
			return mapping.Slot();
		}

		public virtual void CompleteInterruptedTransaction(int transactionId1, int transactionId2
			)
		{
		}

		// do nothing
		public virtual int NewId()
		{
			int id = _idGenerator.NewId();
			_bTree.Add(Transaction(), new IdSlotMapping(id, 0, 0));
			return id;
		}

		private Db4objects.Db4o.Internal.Transaction Transaction()
		{
			return _container.SystemTransaction();
		}

		public virtual void Commit(IVisitable slotChanges, FreespaceCommitter freespaceCommitter
			)
		{
			_container.FreespaceManager().BeginCommit();
			slotChanges.Accept(new _IVisitor4_129(this));
			// TODO: Maybe we want a BTree that doesn't allow duplicates.
			// Then we could do the following in one step without removing first.
			_bTree.Commit(Transaction());
			IdGeneratorValue(_idGenerator.PersistentGeneratorValue());
			if (_idGenerator.IsDirty())
			{
				_idGenerator.SetClean();
				_persistentState.SetStateDirty();
			}
			if (_persistentState.IsDirty())
			{
				_persistentState.Write(Transaction());
			}
			_container.FreespaceManager().EndCommit();
			_transactionalIdSystem.Commit(freespaceCommitter);
			_transactionalIdSystem.Clear();
		}

		private sealed class _IVisitor4_129 : IVisitor4
		{
			public _IVisitor4_129(BTreeIdSystem _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Visit(object slotChange)
			{
				if (!((SlotChange)slotChange).SlotModified())
				{
					return;
				}
				this._enclosing._bTree.Remove(this._enclosing.Transaction(), new IdSlotMapping(((
					TreeInt)slotChange)._key, 0, 0));
				if (((SlotChange)slotChange).RemoveId())
				{
					return;
				}
				this._enclosing._bTree.Add(this._enclosing.Transaction(), new IdSlotMapping(((TreeInt
					)slotChange)._key, ((SlotChange)slotChange).NewSlot()));
				if (DTrace.enabled)
				{
					DTrace.SlotMapped.LogLength(((TreeInt)slotChange)._key, ((SlotChange)slotChange).
						NewSlot());
				}
			}

			private readonly BTreeIdSystem _enclosing;
		}

		public virtual void ReturnUnusedIds(IVisitable visitable)
		{
			visitable.Accept(new _IVisitor4_167(this));
		}

		private sealed class _IVisitor4_167 : IVisitor4
		{
			public _IVisitor4_167(BTreeIdSystem _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Visit(object id)
			{
				this._enclosing._bTree.Remove(this._enclosing.Transaction(), new IdSlotMapping(((
					(int)id)), 0, 0));
			}

			private readonly BTreeIdSystem _enclosing;
		}

		public class IdSlotMappingHandler : IIndexable4
		{
			public virtual void DefragIndexEntry(DefragmentContextImpl context)
			{
				throw new NotImplementedException();
			}

			public virtual object ReadIndexEntry(IContext context, ByteArrayBuffer buffer)
			{
				return IdSlotMapping.Read(buffer);
			}

			public virtual void WriteIndexEntry(IContext context, ByteArrayBuffer buffer, object
				 mapping)
			{
				((IdSlotMapping)mapping).Write(buffer);
			}

			public virtual IPreparedComparison PrepareComparison(IContext context, object sourceMapping
				)
			{
				return new _IPreparedComparison_190(sourceMapping);
			}

			private sealed class _IPreparedComparison_190 : IPreparedComparison
			{
				public _IPreparedComparison_190(object sourceMapping)
				{
					this.sourceMapping = sourceMapping;
				}

				public int CompareTo(object targetMapping)
				{
					return ((IdSlotMapping)sourceMapping)._id == ((IdSlotMapping)targetMapping)._id ? 
						0 : (((IdSlotMapping)sourceMapping)._id < ((IdSlotMapping)targetMapping)._id ? -
						1 : 1);
				}

				private readonly object sourceMapping;
			}

			public int LinkLength()
			{
				return Const4.IntLength * 3;
			}
		}

		public virtual ITransactionalIdSystem FreespaceIdSystem()
		{
			return _transactionalIdSystem;
		}

		public virtual int ChildId()
		{
			return _persistentState.Array()[ChildIdIndex];
		}

		public virtual void ChildId(int id)
		{
			_persistentState.Array()[ChildIdIndex] = id;
			_persistentState.SetStateDirty();
		}

		public virtual void TraverseIds(IVisitor4 visitor)
		{
			_bTree.TraverseKeys(_container.SystemTransaction(), visitor);
		}

		public virtual void TraverseOwnSlots(IProcedure4 block)
		{
			_parentIdSystem.TraverseOwnSlots(block);
			block.Apply(OwnSlotInfo(_persistentState.GetID()));
			block.Apply(OwnSlotInfo(_bTree.GetID()));
			IEnumerator nodeIds = _bTree.AllNodeIds(_container.SystemTransaction());
			while (nodeIds.MoveNext())
			{
				block.Apply(OwnSlotInfo((((int)nodeIds.Current))));
			}
		}

		private Pair OwnSlotInfo(int id)
		{
			return Pair.Of(id, _parentIdSystem.CommittedSlot(id));
		}
	}
}
