/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Typehandlers.Internal
{
	/// <exclude></exclude>
	public class KeyValueHandlerPair
	{
		public readonly ITypeHandler4 _keyHandler;

		public readonly ITypeHandler4 _valueHandler;

		public KeyValueHandlerPair(ITypeHandler4 keyHandler, ITypeHandler4 valueHandler)
		{
			_keyHandler = keyHandler;
			_valueHandler = valueHandler;
		}
	}
}
