/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Tests.Common.Soda.Classes.Untypedhierarchy;

namespace Db4objects.Db4o.Tests.Common.Soda.Classes.Untypedhierarchy
{
	public class STRUH2
	{
		public object parent;

		public object h3;

		public string foo2;

		public STRUH2()
		{
		}

		public STRUH2(STRUH3 a3)
		{
			h3 = a3;
			a3.parent = this;
		}

		public STRUH2(string str)
		{
			foo2 = str;
		}

		public STRUH2(STRUH3 a3, string str)
		{
			h3 = a3;
			a3.parent = this;
			foo2 = str;
		}
	}
}
