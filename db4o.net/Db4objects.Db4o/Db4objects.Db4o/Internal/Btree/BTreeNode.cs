/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Internal.Ids;
using Db4objects.Db4o.Marshall;
using Sharpen;

namespace Db4objects.Db4o.Internal.Btree
{
	/// <summary>
	/// We work with BTreeNode in two states:
	/// - deactivated: never read, no valid members, ID correct or 0 if new
	/// - write: real representation of keys, values and children in arrays
	/// The write state can be detected with canWrite().
	/// </summary>
	/// <remarks>
	/// We work with BTreeNode in two states:
	/// - deactivated: never read, no valid members, ID correct or 0 if new
	/// - write: real representation of keys, values and children in arrays
	/// The write state can be detected with canWrite(). States can be changed
	/// as needed with prepareRead() and prepareWrite().
	/// </remarks>
	/// <exclude></exclude>
	public sealed class BTreeNode : LocalPersistentBase
	{
		private const int CountLeafAnd3LinkLength = (Const4.IntLength * 4) + 1;

		private const int SlotLeadingLength = Const4.LeadingLength + CountLeafAnd3LinkLength;

		internal readonly BTree _btree;

		private int _count;

		private bool _isLeaf;

		private object[] _keys;

		/// <summary>Can contain BTreeNode or Integer for ID of BTreeNode</summary>
		private object[] _children;

		private int _parentID;

		private int _previousID;

		private int _nextID;

		private bool _dead;

		public BTreeNode(BTree btree, int count, bool isLeaf, int parentID, int previousID
			, int nextID)
		{
			_btree = btree;
			_parentID = parentID;
			_previousID = previousID;
			_nextID = nextID;
			_count = count;
			_isLeaf = isLeaf;
			PrepareArrays();
		}

		public BTreeNode(int id, BTree btree)
		{
			_btree = btree;
			SetID(id);
			SetStateDeactivated();
		}

		public BTreeNode(Transaction trans, Db4objects.Db4o.Internal.Btree.BTreeNode firstChild
			, Db4objects.Db4o.Internal.Btree.BTreeNode secondChild) : this(firstChild._btree
			, 2, false, 0, 0, 0)
		{
			_keys[0] = firstChild._keys[0];
			_children[0] = firstChild;
			_keys[1] = secondChild._keys[0];
			_children[1] = secondChild;
			Write(trans.SystemTransaction());
			firstChild.SetParentID(trans, GetID());
			secondChild.SetParentID(trans, GetID());
		}

		public BTree Btree()
		{
			return _btree;
		}

		/// <returns>
		/// the split node if this node is split
		/// or this if the first key has changed
		/// </returns>
		public Db4objects.Db4o.Internal.Btree.BTreeNode Add(Transaction trans, IPreparedComparison
			 preparedComparison, object obj)
		{
			ByteArrayBuffer reader = PrepareRead(trans);
			Searcher s = Search(trans, preparedComparison, reader);
			if (_isLeaf)
			{
				PrepareWrite(trans);
				SetStateDirty();
				if (WasRemoved(trans, s))
				{
					CancelRemoval(trans, obj, s.Cursor());
					return null;
				}
				if (s.Count() > 0 && !s.BeforeFirst())
				{
					s.MoveForward();
				}
				PrepareInsert(s.Cursor());
				_keys[s.Cursor()] = ApplyNewAddPatch(trans, obj);
			}
			else
			{
				Db4objects.Db4o.Internal.Btree.BTreeNode childNode = Child(reader, s.Cursor());
				Db4objects.Db4o.Internal.Btree.BTreeNode childNodeOrSplit = childNode.Add(trans, 
					preparedComparison, obj);
				if (childNodeOrSplit == null)
				{
					return null;
				}
				PrepareWrite(trans);
				SetStateDirty();
				_keys[s.Cursor()] = childNode._keys[0];
				if (childNode != childNodeOrSplit)
				{
					int splitCursor = s.Cursor() + 1;
					PrepareInsert(splitCursor);
					_keys[splitCursor] = childNodeOrSplit._keys[0];
					_children[splitCursor] = childNodeOrSplit;
				}
			}
			if (MustSplit())
			{
				return Split(trans);
			}
			if (s.Cursor() == 0)
			{
				return this;
			}
			return null;
		}

		private bool MustSplit()
		{
			return _count >= _btree.NodeSize();
		}

		private BTreeAdd ApplyNewAddPatch(Transaction trans, object obj)
		{
			SizeIncrement(trans);
			return new BTreeAdd(trans, obj);
		}

		private void CancelRemoval(Transaction trans, object obj, int index)
		{
			BTreeUpdate patch = (BTreeUpdate)KeyPatch(index);
			BTreeUpdate nextPatch = patch.RemoveFor(trans);
			_keys[index] = NewCancelledRemoval(trans, patch.GetObject(), obj, nextPatch);
			SizeIncrement(trans);
		}

		private BTreePatch NewCancelledRemoval(Transaction trans, object originalObject, 
			object currentObject, BTreeUpdate existingPatches)
		{
			return new BTreeCancelledRemoval(trans, originalObject, currentObject, existingPatches
				);
		}

		private void SizeIncrement(Transaction trans)
		{
			_btree.SizeChanged(trans, this, 1);
		}

		private bool WasRemoved(Transaction trans, Searcher s)
		{
			if (!s.FoundMatch())
			{
				return false;
			}
			BTreePatch patch = KeyPatch(trans, s.Cursor());
			return patch != null && patch.IsRemove();
		}

		internal BTreeNodeSearchResult SearchLeaf(Transaction trans, IPreparedComparison 
			preparedComparison, SearchTarget target)
		{
			ByteArrayBuffer reader = PrepareRead(trans);
			Searcher s = Search(trans, preparedComparison, reader, target);
			if (!_isLeaf)
			{
				return Child(reader, s.Cursor()).SearchLeaf(trans, preparedComparison, target);
			}
			if (!s.FoundMatch() || target == SearchTarget.Any || target == SearchTarget.Highest)
			{
				return new BTreeNodeSearchResult(trans, reader, Btree(), s, this);
			}
			if (target == SearchTarget.Lowest)
			{
				BTreeNodeSearchResult res = FindLowestLeafMatch(trans, preparedComparison, s.Cursor
					() - 1);
				if (res != null)
				{
					return res;
				}
				return CreateMatchingSearchResult(trans, reader, s.Cursor());
			}
			throw new InvalidOperationException();
		}

		private BTreeNodeSearchResult FindLowestLeafMatch(Transaction trans, IPreparedComparison
			 preparedComparison, int index)
		{
			return FindLowestLeafMatch(trans, preparedComparison, PrepareRead(trans), index);
		}

		private BTreeNodeSearchResult FindLowestLeafMatch(Transaction trans, IPreparedComparison
			 preparedComparison, ByteArrayBuffer reader, int index)
		{
			if (index >= 0)
			{
				if (!CompareEquals(preparedComparison, trans, reader, index))
				{
					return null;
				}
				if (index > 0)
				{
					BTreeNodeSearchResult res = FindLowestLeafMatch(trans, preparedComparison, reader
						, index - 1);
					if (res != null)
					{
						return res;
					}
					return CreateMatchingSearchResult(trans, reader, index);
				}
			}
			Db4objects.Db4o.Internal.Btree.BTreeNode node = PreviousNode();
			if (node != null)
			{
				ByteArrayBuffer nodeReader = node.PrepareRead(trans);
				BTreeNodeSearchResult res = node.FindLowestLeafMatch(trans, preparedComparison, nodeReader
					, node.LastIndex());
				if (res != null)
				{
					return res;
				}
			}
			if (index < 0)
			{
				return null;
			}
			return CreateMatchingSearchResult(trans, reader, index);
		}

		private bool CompareEquals(IPreparedComparison preparedComparison, Transaction trans
			, ByteArrayBuffer reader, int index)
		{
			if (CanWrite())
			{
				return CompareInWriteMode(preparedComparison, index) == 0;
			}
			return CompareInReadMode(trans, preparedComparison, reader, index) == 0;
		}

		private BTreeNodeSearchResult CreateMatchingSearchResult(Transaction trans, ByteArrayBuffer
			 reader, int index)
		{
			return new BTreeNodeSearchResult(trans, reader, Btree(), this, index, true);
		}

		public bool CanWrite()
		{
			return _keys != null;
		}

		internal Db4objects.Db4o.Internal.Btree.BTreeNode Child(int index)
		{
			if (_children[index] is Db4objects.Db4o.Internal.Btree.BTreeNode)
			{
				return (Db4objects.Db4o.Internal.Btree.BTreeNode)_children[index];
			}
			return ProduceChild(index, ((int)_children[index]));
		}

		internal Db4objects.Db4o.Internal.Btree.BTreeNode Child(ByteArrayBuffer reader, int
			 index)
		{
			if (ChildLoaded(index))
			{
				return (Db4objects.Db4o.Internal.Btree.BTreeNode)_children[index];
			}
			return ProduceChild(index, ChildID(reader, index));
		}

		private Db4objects.Db4o.Internal.Btree.BTreeNode ProduceChild(int index, int childID
			)
		{
			Db4objects.Db4o.Internal.Btree.BTreeNode child = _btree.ProduceNode(childID);
			if (_children != null)
			{
				_children[index] = child;
			}
			return child;
		}

		private int ChildID(ByteArrayBuffer reader, int index)
		{
			if (_children == null)
			{
				SeekChild(reader, index);
				return reader.ReadInt();
			}
			return ChildID(index);
		}

		private int ChildID(int index)
		{
			if (ChildLoaded(index))
			{
				return ((Db4objects.Db4o.Internal.Btree.BTreeNode)_children[index]).GetID();
			}
			return ((int)_children[index]);
		}

		private bool ChildLoaded(int index)
		{
			if (_children == null)
			{
				return false;
			}
			return _children[index] is Db4objects.Db4o.Internal.Btree.BTreeNode;
		}

		private bool ChildCanSupplyFirstKey(int index)
		{
			if (!ChildLoaded(index))
			{
				return false;
			}
			return ((Db4objects.Db4o.Internal.Btree.BTreeNode)_children[index]).CanWrite();
		}

		public void Commit(Transaction trans)
		{
			CommitOrRollback(trans, true);
		}

		internal void CommitOrRollback(Transaction trans, bool isCommit)
		{
			if (DTrace.enabled)
			{
				DTrace.BtreeNodeCommitOrRollback.Log(GetID());
			}
			if (_dead)
			{
				return;
			}
			if (!_isLeaf)
			{
				return;
			}
			if (!IsDirty(trans))
			{
				return;
			}
			object keyZero = _keys[0];
			object[] tempKeys = new object[_btree.NodeSize()];
			int count = 0;
			for (int i = 0; i < _count; i++)
			{
				object key = _keys[i];
				BTreePatch patch = KeyPatch(i);
				if (patch != null)
				{
					key = isCommit ? patch.Commit(trans, _btree, this) : patch.Rollback(trans, _btree
						);
				}
				if (key != No4.Instance)
				{
					tempKeys[count] = key;
					count++;
				}
			}
			_keys = tempKeys;
			_count = count;
			if (FreeIfEmpty(trans))
			{
				return;
			}
			SetStateDirty();
			// TODO: Merge nodes here on low _count value.
			if (_keys[0] != keyZero)
			{
				TellParentAboutChangedKey(trans);
			}
		}

		private bool FreeIfEmpty(Transaction trans)
		{
			return FreeIfEmpty(trans, _count);
		}

		private bool FreeIfEmpty(Transaction trans, int count)
		{
			if (count > 0)
			{
				return false;
			}
			if (IsRoot())
			{
				return false;
			}
			Free((LocalTransaction)trans);
			return true;
		}

		private bool IsRoot()
		{
			return _btree.Root() == this;
		}

		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			if (!(obj is Db4objects.Db4o.Internal.Btree.BTreeNode))
			{
				return false;
			}
			Db4objects.Db4o.Internal.Btree.BTreeNode other = (Db4objects.Db4o.Internal.Btree.BTreeNode
				)obj;
			return GetID() == other.GetID();
		}

		public override int GetHashCode()
		{
			return GetID();
		}

		public override void Free(LocalTransaction trans)
		{
			_dead = true;
			if (!IsRoot())
			{
				Db4objects.Db4o.Internal.Btree.BTreeNode parent = _btree.ProduceNode(_parentID);
				parent.RemoveChild(trans, this);
			}
			PointPreviousTo(trans, _nextID);
			PointNextTo(trans, _previousID);
			base.Free((LocalTransaction)trans);
			_btree.RemoveNode(this);
			_btree.NotifyDeleted(trans, this);
		}

		internal void HoldChildrenAsIDs()
		{
			if (_children == null)
			{
				return;
			}
			for (int i = 0; i < _count; i++)
			{
				if (_children[i] is Db4objects.Db4o.Internal.Btree.BTreeNode)
				{
					_children[i] = ((Db4objects.Db4o.Internal.Btree.BTreeNode)_children[i]).GetID();
				}
			}
		}

		private void RemoveChild(Transaction trans, Db4objects.Db4o.Internal.Btree.BTreeNode
			 child)
		{
			PrepareWrite(trans);
			SetStateDirty();
			int id = child.GetID();
			for (int i = 0; i < _count; i++)
			{
				if (ChildID(i) == id)
				{
					if (FreeIfEmpty(trans, _count - 1))
					{
						return;
					}
					Remove(i);
					if (i < 1)
					{
						TellParentAboutChangedKey(trans);
					}
					if (_count == 0)
					{
						// root node empty case only, have to turn it into a leaf
						_isLeaf = true;
					}
					return;
				}
			}
			throw new InvalidOperationException("child not found");
		}

		private void KeyChanged(Transaction trans, Db4objects.Db4o.Internal.Btree.BTreeNode
			 child)
		{
			PrepareWrite(trans);
			SetStateDirty();
			int id = child.GetID();
			for (int i = 0; i < _count; i++)
			{
				if (ChildID(i) == id)
				{
					_keys[i] = child._keys[0];
					_children[i] = child;
					KeyChanged(trans, i);
					return;
				}
			}
			throw new InvalidOperationException("child not found");
		}

		private void TellParentAboutChangedKey(Transaction trans)
		{
			if (!IsRoot())
			{
				Db4objects.Db4o.Internal.Btree.BTreeNode parent = _btree.ProduceNode(_parentID);
				parent.KeyChanged(trans, this);
			}
		}

		private bool IsDirty(Transaction trans)
		{
			if (!CanWrite())
			{
				return false;
			}
			for (int i = 0; i < _count; i++)
			{
				if (KeyPatch(trans, i) != null)
				{
					return true;
				}
			}
			return false;
		}

		private int CompareInWriteMode(IPreparedComparison preparedComparison, int index)
		{
			return -preparedComparison.CompareTo(Key(index));
		}

		private int CompareInReadMode(Transaction trans, IPreparedComparison preparedComparison
			, ByteArrayBuffer reader, int index)
		{
			SeekKey(reader, index);
			return -preparedComparison.CompareTo(KeyHandler().ReadIndexEntry(trans.Context(), 
				reader));
		}

		public int Count()
		{
			return _count;
		}

		private int EntryLength()
		{
			int len = KeyHandler().LinkLength();
			if (!_isLeaf)
			{
				len += Const4.IdLength;
			}
			return len;
		}

		public int FirstKeyIndex(Transaction trans)
		{
			for (int ix = 0; ix < _count; ix++)
			{
				if (IndexIsValid(trans, ix))
				{
					return ix;
				}
			}
			return -1;
		}

		public int LastKeyIndex(Transaction trans)
		{
			for (int ix = _count - 1; ix >= 0; ix--)
			{
				if (IndexIsValid(trans, ix))
				{
					return ix;
				}
			}
			return -1;
		}

		public bool IndexIsValid(Transaction trans, int index)
		{
			if (!CanWrite())
			{
				return true;
			}
			BTreePatch patch = KeyPatch(index);
			if (patch == null)
			{
				return true;
			}
			return patch.Key(trans) != No4.Instance;
		}

		private object FirstKey(Transaction trans)
		{
			int index = FirstKeyIndex(trans);
			if (-1 == index)
			{
				return No4.Instance;
			}
			return InternalKey(trans, index);
		}

		public override byte GetIdentifier()
		{
			return Const4.BtreeNode;
		}

		private void PrepareInsert(int pos)
		{
			if (pos > LastIndex())
			{
				_count++;
				return;
			}
			int len = _count - pos;
			System.Array.Copy(_keys, pos, _keys, pos + 1, len);
			if (_children != null)
			{
				System.Array.Copy(_children, pos, _children, pos + 1, len);
			}
			_count++;
		}

		private void Remove(int pos)
		{
			if (DTrace.enabled)
			{
				DTrace.BtreeNodeRemove.Log(GetID());
			}
			int len = _count - pos;
			_count--;
			System.Array.Copy(_keys, pos + 1, _keys, pos, len);
			_keys[_count] = null;
			if (_children != null)
			{
				System.Array.Copy(_children, pos + 1, _children, pos, len);
				_children[_count] = null;
			}
		}

		internal object Key(int index)
		{
			object obj = _keys[index];
			if (obj is BTreePatch)
			{
				return ((BTreePatch)obj).GetObject();
			}
			return obj;
		}

		public object Key(Transaction trans, int index)
		{
			return Key(trans, PrepareRead(trans), index);
		}

		internal object Key(Transaction trans, ByteArrayBuffer reader, int index)
		{
			if (CanWrite())
			{
				return InternalKey(trans, index);
			}
			if (reader == null)
			{
				reader = PrepareRead(trans);
			}
			if (CanWrite())
			{
				return InternalKey(trans, index);
			}
			SeekKey(reader, index);
			return KeyHandler().ReadIndexEntry(trans.Context(), reader);
		}

		private object InternalKey(Transaction trans, int index)
		{
			BTreePatch patch = KeyPatch(index);
			if (patch == null)
			{
				return _keys[index];
			}
			return patch.Key(trans);
		}

		private BTreePatch KeyPatch(int index)
		{
			object obj = _keys[index];
			if (obj is BTreePatch)
			{
				return (BTreePatch)obj;
			}
			return null;
		}

		internal BTreePatch KeyPatch(Transaction trans, int index)
		{
			object obj = _keys[index];
			if (obj is BTreePatch)
			{
				return ((BTreePatch)obj).ForTransaction(trans);
			}
			return null;
		}

		private IIndexable4 KeyHandler()
		{
			return _btree.KeyHandler();
		}

		public override int OwnLength()
		{
			return SlotLeadingLength + (_count * EntryLength()) + Const4.BracketsBytes;
		}

		internal ByteArrayBuffer PrepareRead(Transaction trans)
		{
			BTreeNodeCacheEntry cacheEntry = Btree().CacheEntry(this);
			if (CanWrite())
			{
				return null;
			}
			if (IsNew())
			{
				return null;
			}
			Transaction systemTransaction = trans.SystemTransaction();
			ByteArrayBuffer buffer = cacheEntry.Buffer();
			if (buffer != null)
			{
				// Cache hit, still unread
				buffer.Seek(0);
				Read(systemTransaction, buffer);
				cacheEntry.Buffer(null);
				_btree.AddToProcessing(this);
				return null;
			}
			buffer = ProduceReadBuffer(systemTransaction);
			ReadNodeHeader(buffer);
			cacheEntry.Buffer(buffer);
			return buffer;
		}

		internal void PrepareWrite(Transaction trans)
		{
			if (_dead)
			{
				return;
			}
			BTreeNodeCacheEntry cacheEntry = Btree().CacheEntry(this);
			if (CanWrite())
			{
				return;
			}
			ByteArrayBuffer buffer = cacheEntry.Buffer();
			if (buffer != null)
			{
				buffer.Seek(0);
				Read(trans.SystemTransaction(), buffer);
				cacheEntry.Buffer(null);
			}
			else
			{
				Read(trans.SystemTransaction());
			}
			_btree.AddToProcessing(this);
		}

		private void PrepareArrays()
		{
			if (CanWrite())
			{
				return;
			}
			_keys = new object[_btree.NodeSize()];
			if (!_isLeaf)
			{
				_children = new object[_btree.NodeSize()];
			}
		}

		private void ReadNodeHeader(ByteArrayBuffer reader)
		{
			_count = reader.ReadInt();
			byte leafByte = reader.ReadByte();
			_isLeaf = (leafByte == 1);
			_parentID = reader.ReadInt();
			_previousID = reader.ReadInt();
			_nextID = reader.ReadInt();
		}

		public override void ReadThis(Transaction trans, ByteArrayBuffer reader)
		{
			ReadNodeHeader(reader);
			PrepareArrays();
			bool isInner = !_isLeaf;
			for (int i = 0; i < _count; i++)
			{
				_keys[i] = KeyHandler().ReadIndexEntry(trans.Context(), reader);
				if (isInner)
				{
					_children[i] = reader.ReadInt();
				}
			}
		}

		public void Remove(Transaction trans, int index)
		{
			if (!_isLeaf)
			{
				throw new InvalidOperationException();
			}
			PrepareWrite(trans);
			SetStateDirty();
			object obj = null;
			BTreePatch patch = KeyPatch(index);
			if (patch == null)
			{
				obj = _keys[index];
			}
			else
			{
				BTreePatch transPatch = patch.ForTransaction(trans);
				if (transPatch != null)
				{
					obj = transPatch.GetObject();
				}
				else
				{
					// There could be more than one patch with different object
					// identities. We have no means to determine a "best" object 
					// so we just take any one. Could be problematic.
					obj = patch.GetObject();
				}
			}
			Remove(trans, obj, index);
		}

		public bool Remove(Transaction trans, object obj, int index)
		{
			if (!_isLeaf)
			{
				throw new InvalidOperationException();
			}
			PrepareWrite(trans);
			SetStateDirty();
			BTreePatch patch = KeyPatch(index);
			// no patch, no problem, can remove
			if (patch == null)
			{
				_keys[index] = ApplyNewRemovePatch(trans, obj);
				KeyChanged(trans, index);
				return true;
			}
			BTreePatch transPatch = patch.ForTransaction(trans);
			if (transPatch != null)
			{
				if (transPatch.IsAdd())
				{
					CancelAdding(trans, index);
					return true;
				}
				if (transPatch.IsCancelledRemoval())
				{
					BTreeRemove removePatch = ApplyNewRemovePatch(trans, transPatch.GetObject());
					_keys[index] = ((BTreeUpdate)patch).ReplacePatch(transPatch, removePatch);
					KeyChanged(trans, index);
					return true;
				}
			}
			else
			{
				// If the patch is a removal of a cancelled removal for another
				// transaction, we need one for this transaction also.
				if (!patch.IsAdd())
				{
					((BTreeUpdate)patch).Append(ApplyNewRemovePatch(trans, obj));
					return true;
				}
			}
			return false;
		}

		public void Remove(Transaction trans, IPreparedComparison preparedComparison, object
			 obj, int index)
		{
			if (Remove(trans, obj, index))
			{
				return;
			}
			// now we try if removal is OK for the next element in this node
			if (index != LastIndex())
			{
				if (CompareInWriteMode(preparedComparison, index + 1) != 0)
				{
					return;
				}
				Remove(trans, preparedComparison, obj, index + 1);
				return;
			}
			// nothing else worked so far, move on to the next node, try there
			Db4objects.Db4o.Internal.Btree.BTreeNode node = NextNode();
			if (node == null)
			{
				return;
			}
			node.PrepareWrite(trans);
			if (node.CompareInWriteMode(preparedComparison, 0) != 0)
			{
				return;
			}
			node.Remove(trans, preparedComparison, obj, 0);
		}

		private void CancelAdding(Transaction trans, int index)
		{
			_btree.NotifyRemoveListener(new TransactionContext(trans, KeyPatch(index).GetObject
				()));
			if (FreeIfEmpty(trans, _count - 1))
			{
				SizeDecrement(trans);
				return;
			}
			Remove(index);
			KeyChanged(trans, index);
			SizeDecrement(trans);
		}

		private void SizeDecrement(Transaction trans)
		{
			_btree.SizeChanged(trans, this, -1);
		}

		private int LastIndex()
		{
			return _count - 1;
		}

		private BTreeRemove ApplyNewRemovePatch(Transaction trans, object key)
		{
			SizeDecrement(trans);
			return new BTreeRemove(trans, key);
		}

		private void KeyChanged(Transaction trans, int index)
		{
			if (index == 0)
			{
				TellParentAboutChangedKey(trans);
			}
		}

		internal void Rollback(Transaction trans)
		{
			CommitOrRollback(trans, false);
		}

		private Searcher Search(Transaction trans, IPreparedComparison preparedComparison
			, ByteArrayBuffer reader)
		{
			return Search(trans, preparedComparison, reader, SearchTarget.Any);
		}

		private Searcher Search(Transaction trans, IPreparedComparison preparedComparison
			, ByteArrayBuffer reader, SearchTarget target)
		{
			Searcher s = new Searcher(target, _count);
			if (CanWrite())
			{
				while (s.Incomplete())
				{
					s.ResultIs(CompareInWriteMode(preparedComparison, s.Cursor()));
				}
			}
			else
			{
				while (s.Incomplete())
				{
					s.ResultIs(CompareInReadMode(trans, preparedComparison, reader, s.Cursor()));
				}
			}
			return s;
		}

		private void SeekAfterKey(ByteArrayBuffer reader, int ix)
		{
			SeekKey(reader, ix);
			reader._offset += KeyHandler().LinkLength();
		}

		private void SeekChild(ByteArrayBuffer reader, int ix)
		{
			SeekAfterKey(reader, ix);
		}

		private void SeekKey(ByteArrayBuffer reader, int ix)
		{
			reader._offset = SlotLeadingLength + (EntryLength() * ix);
		}

		private Db4objects.Db4o.Internal.Btree.BTreeNode Split(Transaction trans)
		{
			Db4objects.Db4o.Internal.Btree.BTreeNode res = new Db4objects.Db4o.Internal.Btree.BTreeNode
				(_btree, _btree._halfNodeSize, _isLeaf, _parentID, GetID(), _nextID);
			System.Array.Copy(_keys, _btree._halfNodeSize, res._keys, 0, _btree._halfNodeSize
				);
			for (int i = _btree._halfNodeSize; i < _keys.Length; i++)
			{
				_keys[i] = null;
			}
			if (_children != null)
			{
				res._children = new object[_btree.NodeSize()];
				System.Array.Copy(_children, _btree._halfNodeSize, res._children, 0, _btree._halfNodeSize
					);
				for (int i = _btree._halfNodeSize; i < _children.Length; i++)
				{
					_children[i] = null;
				}
			}
			_count = _btree._halfNodeSize;
			res.Write(trans.SystemTransaction());
			_btree.AddNode(res);
			int splitID = res.GetID();
			PointNextTo(trans, splitID);
			SetNextID(trans, splitID);
			if (_children != null)
			{
				for (int i = 0; i < _btree._halfNodeSize; i++)
				{
					if (res._children[i] == null)
					{
						break;
					}
					res.Child(i).SetParentID(trans, splitID);
				}
			}
			_btree.NotifySplit(trans, this, res);
			return res;
		}

		private void PointNextTo(Transaction trans, int id)
		{
			if (_nextID != 0)
			{
				NextNode().SetPreviousID(trans, id);
			}
		}

		private void PointPreviousTo(Transaction trans, int id)
		{
			if (_previousID != 0)
			{
				PreviousNode().SetNextID(trans, id);
			}
		}

		public Db4objects.Db4o.Internal.Btree.BTreeNode PreviousNode()
		{
			if (_previousID == 0)
			{
				return null;
			}
			return _btree.ProduceNode(_previousID);
		}

		public Db4objects.Db4o.Internal.Btree.BTreeNode NextNode()
		{
			if (_nextID == 0)
			{
				return null;
			}
			return _btree.ProduceNode(_nextID);
		}

		internal BTreePointer FirstPointer(Transaction trans)
		{
			ByteArrayBuffer reader = PrepareRead(trans);
			if (_isLeaf)
			{
				return LeafFirstPointer(trans, reader);
			}
			return BranchFirstPointer(trans, reader);
		}

		private BTreePointer BranchFirstPointer(Transaction trans, ByteArrayBuffer reader
			)
		{
			for (int i = 0; i < _count; i++)
			{
				BTreePointer childFirstPointer = Child(reader, i).FirstPointer(trans);
				if (childFirstPointer != null)
				{
					return childFirstPointer;
				}
			}
			return null;
		}

		private BTreePointer LeafFirstPointer(Transaction trans, ByteArrayBuffer reader)
		{
			int index = FirstKeyIndex(trans);
			if (index == -1)
			{
				return null;
			}
			return new BTreePointer(trans, reader, this, index);
		}

		public BTreePointer LastPointer(Transaction trans)
		{
			ByteArrayBuffer reader = PrepareRead(trans);
			if (_isLeaf)
			{
				return LeafLastPointer(trans, reader);
			}
			return BranchLastPointer(trans, reader);
		}

		private BTreePointer BranchLastPointer(Transaction trans, ByteArrayBuffer reader)
		{
			for (int i = _count - 1; i >= 0; i--)
			{
				BTreePointer childLastPointer = Child(reader, i).LastPointer(trans);
				if (childLastPointer != null)
				{
					return childLastPointer;
				}
			}
			return null;
		}

		private BTreePointer LeafLastPointer(Transaction trans, ByteArrayBuffer reader)
		{
			int index = LastKeyIndex(trans);
			if (index == -1)
			{
				return null;
			}
			return new BTreePointer(trans, reader, this, index);
		}

		public void Purge()
		{
			if (_dead)
			{
				_keys = null;
				_children = null;
				return;
			}
			if (!IsPatched())
			{
				return;
			}
			HoldChildrenAsIDs();
			_btree.AddNode(this);
		}

		private bool IsPatched()
		{
			if (_dead)
			{
				return false;
			}
			if (!CanWrite())
			{
				return false;
			}
			for (int i = 0; i < _count; i++)
			{
				if (_keys[i] is BTreePatch)
				{
					return true;
				}
			}
			return false;
		}

		private void SetParentID(Transaction trans, int id)
		{
			PrepareWrite(trans);
			SetStateDirty();
			_parentID = id;
		}

		private void SetPreviousID(Transaction trans, int id)
		{
			PrepareWrite(trans);
			SetStateDirty();
			_previousID = id;
		}

		private void SetNextID(Transaction trans, int id)
		{
			PrepareWrite(trans);
			SetStateDirty();
			_nextID = id;
		}

		public void TraverseKeys(Transaction trans, IVisitor4 visitor)
		{
			ByteArrayBuffer reader = PrepareRead(trans);
			if (_isLeaf)
			{
				for (int i = 0; i < _count; i++)
				{
					object obj = Key(trans, reader, i);
					if (obj != No4.Instance)
					{
						visitor.Visit(obj);
					}
				}
			}
			else
			{
				for (int i = 0; i < _count; i++)
				{
					Child(reader, i).TraverseKeys(trans, visitor);
				}
			}
		}

		public override bool WriteObjectBegin()
		{
			if (_dead)
			{
				return false;
			}
			if (!CanWrite())
			{
				return false;
			}
			return base.WriteObjectBegin();
		}

		public override void WriteThis(Transaction trans, ByteArrayBuffer buffer)
		{
			int count = 0;
			int startOffset = buffer._offset;
			IContext context = trans.Context();
			buffer.IncrementOffset(CountLeafAnd3LinkLength);
			if (_isLeaf)
			{
				for (int i = 0; i < _count; i++)
				{
					object obj = InternalKey(trans, i);
					if (obj != No4.Instance)
					{
						count++;
						KeyHandler().WriteIndexEntry(context, buffer, obj);
					}
				}
			}
			else
			{
				for (int i = 0; i < _count; i++)
				{
					if (ChildCanSupplyFirstKey(i))
					{
						Db4objects.Db4o.Internal.Btree.BTreeNode child = (Db4objects.Db4o.Internal.Btree.BTreeNode
							)_children[i];
						object childKey = child.FirstKey(trans);
						if (childKey != No4.Instance)
						{
							count++;
							KeyHandler().WriteIndexEntry(context, buffer, childKey);
							buffer.WriteIDOf(trans, child);
						}
					}
					else
					{
						count++;
						KeyHandler().WriteIndexEntry(context, buffer, Key(i));
						buffer.WriteIDOf(trans, _children[i]);
					}
				}
			}
			int endOffset = buffer._offset;
			buffer._offset = startOffset;
			buffer.WriteInt(count);
			buffer.WriteByte(_isLeaf ? (byte)1 : (byte)0);
			buffer.WriteInt(_parentID);
			buffer.WriteInt(_previousID);
			buffer.WriteInt(_nextID);
			buffer._offset = endOffset;
		}

		public override string ToString()
		{
			if (_count == 0)
			{
				return "Node " + GetID() + " not loaded";
			}
			string str = "\nBTreeNode";
			str += "\nid: " + GetID();
			str += "\nparent: " + _parentID;
			str += "\nprevious: " + _previousID;
			str += "\nnext: " + _nextID;
			str += "\ncount:" + _count;
			str += "\nleaf:" + _isLeaf + "\n";
			if (CanWrite())
			{
				str += " { ";
				bool first = true;
				for (int i = 0; i < _count; i++)
				{
					if (_keys[i] != null)
					{
						if (!first)
						{
							str += ", ";
						}
						str += _keys[i].ToString();
						first = false;
					}
				}
				str += " }";
			}
			return str;
		}

		public void DebugLoadFully(Transaction trans)
		{
			PrepareWrite(trans);
			if (_isLeaf)
			{
				return;
			}
			for (int i = 0; i < _count; ++i)
			{
				if (_children[i] is int)
				{
					_children[i] = Btree().ProduceNode(((int)_children[i]));
				}
				((Db4objects.Db4o.Internal.Btree.BTreeNode)_children[i]).DebugLoadFully(trans);
			}
		}

		public static void DefragIndex(DefragmentContextImpl context, IIndexable4 keyHandler
			)
		{
			// count
			int count = context.ReadInt();
			// leafByte
			byte leafByte = context.ReadByte();
			bool isLeaf = (leafByte == 1);
			context.CopyID();
			// parent ID
			context.CopyID();
			// previous ID
			context.CopyID();
			// next ID
			for (int i = 0; i < count; i++)
			{
				keyHandler.DefragIndexEntry(context);
				if (!isLeaf)
				{
					context.CopyID();
				}
			}
		}

		public bool IsLeaf()
		{
			return _isLeaf;
		}

		/// <summary>This traversal goes over all nodes, not just leafs</summary>
		internal void TraverseAllNodes(Transaction trans, IVisitor4 command)
		{
			ByteArrayBuffer reader = PrepareRead(trans);
			command.Visit(this);
			if (_isLeaf)
			{
				return;
			}
			for (int childIdx = 0; childIdx < _count; childIdx++)
			{
				Child(reader, childIdx).TraverseAllNodes(trans, command);
			}
		}

		public int Size(Transaction trans)
		{
			PrepareRead(trans);
			if (!CanWrite())
			{
				return _count;
			}
			int size = 0;
			for (int i = 0; i < _count; i++)
			{
				BTreePatch keyPatch = KeyPatch(i);
				if (keyPatch != null)
				{
					size += keyPatch.SizeDiff(trans);
				}
				else
				{
					size++;
				}
			}
			return size;
		}

		public override Db4objects.Db4o.Internal.Slots.SlotChangeFactory SlotChangeFactory
			()
		{
			return _btree.SlotChangeFactory();
		}

		public override ITransactionalIdSystem IdSystem(Transaction trans)
		{
			return _btree.IdSystem(trans);
		}

		public void ToReadMode()
		{
			if (IsNew())
			{
				return;
			}
			if (!CanWrite())
			{
				return;
			}
			if (IsDirty())
			{
				return;
			}
			if (IsPatched())
			{
				return;
			}
			_keys = null;
			_children = null;
		}
	}
}
