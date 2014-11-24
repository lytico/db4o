/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Drs.Tests.Data
{
	public class ArrayHolder
	{
		public string _name;

		public Db4objects.Drs.Tests.Data.ArrayHolder[] _array;

		public Db4objects.Drs.Tests.Data.ArrayHolder[][] _arrayN;

		public ArrayHolder()
		{
		}

		public ArrayHolder(string name)
		{
			_name = name;
		}

		public override string ToString()
		{
			return _name + ", hashcode = " + GetHashCode();
		}

		public virtual string GetName()
		{
			return _name;
		}

		public virtual Db4objects.Drs.Tests.Data.ArrayHolder[] Array()
		{
			return _array;
		}

		public virtual Db4objects.Drs.Tests.Data.ArrayHolder[][] ArrayN()
		{
			return _arrayN;
		}
	}
}
