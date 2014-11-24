/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Drs.Tests.Data;

namespace Db4objects.Drs.Tests.Data
{
	public class R1 : R0
	{
		internal R0 circle1;

		internal R2 r2;

		public virtual R0 GetCircle1()
		{
			return circle1;
		}

		public virtual void SetCircle1(R0 circle1)
		{
			this.circle1 = circle1;
		}

		public virtual R2 GetR2()
		{
			return r2;
		}

		public virtual void SetR2(R2 r2)
		{
			this.r2 = r2;
		}
	}
}
