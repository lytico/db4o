/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy;

namespace Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy
{
	public class STETH4 : STETH2
	{
		public string foo4;

		public STETH4()
		{
		}

		public STETH4(string str1, string str2, string str3) : base(str1, str2)
		{
			foo4 = str3;
		}
	}
}
