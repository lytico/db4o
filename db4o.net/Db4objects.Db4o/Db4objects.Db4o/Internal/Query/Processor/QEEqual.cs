/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal.Query.Processor;

namespace Db4objects.Db4o.Internal.Query.Processor
{
	/// <exclude></exclude>
	public class QEEqual : QEAbstract
	{
		public override void IndexBitMap(bool[] bits)
		{
			bits[QE.Equal] = true;
		}
	}
}
