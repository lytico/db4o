/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Drs.Tests.Data;

namespace Db4objects.Drs.Tests.Data
{
	public class R2 : R1
	{
		internal R0 circle2;

		internal R3 r3;

		public virtual R0 GetCircle2()
		{
			return circle2;
		}

		public virtual void SetCircle2(R0 circle2)
		{
			this.circle2 = circle2;
		}

		public virtual R3 GetR3()
		{
			return r3;
		}

		public virtual void SetR3(R3 r3)
		{
			this.r3 = r3;
		}
	}
}
