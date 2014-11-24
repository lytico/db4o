/* Copyright (C) 2004-2007   Versant Inc.   http://www.db4o.com */
namespace Db4objects.Db4o.Tests.CLI2.TA
{
	public struct Pair<T0, T1> where T1: struct
	{
		public T0 First;
		public T1 Second;

		public Pair(T0 first, T1 second)
		{
			First = first;
			Second = second;
		}
	}
}