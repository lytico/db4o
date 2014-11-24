/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class SimpleObject
	{
		public string _s;

		public int _i;

		public SimpleObject(string s, int i)
		{
			_s = s;
			_i = i;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Db4objects.Db4o.Tests.Common.Assorted.SimpleObject))
			{
				return false;
			}
			Db4objects.Db4o.Tests.Common.Assorted.SimpleObject another = (Db4objects.Db4o.Tests.Common.Assorted.SimpleObject
				)obj;
			return _s.Equals(another._s) && (_i == another._i);
		}

		public virtual int GetI()
		{
			return _i;
		}

		public virtual void SetI(int i)
		{
			_i = i;
		}

		public virtual string GetS()
		{
			return _s;
		}

		public virtual void SetS(string s)
		{
			_s = s;
		}

		public override string ToString()
		{
			return _s + ":" + _i;
		}
	}
}
