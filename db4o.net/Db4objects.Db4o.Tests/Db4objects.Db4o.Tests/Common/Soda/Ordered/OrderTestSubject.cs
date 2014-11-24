/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Tests.Common.Soda.Ordered
{
	public class OrderTestSubject
	{
		public string _name;

		public int _seniority;

		public int _age;

		public OrderTestSubject(string name, int age, int seniority)
		{
			this._name = name;
			this._age = age;
			this._seniority = seniority;
		}

		public override string ToString()
		{
			return _name + " " + _age + " " + _seniority;
		}

		public override bool Equals(object o)
		{
			if (o == null)
			{
				return false;
			}
			if (o.GetType() != GetType())
			{
				return false;
			}
			Db4objects.Db4o.Tests.Common.Soda.Ordered.OrderTestSubject ots = (Db4objects.Db4o.Tests.Common.Soda.Ordered.OrderTestSubject
				)o;
			bool ret = (_age == ots._age) && (_name.Equals(ots._name)) && (_seniority == ots.
				_seniority);
			return ret;
		}
	}
}
