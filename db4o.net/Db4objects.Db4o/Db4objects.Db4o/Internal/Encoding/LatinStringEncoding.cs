/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Config.Encoding;
using Db4objects.Db4o.Internal.Encoding;

namespace Db4objects.Db4o.Internal.Encoding
{
	/// <exclude></exclude>
	public class LatinStringEncoding : BuiltInStringEncoding
	{
		public override string Decode(byte[] bytes, int start, int length)
		{
			throw new NotImplementedException();
		}

		// special StringIO, should never be called
		public override byte[] Encode(string str)
		{
			throw new NotImplementedException();
		}

		// special StringIO, should never be called
		protected override LatinStringIO CreateStringIo(IStringEncoding encoding)
		{
			return new LatinStringIO();
		}
	}
}
