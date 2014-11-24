/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

using System.Collections;

namespace Sharpen.Util 
{
	public class Random 
	{
		readonly System.Random _random = new System.Random();

		public Random() 
		{
		}
		
		public long NextLong() 
		{
			return _random.Next();
		}
		
		public int NextInt()
		{
			return _random.Next(int.MinValue, int.MaxValue);
		}

		public object NextInt(int ceiling)
		{
			return _random.Next(ceiling);
		}
	}
}