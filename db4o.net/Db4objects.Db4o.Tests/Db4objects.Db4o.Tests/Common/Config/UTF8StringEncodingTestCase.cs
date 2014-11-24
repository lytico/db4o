/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Config.Encoding;
using Db4objects.Db4o.Internal.Encoding;
using Db4objects.Db4o.Tests.Common.Config;

namespace Db4objects.Db4o.Tests.Common.Config
{
	public class UTF8StringEncodingTestCase : StringEncodingTestCaseBase
	{
		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.StringEncoding(StringEncodings.Utf8());
		}

		protected override Type StringIoClass()
		{
			return typeof(DelegatingStringIO);
		}

		public static void Main(string[] arguments)
		{
			new UTF8StringEncodingTestCase().RunEmbedded();
		}

		public virtual void TestEncodeDecode()
		{
			string original = "ABCZabcz?$@#.,;:";
			UTF8StringEncoding encoder = new UTF8StringEncoding();
			byte[] bytes = encoder.Encode(original);
			string decoded = encoder.Decode(bytes, 0, bytes.Length);
			Assert.AreEqual(original, decoded);
		}
	}
}
