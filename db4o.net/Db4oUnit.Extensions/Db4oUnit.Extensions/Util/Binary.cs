/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;

namespace Db4oUnit.Extensions.Util
{
	/// <exclude></exclude>
	public class Binary
	{
		public static long LongForBits(long bits)
		{
			return (long)((Math.Pow(2, bits)) - 1);
		}

		public static int NumberOfBits(long l)
		{
			if (l < 0)
			{
				throw new ArgumentException();
			}
			long bit = 1;
			int counter = 0;
			for (int i = 0; i < 64; i++)
			{
				if ((l & bit) == 0)
				{
					counter++;
				}
				else
				{
					counter = 0;
				}
				bit = bit << 1;
			}
			return 64 - counter;
		}
	}
}
