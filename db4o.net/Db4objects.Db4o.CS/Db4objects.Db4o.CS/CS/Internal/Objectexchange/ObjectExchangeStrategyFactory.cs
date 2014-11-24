/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Internal.Objectexchange;

namespace Db4objects.Db4o.CS.Internal.Objectexchange
{
	public class ObjectExchangeStrategyFactory
	{
		public static IObjectExchangeStrategy ForConfig(ObjectExchangeConfiguration config
			)
		{
			if (config.prefetchDepth > 0)
			{
				return new EagerObjectExchangeStrategy(config);
			}
			return DeferredObjectExchangeStrategy.Instance;
		}
	}
}
