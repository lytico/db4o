/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	/// <exclude></exclude>
	public abstract class Tree : IShallowClone, IDeepClone, IVisitable
	{
		public Tree _preceding;

		public int _size = 1;

		public Tree _subsequent;

		public static Tree Add(Tree oldTree, Tree newTree)
		{
			if (oldTree == null)
			{
				return newTree;
			}
			return (Tree)((Tree)oldTree).Add(newTree);
		}

		public Tree Add(Tree newNode)
		{
			return Add(newNode, Compare(newNode));
		}

		/// <summary>
		/// On adding a node to a tree, if it already exists, and if
		/// Tree#duplicates() returns false, #isDuplicateOf() will be
		/// called.
		/// </summary>
		/// <remarks>
		/// On adding a node to a tree, if it already exists, and if
		/// Tree#duplicates() returns false, #isDuplicateOf() will be
		/// called. The added node can then be asked for the node that
		/// prevails in the tree using #duplicateOrThis(). This mechanism
		/// allows doing find() and add() in one run.
		/// </remarks>
		public virtual Tree Add(Tree newNode, int cmp)
		{
			if (cmp < 0)
			{
				if (_subsequent == null)
				{
					_subsequent = newNode;
					_size++;
				}
				else
				{
					_subsequent = _subsequent.Add(newNode);
					if (_preceding == null)
					{
						return (Tree)RotateLeft();
					}
					return (Tree)Balance();
				}
			}
			else
			{
				if (cmp > 0 || ((Tree)newNode).Duplicates())
				{
					if (_preceding == null)
					{
						_preceding = newNode;
						_size++;
					}
					else
					{
						_preceding = _preceding.Add(newNode);
						if (_subsequent == null)
						{
							return (Tree)RotateRight();
						}
						return (Tree)Balance();
					}
				}
				else
				{
					return (Tree)((Tree)newNode).OnAttemptToAddDuplicate(this);
				}
			}
			return (Tree)this;
		}

		/// <summary>
		/// On adding a node to a tree, if it already exists, and if
		/// Tree#duplicates() returns false, #onAttemptToAddDuplicate()
		/// will be called and the existing node will be stored in
		/// this._preceding.
		/// </summary>
		/// <remarks>
		/// On adding a node to a tree, if it already exists, and if
		/// Tree#duplicates() returns false, #onAttemptToAddDuplicate()
		/// will be called and the existing node will be stored in
		/// this._preceding.
		/// This node node can then be asked for the node that prevails
		/// in the tree on adding, using the #addedOrExisting() method.
		/// This mechanism allows doing find() and add() in one run.
		/// </remarks>
		public virtual Tree AddedOrExisting()
		{
			if (WasAddedToTree())
			{
				return this;
			}
			return _preceding;
		}

		public virtual bool WasAddedToTree()
		{
			return _size != 0;
		}

		public Tree Balance()
		{
			int cmp = _subsequent.Nodes() - _preceding.Nodes();
			if (cmp < -2)
			{
				return RotateRight();
			}
			else
			{
				if (cmp > 2)
				{
					return RotateLeft();
				}
				else
				{
					SetSizeOwnPrecedingSubsequent();
					return this;
				}
			}
		}

		public virtual Tree BalanceCheckNulls()
		{
			if (_subsequent == null)
			{
				if (_preceding == null)
				{
					SetSizeOwn();
					return this;
				}
				return RotateRight();
			}
			else
			{
				if (_preceding == null)
				{
					return RotateLeft();
				}
			}
			return Balance();
		}

		public virtual void CalculateSize()
		{
			if (_preceding == null)
			{
				if (_subsequent == null)
				{
					SetSizeOwn();
				}
				else
				{
					SetSizeOwnSubsequent();
				}
			}
			else
			{
				if (_subsequent == null)
				{
					SetSizeOwnPreceding();
				}
				else
				{
					SetSizeOwnPrecedingSubsequent();
				}
			}
		}

		/// <summary>
		/// returns 0, if keys are equal
		/// uses this - other
		/// returns positive if this is greater than a_to
		/// returns negative if this is smaller than a_to
		/// </summary>
		public abstract int Compare(Tree a_to);

		public static Tree DeepClone(Tree a_tree, object a_param)
		{
			if (a_tree == null)
			{
				return null;
			}
			Tree newNode = (Tree)a_tree.DeepClone(a_param);
			newNode._size = a_tree._size;
			newNode._preceding = Tree.DeepClone(((Tree)a_tree._preceding), a_param);
			newNode._subsequent = Tree.DeepClone(((Tree)a_tree._subsequent), a_param);
			return newNode;
		}

		public virtual object DeepClone(object a_param)
		{
			return ShallowClone();
		}

		public virtual bool Duplicates()
		{
			return true;
		}

		public Tree Filter(IPredicate4 a_filter)
		{
			if (_preceding != null)
			{
				_preceding = _preceding.Filter(a_filter);
			}
			if (_subsequent != null)
			{
				_subsequent = _subsequent.Filter(a_filter);
			}
			if (!a_filter.Match(this))
			{
				return Remove();
			}
			return this;
		}

		public static Tree Find(Tree inTree, Tree template)
		{
			if (inTree == null)
			{
				return null;
			}
			return inTree.Find(template);
		}

		public Tree Find(Tree template)
		{
			Tree current = this;
			while (true)
			{
				int comparisonResult = current.Compare(template);
				if (comparisonResult == 0)
				{
					return current;
				}
				if (comparisonResult < 0)
				{
					current = ((Tree)current._subsequent);
				}
				else
				{
					current = ((Tree)current._preceding);
				}
				if (current == null)
				{
					return null;
				}
			}
		}

		public static Tree FindGreaterOrEqual(Tree a_in, Tree a_finder)
		{
			if (a_in == null)
			{
				return null;
			}
			int cmp = a_in.Compare(a_finder);
			if (cmp == 0)
			{
				return a_in;
			}
			// the highest node in the hierarchy !!!
			if (cmp > 0)
			{
				Tree node = FindGreaterOrEqual(((Tree)a_in._preceding), a_finder);
				if (node != null)
				{
					return node;
				}
				return a_in;
			}
			return FindGreaterOrEqual(((Tree)a_in._subsequent), a_finder);
		}

		public static Tree FindSmaller(Tree a_in, Tree a_node)
		{
			if (a_in == null)
			{
				return null;
			}
			int cmp = a_in.Compare(a_node);
			if (cmp < 0)
			{
				Tree node = FindSmaller(((Tree)a_in._subsequent), a_node);
				if (node != null)
				{
					return node;
				}
				return a_in;
			}
			return FindSmaller(((Tree)a_in._preceding), a_node);
		}

		public Tree First()
		{
			if (_preceding == null)
			{
				return this;
			}
			return _preceding.First();
		}

		public static Tree Last(Tree tree)
		{
			if (tree == null)
			{
				return null;
			}
			return tree.Last();
		}

		public Tree Last()
		{
			if (_subsequent == null)
			{
				return this;
			}
			return _subsequent.Last();
		}

		public virtual Tree OnAttemptToAddDuplicate(Tree oldNode)
		{
			_size = 0;
			_preceding = oldNode;
			return oldNode;
		}

		/// <returns>the number of nodes in this tree for balancing</returns>
		public virtual int Nodes()
		{
			return _size;
		}

		public virtual int OwnSize()
		{
			return 1;
		}

		public virtual Tree Remove()
		{
			if (_subsequent != null && _preceding != null)
			{
				_subsequent = _subsequent.RotateSmallestUp();
				_subsequent._preceding = _preceding;
				_subsequent.CalculateSize();
				return _subsequent;
			}
			if (_subsequent != null)
			{
				return _subsequent;
			}
			return _preceding;
		}

		public virtual void RemoveChildren()
		{
			_preceding = null;
			_subsequent = null;
			SetSizeOwn();
		}

		public virtual Tree RemoveFirst()
		{
			if (_preceding == null)
			{
				return _subsequent;
			}
			_preceding = _preceding.RemoveFirst();
			CalculateSize();
			return this;
		}

		public static Tree RemoveLike(Tree from, Tree a_find)
		{
			if (from == null)
			{
				return null;
			}
			return from.RemoveLike(a_find);
		}

		public Tree RemoveLike(Tree a_find)
		{
			int cmp = Compare(a_find);
			if (cmp == 0)
			{
				return (Tree)Remove();
			}
			if (cmp > 0)
			{
				if (_preceding != null)
				{
					_preceding = _preceding.RemoveLike(a_find);
				}
			}
			else
			{
				if (_subsequent != null)
				{
					_subsequent = _subsequent.RemoveLike(a_find);
				}
			}
			CalculateSize();
			return (Tree)this;
		}

		public Tree RemoveNode(Tree a_tree)
		{
			if (this == a_tree)
			{
				return Remove();
			}
			int cmp = Compare(a_tree);
			if (cmp >= 0)
			{
				if (_preceding != null)
				{
					_preceding = _preceding.RemoveNode(a_tree);
				}
			}
			if (cmp <= 0)
			{
				if (_subsequent != null)
				{
					_subsequent = _subsequent.RemoveNode(a_tree);
				}
			}
			CalculateSize();
			return this;
		}

		public Tree RotateLeft()
		{
			Tree tree = _subsequent;
			_subsequent = ((Tree)tree._preceding);
			CalculateSize();
			tree._preceding = this;
			if (((Tree)tree._subsequent) == null)
			{
				tree.SetSizeOwnPlus(this);
			}
			else
			{
				tree.SetSizeOwnPlus(this, ((Tree)tree._subsequent));
			}
			return tree;
		}

		public Tree RotateRight()
		{
			Tree tree = _preceding;
			_preceding = ((Tree)tree._subsequent);
			CalculateSize();
			tree._subsequent = this;
			if (((Tree)tree._preceding) == null)
			{
				tree.SetSizeOwnPlus(this);
			}
			else
			{
				tree.SetSizeOwnPlus(this, ((Tree)tree._preceding));
			}
			return tree;
		}

		private Tree RotateSmallestUp()
		{
			if (_preceding != null)
			{
				_preceding = _preceding.RotateSmallestUp();
				return RotateRight();
			}
			return this;
		}

		public void SetSizeOwn()
		{
			_size = OwnSize();
		}

		public void SetSizeOwnPrecedingSubsequent()
		{
			_size = OwnSize() + _preceding._size + _subsequent._size;
		}

		public void SetSizeOwnPreceding()
		{
			_size = OwnSize() + _preceding._size;
		}

		public void SetSizeOwnSubsequent()
		{
			_size = OwnSize() + _subsequent._size;
		}

		public void SetSizeOwnPlus(Tree tree)
		{
			_size = OwnSize() + tree._size;
		}

		public void SetSizeOwnPlus(Tree tree1, Tree tree2)
		{
			_size = OwnSize() + tree1._size + tree2._size;
		}

		public static int Size(Tree a_tree)
		{
			if (a_tree == null)
			{
				return 0;
			}
			return a_tree.Size();
		}

		/// <returns>the number of objects represented.</returns>
		public virtual int Size()
		{
			return _size;
		}

		public static void Traverse(Tree tree, IVisitor4 visitor)
		{
			if (tree == null)
			{
				return;
			}
			tree.Traverse(visitor);
		}

		/// <summary>Traverses a tree with a starting point node.</summary>
		/// <remarks>
		/// Traverses a tree with a starting point node.
		/// If there is no exact match for the starting node, the next higher will be taken.
		/// </remarks>
		public static void Traverse(Tree tree, Tree startingNode, ICancellableVisitor4 visitor
			)
		{
			if (tree == null)
			{
				return;
			}
			tree.Traverse(startingNode, visitor);
		}

		private bool Traverse(Tree startingNode, ICancellableVisitor4 visitor)
		{
			if (startingNode != null)
			{
				int cmp = Compare(startingNode);
				if (cmp < 0)
				{
					if (_subsequent != null)
					{
						return _subsequent.Traverse(startingNode, visitor);
					}
					return true;
				}
				else
				{
					if (cmp > 0)
					{
						if (_preceding != null)
						{
							if (!_preceding.Traverse(startingNode, visitor))
							{
								return false;
							}
						}
					}
				}
			}
			else
			{
				if (_preceding != null)
				{
					if (!_preceding.Traverse(null, visitor))
					{
						return false;
					}
				}
			}
			if (!visitor.Visit(this))
			{
				return false;
			}
			if (_subsequent != null)
			{
				if (!_subsequent.Traverse(null, visitor))
				{
					return false;
				}
			}
			return true;
		}

		public void Traverse(IVisitor4 visitor)
		{
			if (_preceding != null)
			{
				_preceding.Traverse(visitor);
			}
			visitor.Visit((Tree)this);
			if (_subsequent != null)
			{
				_subsequent.Traverse(visitor);
			}
		}

		public void TraverseFromLeaves(IVisitor4 a_visitor)
		{
			if (_preceding != null)
			{
				_preceding.TraverseFromLeaves(a_visitor);
			}
			if (_subsequent != null)
			{
				_subsequent.TraverseFromLeaves(a_visitor);
			}
			a_visitor.Visit(this);
		}

		// Keep the debug method to debug the depth
		//	public final void debugLeafDepth(int currentDepth){
		//		currentDepth++;
		//		if(_preceding == null && _subsequent == null){
		//			System.out.println("" + currentDepth + " tree leaf depth.");
		//			return;
		//		}
		//	    if (_preceding != null){
		//	    	_preceding.debugLeafDepth(currentDepth);
		//	    }
		//	    if(_subsequent != null){
		//	    	_subsequent.debugLeafDepth(currentDepth);
		//	    }
		//	}
		protected virtual Tree ShallowCloneInternal(Tree tree)
		{
			tree._preceding = _preceding;
			tree._size = _size;
			tree._subsequent = _subsequent;
			return tree;
		}

		public virtual object ShallowClone()
		{
			throw new NotImplementedException();
		}

		public abstract object Key();

		public virtual object Root()
		{
			return this;
		}

		public virtual void Accept(IVisitor4 visitor)
		{
			Traverse(new _IVisitor4_513(visitor));
		}

		private sealed class _IVisitor4_513 : IVisitor4
		{
			public _IVisitor4_513(IVisitor4 visitor)
			{
				this.visitor = visitor;
			}

			public void Visit(object obj)
			{
				Tree tree = (Tree)obj;
				visitor.Visit(tree.Key());
			}

			private readonly IVisitor4 visitor;
		}

		public static int Depth(Tree tree)
		{
			if (tree == null)
			{
				return 0;
			}
			return Math.Max(Depth(((Tree)tree._preceding)), Depth(((Tree)tree._subsequent))) 
				+ 1;
		}
	}
}
