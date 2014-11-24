/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit.Fixtures;

namespace Db4objects.Db4o.Tests.Jre5.Collections.Typehandler
{
	public class ListTypeHandlerTestElementsSpec : ILabeled
	{
		public readonly object[] _elements;

		public readonly object _notContained;

		public readonly object _largeElement;

		public ListTypeHandlerTestElementsSpec(object[] elements, object notContained, object
			 largeElement)
		{
			_elements = elements;
			_notContained = notContained;
			_largeElement = largeElement;
		}

		public virtual string Label()
		{
			return _elements[0].GetType().Name;
		}
	}
}
