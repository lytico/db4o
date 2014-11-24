/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Drs.Tests.Data;

namespace Db4objects.Drs.Tests.Data
{
	public class R0
	{
		internal string name;

		internal R0 r0;

		internal R1 r1;

		public virtual string GetName()
		{
			return name;
		}

		public virtual void SetName(string name)
		{
			this.name = name;
		}

		public virtual R0 GetR0()
		{
			return r0;
		}

		public virtual void SetR0(R0 r0)
		{
			this.r0 = r0;
		}

		public virtual R1 GetR1()
		{
			return r1;
		}

		public virtual void SetR1(R1 r1)
		{
			this.r1 = r1;
		}
	}
}
