/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy;

namespace Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy
{
	public class STETH2 : STETH1TestCase
	{
		public string foo2;

		public STETH2()
		{
		}

		public STETH2(string str1, string str2) : base(str1)
		{
			foo2 = str2;
		}
	}
}
