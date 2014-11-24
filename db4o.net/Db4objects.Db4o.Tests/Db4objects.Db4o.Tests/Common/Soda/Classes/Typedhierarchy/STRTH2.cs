/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy;

namespace Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy
{
	public class STRTH2
	{
		public STRTH1TestCase parent;

		public STRTH3 h3;

		public string foo2;

		public STRTH2()
		{
		}

		public STRTH2(STRTH3 a3)
		{
			h3 = a3;
			a3.parent = this;
		}

		public STRTH2(string str)
		{
			foo2 = str;
		}

		public STRTH2(STRTH3 a3, string str)
		{
			h3 = a3;
			a3.parent = this;
			foo2 = str;
		}
	}
}
