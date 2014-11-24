/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	/// <summary>Useful as "out" or "by reference" function parameter.</summary>
	/// <remarks>Useful as "out" or "by reference" function parameter.</remarks>
	public class ByRef
	{
		public static ByRef NewInstance(object initialValue)
		{
			ByRef instance = new ByRef();
			instance.value = initialValue;
			return instance;
		}

		public static ByRef NewInstance()
		{
			return new ByRef();
		}

		public object value;
	}
}
