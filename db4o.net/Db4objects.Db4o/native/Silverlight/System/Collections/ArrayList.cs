/* Copyright (C) 2009   Versant Inc.   http://www.db4o.com */
#if SILVERLIGHT

using System.Collections.Generic;

namespace System.Collections
{
	public class ArrayList : List<object>
	{
		public ArrayList(int capacity) : base(capacity)
		{
		}

		public ArrayList()
		{
		}
	}
}

#endif