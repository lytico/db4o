/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Drs.Inside
{
	public class DrsDebug
	{
		private static bool production = false;

		public static readonly bool runEventListenerEmbedded = !production;

		public const bool verbose = false;

		public const bool fastReplicationFeaturesMain = true;

		public static long Timeout(long designed)
		{
			return designed;
		}
	}
}
