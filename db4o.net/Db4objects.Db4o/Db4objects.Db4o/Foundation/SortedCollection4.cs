/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	/// <exclude></exclude>
	public class SortedCollection4
	{
		private readonly IComparison4 _comparison;

		private Tree _tree;

		public SortedCollection4(IComparison4 comparison)
		{
			if (null == comparison)
			{
				throw new ArgumentNullException();
			}
			_comparison = comparison;
			_tree = null;
		}

		public virtual object SingleElement()
		{
			if (1 != Size())
			{
				throw new InvalidOperationException();
			}
			return _tree.Key();
		}

		public virtual void AddAll(IEnumerator iterator)
		{
			while (iterator.MoveNext())
			{
				Add(iterator.Current);
			}
		}

		public virtual void Add(object element)
		{
			_tree = Tree.Add(_tree, new TreeObject(element, _comparison));
		}

		public virtual void Remove(object element)
		{
			_tree = Tree.RemoveLike(_tree, new TreeObject(element, _comparison));
		}

		public virtual object[] ToArray(object[] array)
		{
			Tree.Traverse(_tree, new _IVisitor4_43(array));
			return array;
		}

		private sealed class _IVisitor4_43 : IVisitor4
		{
			public _IVisitor4_43(object[] array)
			{
				this.array = array;
				this.i = 0;
			}

			internal int i;

			public void Visit(object obj)
			{
				array[this.i++] = ((TreeObject)obj).Key();
			}

			private readonly object[] array;
		}

		public virtual int Size()
		{
			return Tree.Size(_tree);
		}
	}
}
