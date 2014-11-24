/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;

namespace Db4objects.Db4o.Internal.Btree
{
	/// <exclude></exclude>
	public sealed class BTreePointer
	{
		public static Db4objects.Db4o.Internal.Btree.BTreePointer Max(Db4objects.Db4o.Internal.Btree.BTreePointer
			 x, Db4objects.Db4o.Internal.Btree.BTreePointer y)
		{
			if (x == null)
			{
				return x;
			}
			if (y == null)
			{
				return y;
			}
			if (x.CompareTo(y) > 0)
			{
				return x;
			}
			return y;
		}

		public static Db4objects.Db4o.Internal.Btree.BTreePointer Min(Db4objects.Db4o.Internal.Btree.BTreePointer
			 x, Db4objects.Db4o.Internal.Btree.BTreePointer y)
		{
			if (x == null)
			{
				return y;
			}
			if (y == null)
			{
				return x;
			}
			if (x.CompareTo(y) < 0)
			{
				return x;
			}
			return y;
		}

		private BTreeNode _node;

		private int _index;

		private readonly Transaction _transaction;

		private ByteArrayBuffer _nodeReader;

		public Db4objects.Db4o.Internal.Btree.BTreePointer ShallowClone()
		{
			return new Db4objects.Db4o.Internal.Btree.BTreePointer(_transaction, _nodeReader, 
				_node, _index);
		}

		public void CopyTo(Db4objects.Db4o.Internal.Btree.BTreePointer target)
		{
			target._node = _node;
			target._index = _index;
			target._nodeReader = _nodeReader;
		}

		public BTreePointer(Transaction transaction, ByteArrayBuffer nodeReader, BTreeNode
			 node, int index)
		{
			if (transaction == null || node == null)
			{
				throw new ArgumentNullException();
			}
			_transaction = transaction;
			_nodeReader = nodeReader;
			_node = node;
			_index = index;
		}

		public int Index()
		{
			return _index;
		}

		public BTreeNode Node()
		{
			return _node;
		}

		public object Key()
		{
			return _node.Key(_transaction, _nodeReader, _index);
		}

		public Db4objects.Db4o.Internal.Btree.BTreePointer Next()
		{
			int indexInMyNode = _index + 1;
			while (indexInMyNode < _node.Count())
			{
				if (_node.IndexIsValid(_transaction, indexInMyNode))
				{
					return new Db4objects.Db4o.Internal.Btree.BTreePointer(_transaction, _nodeReader, 
						_node, indexInMyNode);
				}
				indexInMyNode++;
			}
			int newIndex = -1;
			BTreeNode nextNode = _node;
			ByteArrayBuffer nextReader = null;
			while (newIndex == -1)
			{
				nextNode = nextNode.NextNode();
				if (nextNode == null)
				{
					return null;
				}
				nextReader = nextNode.PrepareRead(_transaction);
				newIndex = nextNode.FirstKeyIndex(_transaction);
			}
			Btree().ConvertCacheEvictedNodesToReadMode();
			return new Db4objects.Db4o.Internal.Btree.BTreePointer(_transaction, nextReader, 
				nextNode, newIndex);
		}

		/// <summary>
		/// Duplicate code of next(), reusing this BTreePointer without creating a
		/// new BTreePointer, very dangerous to call because there may be side
		/// effects if this BTreePointer is used elsewhere and code relies on
		/// state the stay the same.
		/// </summary>
		/// <remarks>
		/// Duplicate code of next(), reusing this BTreePointer without creating a
		/// new BTreePointer, very dangerous to call because there may be side
		/// effects if this BTreePointer is used elsewhere and code relies on
		/// state the stay the same.
		/// </remarks>
		public Db4objects.Db4o.Internal.Btree.BTreePointer UnsafeFastNext()
		{
			int indexInMyNode = _index + 1;
			while (indexInMyNode < _node.Count())
			{
				if (_node.IndexIsValid(_transaction, indexInMyNode))
				{
					_index = indexInMyNode;
					return this;
				}
				indexInMyNode++;
			}
			int newIndex = -1;
			BTreeNode nextNode = _node;
			ByteArrayBuffer nextReader = null;
			while (newIndex == -1)
			{
				nextNode = nextNode.NextNode();
				if (nextNode == null)
				{
					return null;
				}
				nextReader = nextNode.PrepareRead(_transaction);
				newIndex = nextNode.FirstKeyIndex(_transaction);
			}
			Btree().ConvertCacheEvictedNodesToReadMode();
			_nodeReader = nextReader;
			_node = nextNode;
			_index = newIndex;
			return this;
		}

		public Db4objects.Db4o.Internal.Btree.BTreePointer Previous()
		{
			int indexInMyNode = _index - 1;
			while (indexInMyNode >= 0)
			{
				if (_node.IndexIsValid(_transaction, indexInMyNode))
				{
					return new Db4objects.Db4o.Internal.Btree.BTreePointer(_transaction, _nodeReader, 
						_node, indexInMyNode);
				}
				indexInMyNode--;
			}
			int newIndex = -1;
			BTreeNode previousNode = _node;
			ByteArrayBuffer previousReader = null;
			while (newIndex == -1)
			{
				previousNode = previousNode.PreviousNode();
				if (previousNode == null)
				{
					return null;
				}
				previousReader = previousNode.PrepareRead(_transaction);
				newIndex = previousNode.LastKeyIndex(_transaction);
			}
			return new Db4objects.Db4o.Internal.Btree.BTreePointer(_transaction, previousReader
				, previousNode, newIndex);
		}

		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			if (!(obj is Db4objects.Db4o.Internal.Btree.BTreePointer))
			{
				return false;
			}
			Db4objects.Db4o.Internal.Btree.BTreePointer other = (Db4objects.Db4o.Internal.Btree.BTreePointer
				)obj;
			if (_index != other._index)
			{
				return false;
			}
			return _node.Equals(other._node);
		}

		public override int GetHashCode()
		{
			return _node.GetHashCode();
		}

		public override string ToString()
		{
			return "BTreePointer(index=" + _index + ", node=" + _node + ")";
		}

		public int CompareTo(Db4objects.Db4o.Internal.Btree.BTreePointer y)
		{
			if (null == y)
			{
				throw new ArgumentNullException();
			}
			if (Btree() != y.Btree())
			{
				throw new ArgumentException();
			}
			return Btree().CompareKeys(_transaction.Context(), Key(), y.Key());
		}

		private BTree Btree()
		{
			return _node.Btree();
		}

		public static bool LessThan(Db4objects.Db4o.Internal.Btree.BTreePointer x, Db4objects.Db4o.Internal.Btree.BTreePointer
			 y)
		{
			return Db4objects.Db4o.Internal.Btree.BTreePointer.Min(x, y) == x && !Equals(x, y
				);
		}

		public static bool Equals(Db4objects.Db4o.Internal.Btree.BTreePointer x, Db4objects.Db4o.Internal.Btree.BTreePointer
			 y)
		{
			if (x == null)
			{
				return y == null;
			}
			return x.Equals(y);
		}

		public bool IsValid()
		{
			return _node.IndexIsValid(_transaction, _index);
		}
	}
}
