/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Drs.Inside;
using Db4objects.Drs.Tests.Data;

namespace Db4objects.Drs.Tests
{
	internal class R0Linker
	{
		internal R0 r0;

		internal R1 r1;

		internal R2 r2;

		internal R3 r3;

		internal R4 r4;

		internal R0Linker()
		{
			r0 = new R0();
			r1 = new R1();
			r2 = new R2();
			r3 = new R3();
			r4 = new R4();
		}

		internal virtual void SetNames(string name)
		{
			r0.SetName("0" + name);
			r1.SetName("1" + name);
			r2.SetName("2" + name);
			r3.SetName("3" + name);
			r4.SetName("4" + name);
		}

		internal virtual void LinkCircles()
		{
			LinkList();
			r1.SetCircle1(r0);
			r2.SetCircle2(r0);
			r3.SetCircle3(r0);
			r4.SetCircle4(r0);
		}

		internal virtual void LinkList()
		{
			r0.SetR1(r1);
			r1.SetR2(r2);
			r2.SetR3(r3);
			r3.SetR4(r4);
		}

		internal virtual void LinkThis()
		{
			r0.SetR0(r0);
			r1.SetR1(r1);
			r2.SetR2(r2);
			r3.SetR3(r3);
			r4.SetR4(r4);
		}

		internal virtual void LinkBack()
		{
			r1.SetR0(r0);
			r2.SetR1(r1);
			r3.SetR2(r2);
			r4.SetR3(r3);
		}

		public virtual void Store(ITestableReplicationProviderInside provider)
		{
			provider.StoreNew(r4);
			provider.StoreNew(r3);
			provider.StoreNew(r2);
			provider.StoreNew(r1);
			provider.StoreNew(r0);
		}
	}
}
