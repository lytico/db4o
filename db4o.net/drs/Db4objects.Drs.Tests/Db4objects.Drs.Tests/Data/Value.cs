/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Drs.Tests.Data
{
	public struct Value
	{
		public int value;

		public Value(int value)
		{
			this.value = value;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Db4objects.Drs.Tests.Data.Value))
			{
				return false;
			}
			Db4objects.Drs.Tests.Data.Value other = (Db4objects.Drs.Tests.Data.Value)obj;
			return other.value == value;
		}
	}
}
