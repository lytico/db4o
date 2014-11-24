/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Drs.Tests.Data;

namespace Db4objects.Drs.Tests.Data
{
	public class R4 : R3
	{
		internal R0 circle4;

		public virtual R0 GetCircle4()
		{
			return circle4;
		}

		public virtual void SetCircle4(R0 circle4)
		{
			this.circle4 = circle4;
		}
	}
}
