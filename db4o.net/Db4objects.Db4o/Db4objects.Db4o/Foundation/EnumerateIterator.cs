/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	public class EnumerateIterator : MappingIterator
	{
		public sealed class Tuple
		{
			public readonly int index;

			public readonly object value;

			public Tuple(int index_, object value_)
			{
				index = index_;
				value = value_;
			}
		}

		private int _index;

		public EnumerateIterator(IEnumerator iterator) : base(iterator)
		{
			_index = 0;
		}

		public override bool MoveNext()
		{
			if (base.MoveNext())
			{
				++_index;
				return true;
			}
			return false;
		}

		protected override object Map(object current)
		{
			return new EnumerateIterator.Tuple(_index, current);
		}
	}
}
