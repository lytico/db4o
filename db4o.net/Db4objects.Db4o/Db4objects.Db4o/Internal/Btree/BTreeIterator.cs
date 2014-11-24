/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;

namespace Db4objects.Db4o.Internal.Btree
{
	/// <exclude></exclude>
	public class BTreeIterator : IEnumerator
	{
		private readonly Transaction _transaction;

		private readonly BTree _bTree;

		private BTreePointer _currentPointer;

		private bool _beyondEnd;

		public BTreeIterator(Transaction trans, BTree bTree)
		{
			_transaction = trans;
			_bTree = bTree;
		}

		public virtual object Current
		{
			get
			{
				if (_currentPointer == null)
				{
					throw new InvalidOperationException();
				}
				return _currentPointer.Key();
			}
		}

		public virtual bool MoveNext()
		{
			if (_beyondEnd)
			{
				return false;
			}
			if (BeforeFirst())
			{
				_currentPointer = _bTree.FirstPointer(_transaction);
			}
			else
			{
				_currentPointer = _currentPointer.Next();
			}
			_beyondEnd = (_currentPointer == null);
			return !_beyondEnd;
		}

		private bool BeforeFirst()
		{
			return _currentPointer == null;
		}

		public virtual void Reset()
		{
			throw new NotSupportedException();
		}
	}
}
