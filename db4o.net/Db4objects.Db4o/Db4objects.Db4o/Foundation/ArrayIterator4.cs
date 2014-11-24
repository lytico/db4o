/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	public class ArrayIterator4 : IndexedIterator
	{
		private readonly object[] _elements;

		public ArrayIterator4(object[] elements) : base(elements.Length)
		{
			_elements = elements;
		}

		protected override object Get(int index)
		{
			return _elements[index];
		}
	}
}
