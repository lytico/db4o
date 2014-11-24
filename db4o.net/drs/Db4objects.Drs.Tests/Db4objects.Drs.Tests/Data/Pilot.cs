/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Drs.Tests.Data
{
	public class Pilot
	{
		private string _name;

		private int _age;

		public Pilot()
		{
		}

		public Pilot(string name, int age)
		{
			this._name = name;
			this._age = age;
		}

		public virtual void SetName(string name)
		{
			this._name = name;
		}

		public virtual string Name()
		{
			return _name;
		}

		public override string ToString()
		{
			return "Pilot name:" + Name();
		}
	}
}
