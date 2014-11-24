/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Config.Encoding;
using Db4objects.Db4o.Internal.Encoding;

namespace Db4objects.Db4o.Internal.Encoding
{
	/// <exclude></exclude>
	public class UnicodeStringEncoding : LatinStringEncoding
	{
		protected override LatinStringIO CreateStringIo(IStringEncoding encoding)
		{
			return new UnicodeStringIO();
		}
	}
}
