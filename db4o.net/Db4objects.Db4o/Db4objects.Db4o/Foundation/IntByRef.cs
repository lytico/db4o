/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Foundation
{
	/// <summary>Useful as "out" or "by ref" function parameter.</summary>
	/// <remarks>Useful as "out" or "by ref" function parameter.</remarks>
	public sealed class IntByRef
	{
		public int value;

		public IntByRef(int initialValue)
		{
			value = initialValue;
		}

		public IntByRef()
		{
		}
	}
}
