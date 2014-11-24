/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	/// <exclude></exclude>
	public class HashtableIterator : IEnumerator
	{
		private readonly HashtableIntEntry[] _table;

		private HashtableIntEntry _currentEntry;

		private int _currentIndex;

		public HashtableIterator(HashtableIntEntry[] table)
		{
			_table = table;
			Reset();
		}

		private void CheckInvalidTable()
		{
			if (_table == null || _table.Length == 0)
			{
				PositionBeyondLast();
			}
		}

		public virtual object Current
		{
			get
			{
				if (_currentEntry == null)
				{
					throw new InvalidOperationException();
				}
				return _currentEntry;
			}
		}

		public virtual bool MoveNext()
		{
			if (IsBeyondLast())
			{
				return false;
			}
			if (_currentEntry != null)
			{
				_currentEntry = _currentEntry._next;
			}
			while (_currentEntry == null)
			{
				if (_currentIndex >= _table.Length)
				{
					PositionBeyondLast();
					return false;
				}
				_currentEntry = _table[_currentIndex++];
			}
			return true;
		}

		public virtual void Reset()
		{
			_currentEntry = null;
			_currentIndex = 0;
			CheckInvalidTable();
		}

		private bool IsBeyondLast()
		{
			return _currentIndex == -1;
		}

		private void PositionBeyondLast()
		{
			_currentIndex = -1;
		}
	}
}
