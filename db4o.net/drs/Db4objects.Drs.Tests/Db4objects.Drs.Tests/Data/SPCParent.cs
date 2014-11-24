/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Drs.Tests.Data;

namespace Db4objects.Drs.Tests.Data
{
	public class SPCParent
	{
		private SPCChild child;

		private string name;

		public SPCParent()
		{
		}

		public SPCParent(string name)
		{
			this.name = name;
		}

		public SPCParent(SPCChild child, string name)
		{
			this.child = child;
			this.name = name;
		}

		public virtual SPCChild GetChild()
		{
			return child;
		}

		public virtual void SetChild(SPCChild child)
		{
			this.child = child;
		}

		public virtual string GetName()
		{
			return name;
		}

		public virtual void SetName(string name)
		{
			this.name = name;
		}

		public override string ToString()
		{
			return "SPCParent{" + "child=" + child + ", name='" + name + '\'' + '}';
		}
	}
}
