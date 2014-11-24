/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Ext;
using Db4objects.Db4o.IO;

namespace Db4objects.Db4o.IO
{
	/// <exclude></exclude>
	public class ReadOnlyBin : BinDecorator
	{
		public ReadOnlyBin(IBin storage) : base(storage)
		{
		}

		public override void Write(long position, byte[] bytes, int bytesToWrite)
		{
			throw new Db4oIOException();
		}
	}
}
