/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy;

namespace Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy
{
	public class STSDFT2 : STSDFT1TestCase
	{
		public string foo;

		public STSDFT2()
		{
		}

		public STSDFT2(string str)
		{
			foo = str;
		}
	}
}
