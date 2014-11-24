/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.CS.Internal.Objectexchange
{
	public class ObjectExchangeConfiguration
	{
		public int prefetchDepth;

		public int prefetchCount;

		public ObjectExchangeConfiguration(int prefetchDepth, int prefetchCount)
		{
			this.prefetchDepth = prefetchDepth;
			this.prefetchCount = prefetchCount;
		}
	}
}
