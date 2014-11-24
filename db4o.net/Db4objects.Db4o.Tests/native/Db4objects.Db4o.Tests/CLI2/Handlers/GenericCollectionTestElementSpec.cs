/* Copyright (C) 2004 - 2008  Versant Inc.  http://www.db4o.com */
using Db4oUnit.Fixtures;

namespace Db4objects.Db4o.Tests.CLI2.Handlers
{
	public class GenericCollectionTestElementSpec<T> : ILabeled
	{
		public T[] _elements;

		public T _notContained;

		public T _largeElement;

		public GenericCollectionTestElementSpec(T[] elements, T notContained, T largeElement)
		{
			_elements = elements;
			_notContained = notContained;
			_largeElement = largeElement;
		}

		public virtual string Label()
		{
			return typeof(T).Name;
		}
	}
}
