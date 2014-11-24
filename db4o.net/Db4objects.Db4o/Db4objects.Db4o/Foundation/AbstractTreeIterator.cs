/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	/// <exclude></exclude>
	public abstract class AbstractTreeIterator : IEnumerator
	{
		private readonly Tree _tree;

		private Stack4 _stack;

		public AbstractTreeIterator(Tree tree)
		{
			_tree = tree;
		}

		public virtual object Current
		{
			get
			{
				if (_stack == null)
				{
					throw new InvalidOperationException();
				}
				Tree tree = Peek();
				if (tree == null)
				{
					return null;
				}
				return CurrentValue(tree);
			}
		}

		private Tree Peek()
		{
			return (Tree)_stack.Peek();
		}

		public virtual void Reset()
		{
			_stack = null;
		}

		public virtual bool MoveNext()
		{
			if (_stack == null)
			{
				InitStack();
				return _stack != null;
			}
			Tree current = Peek();
			if (current == null)
			{
				return false;
			}
			if (PushPreceding(((Tree)current._subsequent)))
			{
				return true;
			}
			while (true)
			{
				_stack.Pop();
				Tree parent = Peek();
				if (parent == null)
				{
					return false;
				}
				if (current == ((Tree)parent._preceding))
				{
					return true;
				}
				current = parent;
			}
		}

		private void InitStack()
		{
			if (_tree == null)
			{
				return;
			}
			_stack = new Stack4();
			PushPreceding(_tree);
		}

		private bool PushPreceding(Tree node)
		{
			if (node == null)
			{
				return false;
			}
			while (node != null)
			{
				_stack.Push(node);
				node = ((Tree)node._preceding);
			}
			return true;
		}

		protected abstract object CurrentValue(Tree tree);
	}
}
