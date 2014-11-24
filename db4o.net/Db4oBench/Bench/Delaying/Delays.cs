/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Bench.Delaying
{
	public class Delays
	{
		public const int Read = 0;

		public const int Write = 1;

		public const int Sync = 3;

		public const int Count = 4;

		public static readonly string units = "nanoseconds";

		public long[] values;

		public Delays(long read, long write, long sync)
		{
			values = new long[] { read, write, sync };
		}

		public override string ToString()
		{
			return "[delays in " + units + "] read: " + values[Read] + " | write: " + values[
				Write] + " | sync: " + values[Sync];
		}
	}
}
