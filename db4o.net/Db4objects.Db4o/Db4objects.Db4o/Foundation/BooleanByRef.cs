/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Foundation
{
	/// <summary>Useful as "out" or "by ref" function parameter.</summary>
	/// <remarks>Useful as "out" or "by ref" function parameter.</remarks>
	public class BooleanByRef
	{
		public bool value;

		public BooleanByRef() : this(false)
		{
		}

		public BooleanByRef(bool value_)
		{
			value = value_;
		}
	}
}
