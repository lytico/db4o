/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Config.Encoding;
using Db4objects.Db4o.Internal.Encoding;

namespace Db4objects.Db4o.Config.Encoding
{
	/// <summary>All built in String encodings</summary>
	/// <seealso cref="Db4objects.Db4o.Config.IConfiguration.StringEncoding(IStringEncoding)
	/// 	">Db4objects.Db4o.Config.IConfiguration.StringEncoding(IStringEncoding)</seealso>
	public class StringEncodings
	{
		public static IStringEncoding Utf8()
		{
			return new UTF8StringEncoding();
		}

		public static IStringEncoding Unicode()
		{
			return new UnicodeStringEncoding();
		}

		public static IStringEncoding Latin()
		{
			return new LatinStringEncoding();
		}
	}
}
