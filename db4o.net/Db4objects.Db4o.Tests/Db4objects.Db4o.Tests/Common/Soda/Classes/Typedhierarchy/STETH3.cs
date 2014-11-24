/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy;

namespace Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy
{
	public class STETH3 : STETH2
	{
		public string foo3;

		public STETH3()
		{
		}

		public STETH3(string str1, string str2, string str3) : base(str1, str2)
		{
			foo3 = str3;
		}
	}
}
