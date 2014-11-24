/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;

namespace Db4objects.Db4o.Tests.Util
{
	public class PermutingTestConfig
	{
		private object[][] _values;

		private int[] _indices;

		private bool _started;

		public PermutingTestConfig(object[][] values)
		{
			_values = values;
			_indices = new int[_values.Length];
			_started = false;
		}

		public virtual bool MoveNext()
		{
			if (!_started)
			{
				_started = true;
				return true;
			}
			for (int groupIdx = _indices.Length - 1; groupIdx >= 0; groupIdx--)
			{
				if (_indices[groupIdx] < _values[groupIdx].Length - 1)
				{
					_indices[groupIdx]++;
					for (int resetGroupIdx = groupIdx + 1; resetGroupIdx < _indices.Length; resetGroupIdx
						++)
					{
						_indices[resetGroupIdx] = 0;
					}
					return true;
				}
			}
			return false;
		}

		/// <exception cref="System.InvalidOperationException"></exception>
		/// <exception cref="System.ArgumentException"></exception>
		public virtual object Current(int groupIdx)
		{
			if (!_started)
			{
				throw new InvalidOperationException();
			}
			if (groupIdx < 0 || groupIdx >= _indices.Length)
			{
				throw new ArgumentException();
			}
			return _values[groupIdx][_indices[groupIdx]];
		}
	}
}
