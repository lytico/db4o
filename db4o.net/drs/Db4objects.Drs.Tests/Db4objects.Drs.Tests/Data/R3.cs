/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Drs.Tests.Data;

namespace Db4objects.Drs.Tests.Data
{
	public class R3 : R2
	{
		internal R0 circle3;

		internal R4 r4;

		public virtual R0 GetCircle3()
		{
			return circle3;
		}

		public virtual void SetCircle3(R0 circle3)
		{
			this.circle3 = circle3;
		}

		public virtual R4 GetR4()
		{
			return r4;
		}

		public virtual void SetR4(R4 r4)
		{
			this.r4 = r4;
		}
	}
}
