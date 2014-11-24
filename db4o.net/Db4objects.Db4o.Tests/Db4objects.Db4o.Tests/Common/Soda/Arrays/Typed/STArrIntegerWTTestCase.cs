/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda.Util;

namespace Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed
{
	public class STArrIntegerWTTestCase : SodaBaseTestCase
	{
		public int[] intArr;

		public STArrIntegerWTTestCase()
		{
		}

		public STArrIntegerWTTestCase(int[] arr)
		{
			intArr = arr;
		}

		public override object[] CreateData()
		{
			return new object[] { new Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrIntegerWTTestCase
				(), new Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrIntegerWTTestCase(new 
				int[0]), new Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrIntegerWTTestCase
				(new int[] { 0, 0 }), new Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrIntegerWTTestCase
				(new int[] { 1, 17, int.MaxValue - 1 }), new Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrIntegerWTTestCase
				(new int[] { 3, 17, 25, int.MaxValue - 2 }) };
		}

		public virtual void TestDefaultContainsOne()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrIntegerWTTestCase
				(new int[] { 17 }));
			Expect(q, new int[] { 3, 4 });
		}

		public virtual void TestDefaultContainsTwo()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrIntegerWTTestCase
				(new int[] { 17, 25 }));
			Expect(q, new int[] { 4 });
		}
	}
}
