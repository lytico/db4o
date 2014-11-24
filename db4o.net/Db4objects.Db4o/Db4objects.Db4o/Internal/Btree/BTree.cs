/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using System.Text;
using Db4objects.Db4o;
using Db4objects.Db4o.Defragment;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Internal.Caching;
using Db4objects.Db4o.Internal.Ids;
using Db4objects.Db4o.Marshall;

namespace Db4objects.Db4o.Internal.Btree
{
	/// <exclude></exclude>
	public class BTree : LocalPersistentBase, ITransactionParticipant, IBTreeStructureListener
	{
		private readonly BTreeConfiguration _config;

		private const byte BtreeVersion = (byte)1;

		private const int DefragmentIncrementOffset = 1 + Const4.IntLength * 2;

		private readonly IIndexable4 _keyHandler;

		private BTreeNode _root;

		/// <summary>All instantiated nodes are held in this tree.</summary>
		/// <remarks>All instantiated nodes are held in this tree.</remarks>
		private TreeIntObject _nodes;

		private int _size;

		private IVisitor4 _removeListener;

		private sealed class _TransactionLocal_40 : TransactionLocal
		{
			public _TransactionLocal_40()
			{
			}

			// version byte
			// size, node size  
			public override object InitialValueFor(Transaction transaction)
			{
				return 0;
			}
		}

		private readonly TransactionLocal _sizeDeltaInTransaction = new _TransactionLocal_40
			();

		protected IQueue4 _processing;

		private int _nodeSize;

		internal int _halfNodeSize;

		private IBTreeStructureListener _structureListener;

		private readonly ICache4 _nodeCache;

		private TreeIntObject _evictedFromCache;

		private bool _disposed;

		public BTree(Transaction trans, BTreeConfiguration config, int id, IIndexable4 keyHandler
			, int treeNodeSize) : base(config._idSystem)
		{
			_config = config;
			if (null == keyHandler)
			{
				throw new ArgumentNullException();
			}
			_nodeSize = treeNodeSize;
			_nodeCache = CacheFactory.NewLRUIntCache(config._cacheSize);
			_halfNodeSize = _nodeSize / 2;
			_nodeSize = _halfNodeSize * 2;
			_keyHandler = keyHandler;
			SetID(id);
			if (IsNew())
			{
				SetStateDirty();
				_root = new BTreeNode(this, 0, true, 0, 0, 0);
				_root.Write(trans.SystemTransaction());
				AddNode(_root);
				Write(trans.SystemTransaction());
			}
			else
			{
				SetStateDeactivated();
			}
		}

		public BTree(Transaction trans, BTreeConfiguration config, IIndexable4 keyHandler
			) : this(trans, config, 0, keyHandler)
		{
		}

		public BTree(Transaction trans, BTreeConfiguration config, int id, IIndexable4 keyHandler
			) : this(trans, config, id, keyHandler, Config(trans).BTreeNodeSize())
		{
		}

		public BTree(Transaction trans, int id, IIndexable4 keyHandler) : this(trans, BTreeConfiguration
			.Default, id, keyHandler)
		{
		}

		public BTree(Transaction trans, int id, IIndexable4 keyHandler, int nodeSize) : this
			(trans, BTreeConfiguration.Default, id, keyHandler, nodeSize)
		{
		}

		public virtual BTreeNode Root()
		{
			return _root;
		}

		public virtual int NodeSize()
		{
			return _nodeSize;
		}

		public virtual void Add(Transaction trans, object key)
		{
			KeyCantBeNull(key);
			IPreparedComparison preparedComparison = _keyHandler.PrepareComparison(trans.Context
				(), key);
			Add(trans, preparedComparison, key);
		}

		public virtual void Add(Transaction trans, IPreparedComparison preparedComparison
			, object key)
		{
			EnsureActive(trans);
			Enlist(trans);
			BTreeNode rootOrSplit = _root.Add(trans, preparedComparison, key);
			if (rootOrSplit != null && rootOrSplit != _root)
			{
				EnsureDirty(trans);
				_root = new BTreeNode(trans, _root, rootOrSplit);
				_root.Write(trans.SystemTransaction());
				AddNode(_root);
			}
			ConvertCacheEvictedNodesToReadMode();
		}

		public virtual object Remove(Transaction trans, object key)
		{
			BTreePointer bTreePointer = SearchPointer(trans, key);
			if (bTreePointer == null)
			{
				return null;
			}
			object result = bTreePointer.Key();
			Enlist(trans);
			IPreparedComparison preparedComparison = KeyHandler().PrepareComparison(trans.Context
				(), key);
			BTreeNode node = bTreePointer.Node();
			node.Remove(trans, preparedComparison, key, bTreePointer.Index());
			ConvertCacheEvictedNodesToReadMode();
			return result;
		}

		public virtual IBTreeRange SearchRange(Transaction trans, object key)
		{
			KeyCantBeNull(key);
			return SearchRange(trans, KeyHandler().PrepareComparison(trans.Context(), key));
		}

		public virtual BTreePointer SearchPointer(Transaction trans, object key)
		{
			EnsureActive(trans);
			KeyCantBeNull(key);
			IPreparedComparison preparedComparison = KeyHandler().PrepareComparison(trans.Context
				(), key);
			BTreeNodeSearchResult start = SearchLeaf(trans, preparedComparison, SearchTarget.
				Lowest);
			BTreePointer bTreePointer = start.FirstValidPointer();
			if (bTreePointer == null)
			{
				ConvertCacheEvictedNodesToReadMode();
				return null;
			}
			object found = bTreePointer.Key();
			ConvertCacheEvictedNodesToReadMode();
			if (preparedComparison.CompareTo(found) == 0)
			{
				return bTreePointer;
			}
			return null;
		}

		public virtual object Search(Transaction trans, object key)
		{
			BTreePointer bTreePointer = SearchPointer(trans, key);
			if (bTreePointer != null)
			{
				return bTreePointer.Key();
			}
			return null;
		}

		private IBTreeRange SearchRange(Transaction trans, IPreparedComparison preparedComparison
			)
		{
			EnsureActive(trans);
			// TODO: Optimize the following.
			//       Part of the search operates against the same nodes.
			//       As long as the bounds are on one node, the search
			//       should walk the nodes in one go.
			BTreeNodeSearchResult start = SearchLeaf(trans, preparedComparison, SearchTarget.
				Lowest);
			BTreeNodeSearchResult end = SearchLeaf(trans, preparedComparison, SearchTarget.Highest
				);
			IBTreeRange range = start.CreateIncludingRange(end);
			ConvertCacheEvictedNodesToReadMode();
			return range;
		}

		private void KeyCantBeNull(object key)
		{
			if (null == key)
			{
				throw new ArgumentNullException();
			}
		}

		public virtual IIndexable4 KeyHandler()
		{
			return _keyHandler;
		}

		public virtual BTreeNodeSearchResult SearchLeafByObject(Transaction trans, object
			 key, SearchTarget target)
		{
			return SearchLeaf(trans, _keyHandler.PrepareComparison(trans.Context(), key), target
				);
		}

		public virtual BTreeNodeSearchResult SearchLeaf(Transaction trans, IPreparedComparison
			 preparedComparison, SearchTarget target)
		{
			EnsureActive(trans);
			BTreeNodeSearchResult result = _root.SearchLeaf(trans, preparedComparison, target
				);
			ConvertCacheEvictedNodesToReadMode();
			return result;
		}

		public virtual void Commit(Transaction transaction)
		{
			if (_disposed)
			{
				return;
			}
			UpdateSize(transaction);
			CommitNodes(transaction);
			FinishTransaction(transaction);
			ConvertCacheEvictedNodesToReadMode();
		}

		private void UpdateSize(Transaction transaction)
		{
			ByRef sizeInTransaction = SizeIn(transaction);
			int sizeModification = (((int)sizeInTransaction.value));
			if (sizeModification == 0)
			{
				return;
			}
			EnsureDirty(transaction);
			_size += sizeModification;
			sizeInTransaction.value = 0;
		}

		private ByRef SizeIn(Transaction trans)
		{
			return trans.Get(_sizeDeltaInTransaction);
		}

		private void CommitNodes(Transaction trans)
		{
			ProcessEachNode(new _IProcedure4_237(trans));
		}

		private sealed class _IProcedure4_237 : IProcedure4
		{
			public _IProcedure4_237(Transaction trans)
			{
				this.trans = trans;
			}

			public void Apply(object node)
			{
				((BTreeNode)node).Commit(trans);
			}

			private readonly Transaction trans;
		}

		private void ProcessEachNode(IProcedure4 action)
		{
			if (_nodes == null)
			{
				return;
			}
			ProcessAllNodes();
			while (_processing.HasNext())
			{
				action.Apply((BTreeNode)_processing.Next());
			}
			_processing = null;
		}

		public virtual void Rollback(Transaction trans)
		{
			RollbackNodes(trans);
			FinishTransaction(trans);
			ConvertCacheEvictedNodesToReadMode();
		}

		private void FinishTransaction(Transaction trans)
		{
			Transaction systemTransaction = trans.SystemTransaction();
			WriteAllNodes(systemTransaction);
			Write(systemTransaction);
			Purge();
		}

		private void RollbackNodes(Transaction trans)
		{
			ProcessEachNode(new _IProcedure4_266(trans));
		}

		private sealed class _IProcedure4_266 : IProcedure4
		{
			public _IProcedure4_266(Transaction trans)
			{
				this.trans = trans;
			}

			public void Apply(object node)
			{
				((BTreeNode)node).Rollback(trans);
			}

			private readonly Transaction trans;
		}

		private void WriteAllNodes(Transaction systemTransaction)
		{
			if (_nodes == null)
			{
				return;
			}
			_nodes.Traverse(new _IVisitor4_275(systemTransaction));
		}

		private sealed class _IVisitor4_275 : IVisitor4
		{
			public _IVisitor4_275(Transaction systemTransaction)
			{
				this.systemTransaction = systemTransaction;
			}

			public void Visit(object obj)
			{
				((BTreeNode)((TreeIntObject)obj).GetObject()).Write(systemTransaction);
			}

			private readonly Transaction systemTransaction;
		}

		private void Purge()
		{
			if (_nodes == null)
			{
				return;
			}
			Tree temp = _nodes;
			_nodes = null;
			_root.HoldChildrenAsIDs();
			AddNode(_root);
			temp.Traverse(new _IVisitor4_294());
			for (IEnumerator entryIter = _nodeCache.GetEnumerator(); entryIter.MoveNext(); )
			{
				BTreeNodeCacheEntry entry = ((BTreeNodeCacheEntry)entryIter.Current);
				entry._node.HoldChildrenAsIDs();
			}
		}

		private sealed class _IVisitor4_294 : IVisitor4
		{
			public _IVisitor4_294()
			{
			}

			public void Visit(object obj)
			{
				BTreeNode node = (BTreeNode)((TreeIntObject)obj).GetObject();
				node.Purge();
			}
		}

		private void ProcessAllNodes()
		{
			_processing = new NonblockingQueue();
			_nodes.Traverse(new _IVisitor4_311(this));
		}

		private sealed class _IVisitor4_311 : IVisitor4
		{
			public _IVisitor4_311(BTree _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Visit(object node)
			{
				this._enclosing._processing.Add(((TreeIntObject)node).GetObject());
			}

			private readonly BTree _enclosing;
		}

		private void EnsureActive(Transaction trans)
		{
			if (!IsActive())
			{
				Read(trans.SystemTransaction());
			}
		}

		private void EnsureDirty(Transaction trans)
		{
			EnsureActive(trans);
			Enlist(trans);
			SetStateDirty();
		}

		private void Enlist(Transaction trans)
		{
			if (CanEnlistWithTransaction())
			{
				((LocalTransaction)trans).Enlist(this);
			}
		}

		protected virtual bool CanEnlistWithTransaction()
		{
			return _config._canEnlistWithTransaction;
		}

		public override byte GetIdentifier()
		{
			return Const4.Btree;
		}

		public virtual void SetRemoveListener(IVisitor4 vis)
		{
			_removeListener = vis;
		}

		public override int OwnLength()
		{
			return 1 + Const4.ObjectLength + (Const4.IntLength * 2) + Const4.IdLength;
		}

		public virtual BTreeNode ProduceNode(int id)
		{
			if (DTrace.enabled)
			{
				DTrace.BtreeProduceNode.Log(id);
			}
			TreeIntObject addtio = new TreeIntObject(id);
			_nodes = (TreeIntObject)((TreeIntObject)Tree.Add(_nodes, addtio));
			TreeIntObject tio = (TreeIntObject)addtio.AddedOrExisting();
			BTreeNode node = (BTreeNode)tio.GetObject();
			if (node == null)
			{
				node = CacheEntry(new BTreeNode(id, this))._node;
				tio.SetObject(node);
				AddToProcessing(node);
			}
			return node;
		}

		internal virtual void AddNode(BTreeNode node)
		{
			_nodes = (TreeIntObject)((TreeIntObject)Tree.Add(_nodes, new TreeIntObject(node.GetID
				(), node)));
			AddToProcessing(node);
		}

		internal virtual void AddToProcessing(BTreeNode node)
		{
			if (_processing != null)
			{
				_processing.Add(node);
			}
		}

		internal virtual void RemoveNode(BTreeNode node)
		{
			_nodes = (TreeIntObject)((TreeInt)_nodes.RemoveLike(new TreeInt(node.GetID())));
		}

		internal virtual void NotifyRemoveListener(object obj)
		{
			if (_removeListener != null)
			{
				_removeListener.Visit(obj);
			}
		}

		public override void ReadThis(Transaction a_trans, ByteArrayBuffer a_reader)
		{
			a_reader.IncrementOffset(1);
			// first byte is version, for possible future format changes
			_size = a_reader.ReadInt();
			_nodeSize = a_reader.ReadInt();
			_halfNodeSize = NodeSize() / 2;
			_root = ProduceNode(a_reader.ReadInt());
		}

		public override void WriteThis(Transaction trans, ByteArrayBuffer a_writer)
		{
			a_writer.WriteByte(BtreeVersion);
			a_writer.WriteInt(_size);
			a_writer.WriteInt(NodeSize());
			a_writer.WriteIDOf(trans, _root);
		}

		public virtual int Size(Transaction trans)
		{
			// This implementation of size will not work accurately for multiple
			// transactions. If two transactions call clear and both commit, _size
			// can end up negative.
			// For multiple transactions the size patches only are an estimate.
			EnsureActive(trans);
			return _size + (((int)SizeIn(trans).value));
		}

		public virtual void TraverseKeys(Transaction trans, IVisitor4 visitor)
		{
			EnsureActive(trans);
			if (_root == null)
			{
				return;
			}
			_root.TraverseKeys(trans, visitor);
			ConvertCacheEvictedNodesToReadMode();
		}

		public virtual void SizeChanged(Transaction transaction, BTreeNode node, int changeBy
			)
		{
			NotifyCountChanged(transaction, node, changeBy);
			ByRef sizeInTransaction = SizeIn(transaction);
			sizeInTransaction.value = (((int)sizeInTransaction.value)) + changeBy;
		}

		public virtual void Dispose(Transaction transaction)
		{
		}

		public virtual BTreePointer FirstPointer(Transaction trans)
		{
			EnsureActive(trans);
			if (null == _root)
			{
				return null;
			}
			BTreePointer pointer = _root.FirstPointer(trans);
			ConvertCacheEvictedNodesToReadMode();
			return pointer;
		}

		public virtual BTreePointer LastPointer(Transaction trans)
		{
			EnsureActive(trans);
			if (null == _root)
			{
				return null;
			}
			BTreePointer pointer = _root.LastPointer(trans);
			ConvertCacheEvictedNodesToReadMode();
			return pointer;
		}

		public virtual Db4objects.Db4o.Internal.Btree.BTree DebugLoadFully(Transaction trans
			)
		{
			EnsureActive(trans);
			_root.DebugLoadFully(trans);
			return this;
		}

		private void TraverseAllNodes(Transaction trans, IVisitor4 command)
		{
			EnsureActive(trans);
			_root.TraverseAllNodes(trans, command);
		}

		public virtual void DefragIndex(DefragmentContextImpl context)
		{
			context.IncrementOffset(DefragmentIncrementOffset);
			context.CopyID();
		}

		public virtual void DefragIndexNode(DefragmentContextImpl context)
		{
			BTreeNode.DefragIndex(context, _keyHandler);
		}

		public virtual void DefragBTree(IDefragmentServices services)
		{
			DefragmentContextImpl.ProcessCopy(services, GetID(), new _ISlotCopyHandler_481(this
				));
			services.TraverseAllIndexSlots(this, new _IVisitor4_486(this, services));
			ConvertCacheEvictedNodesToReadMode();
		}

		private sealed class _ISlotCopyHandler_481 : ISlotCopyHandler
		{
			public _ISlotCopyHandler_481(BTree _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void ProcessCopy(DefragmentContextImpl context)
			{
				this._enclosing.DefragIndex(context);
			}

			private readonly BTree _enclosing;
		}

		private sealed class _IVisitor4_486 : IVisitor4
		{
			public _IVisitor4_486(BTree _enclosing, IDefragmentServices services)
			{
				this._enclosing = _enclosing;
				this.services = services;
			}

			public void Visit(object obj)
			{
				int id = ((int)obj);
				DefragmentContextImpl.ProcessCopy(services, id, new _ISlotCopyHandler_489(this));
			}

			private sealed class _ISlotCopyHandler_489 : ISlotCopyHandler
			{
				public _ISlotCopyHandler_489(_IVisitor4_486 _enclosing)
				{
					this._enclosing = _enclosing;
				}

				public void ProcessCopy(DefragmentContextImpl context)
				{
					this._enclosing._enclosing.DefragIndexNode(context);
				}

				private readonly _IVisitor4_486 _enclosing;
			}

			private readonly BTree _enclosing;

			private readonly IDefragmentServices services;
		}

		internal virtual int CompareKeys(IContext context, object key1, object key2)
		{
			IPreparedComparison preparedComparison = _keyHandler.PrepareComparison(context, key1
				);
			return preparedComparison.CompareTo(key2);
		}

		private static Config4Impl Config(Transaction trans)
		{
			if (null == trans)
			{
				throw new ArgumentNullException();
			}
			return trans.Container().ConfigImpl;
		}

		public override void Free(LocalTransaction systemTrans)
		{
			_disposed = true;
			FreeAllNodeIds(systemTrans, AllNodeIds(systemTrans));
			base.Free((LocalTransaction)systemTrans);
		}

		private void FreeAllNodeIds(LocalTransaction systemTrans, IEnumerator allNodeIDs)
		{
			ITransactionalIdSystem idSystem = IdSystem(systemTrans);
			while (allNodeIDs.MoveNext())
			{
				int id = ((int)allNodeIDs.Current);
				idSystem.NotifySlotDeleted(id, SlotChangeFactory());
			}
		}

		public virtual IEnumerator AllNodeIds(Transaction systemTrans)
		{
			Collection4 allNodeIDs = new Collection4();
			TraverseAllNodes(systemTrans, new _IVisitor4_527(allNodeIDs));
			return allNodeIDs.GetEnumerator();
		}

		private sealed class _IVisitor4_527 : IVisitor4
		{
			public _IVisitor4_527(Collection4 allNodeIDs)
			{
				this.allNodeIDs = allNodeIDs;
			}

			public void Visit(object node)
			{
				allNodeIDs.Add(((BTreeNode)node).GetID());
			}

			private readonly Collection4 allNodeIDs;
		}

		public virtual IBTreeRange AsRange(Transaction trans)
		{
			return new BTreeRangeSingle(trans, this, FirstPointer(trans), null);
		}

		private void TraverseAllNodes(IVisitor4 visitor)
		{
			if (_nodes == null)
			{
				return;
			}
			_nodes.Traverse(new _IVisitor4_543(visitor));
		}

		private sealed class _IVisitor4_543 : IVisitor4
		{
			public _IVisitor4_543(IVisitor4 visitor)
			{
				this.visitor = visitor;
			}

			public void Visit(object obj)
			{
				visitor.Visit(((TreeIntObject)obj).GetObject());
			}

			private readonly IVisitor4 visitor;
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("BTree ");
			sb.Append(GetID());
			sb.Append(" Active Nodes: \n");
			TraverseAllNodes(new _IVisitor4_555(sb));
			return sb.ToString();
		}

		private sealed class _IVisitor4_555 : IVisitor4
		{
			public _IVisitor4_555(StringBuilder sb)
			{
				this.sb = sb;
			}

			public void Visit(object obj)
			{
				sb.Append(obj);
				sb.Append("\n");
			}

			private readonly StringBuilder sb;
		}

		public virtual void StructureListener(IBTreeStructureListener listener)
		{
			_structureListener = listener;
		}

		public virtual void NotifySplit(Transaction trans, BTreeNode originalNode, BTreeNode
			 newRightNode)
		{
			if (_structureListener != null)
			{
				_structureListener.NotifySplit(trans, originalNode, newRightNode);
			}
		}

		public virtual void NotifyDeleted(Transaction trans, BTreeNode node)
		{
			if (_structureListener != null)
			{
				_structureListener.NotifyDeleted(trans, node);
			}
		}

		public virtual void NotifyCountChanged(Transaction trans, BTreeNode node, int diff
			)
		{
			if (_structureListener != null)
			{
				_structureListener.NotifyCountChanged(trans, node, diff);
			}
		}

		public virtual IEnumerator Iterator(Transaction trans)
		{
			return new BTreeIterator(trans, this);
		}

		public virtual void Clear(Transaction transaction)
		{
			BTreePointer currentPointer = FirstPointer(transaction);
			while (currentPointer != null && currentPointer.IsValid())
			{
				BTreeNode node = currentPointer.Node();
				int index = currentPointer.Index();
				node.Remove(transaction, index);
				currentPointer = currentPointer.Next();
			}
		}

		public virtual ICache4 NodeCache()
		{
			return _nodeCache;
		}

		internal virtual BTreeNodeCacheEntry CacheEntry(BTreeNode node)
		{
			return ((BTreeNodeCacheEntry)_nodeCache.Produce(node.GetID(), new _IFunction4_605
				(node), new _IProcedure4_609(this)));
		}

		private sealed class _IFunction4_605 : IFunction4
		{
			public _IFunction4_605(BTreeNode node)
			{
				this.node = node;
			}

			public object Apply(object id)
			{
				return new BTreeNodeCacheEntry(node);
			}

			private readonly BTreeNode node;
		}

		private sealed class _IProcedure4_609 : IProcedure4
		{
			public _IProcedure4_609(BTree _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Apply(object entry)
			{
				this._enclosing.EvictedFromCache(((BTreeNodeCacheEntry)entry)._node);
			}

			private readonly BTree _enclosing;
		}

		public override Db4objects.Db4o.Internal.Slots.SlotChangeFactory SlotChangeFactory
			()
		{
			return _config._slotChangeFactory;
		}

		public virtual void EvictedFromCache(BTreeNode node)
		{
			_evictedFromCache = ((TreeIntObject)Tree.Add(_evictedFromCache, new TreeIntObject
				(node.GetID(), node)));
		}

		public virtual void ConvertCacheEvictedNodesToReadMode()
		{
			if (_evictedFromCache == null)
			{
				return;
			}
			Tree.Traverse(_evictedFromCache, new _IVisitor4_628());
			_evictedFromCache = null;
		}

		private sealed class _IVisitor4_628 : IVisitor4
		{
			public _IVisitor4_628()
			{
			}

			public void Visit(object treeIntObject)
			{
				((BTreeNode)((TreeIntObject)treeIntObject)._object).ToReadMode();
			}
		}
	}
}
