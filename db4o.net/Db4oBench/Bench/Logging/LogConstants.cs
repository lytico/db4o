/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Sharpen.Util;

namespace Db4objects.Db4o.Bench.Logging
{
	public class LogConstants
	{
		public static readonly string ReadEntry = "READ ";

		public static readonly string WriteEntry = "WRITE ";

		public static readonly string SyncEntry = "SYNC ";

		public static readonly string[] AllConstants = new string[] { ReadEntry, WriteEntry
			, SyncEntry };

		public static readonly string Separator = ",";

		public static Sharpen.Util.ISet AllEntries()
		{
			HashSet entries = new HashSet();
			Sharpen.Collections.AddAll(entries, Arrays.AsList(AllConstants));
			return entries;
		}
	}
}
