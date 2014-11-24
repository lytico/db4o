/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda.Util;

namespace Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed
{
	public class STArrIntegerTTestCase : SodaBaseTestCase
	{
		public int[] intArr;

		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrIntegerTTestCase().RunSolo
				();
		}

		public STArrIntegerTTestCase()
		{
		}

		public STArrIntegerTTestCase(int[] arr)
		{
			intArr = arr;
		}

		public override object[] CreateData()
		{
			return new object[] { new Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrIntegerTTestCase
				(), new Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrIntegerTTestCase(new 
				int[0]), new Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrIntegerTTestCase
				(new int[] { 0, 0 }), new Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrIntegerTTestCase
				(new int[] { 1, 17, int.MaxValue - 1 }), new Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrIntegerTTestCase
				(new int[] { 3, 17, 25, int.MaxValue - 2 }) };
		}

		public virtual void _testDefaultContainsOne()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrIntegerTTestCase
				(new int[] { 17 }));
			Expect(q, new int[] { 3, 4 });
		}

		public virtual void _testDefaultContainsTwo()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrIntegerTTestCase
				(new int[] { 17, 25 }));
			Expect(q, new int[] { 4 });
		}

		public virtual void TestDescendOne()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrIntegerTTestCase
				));
			q.Descend("intArr").Constrain(17);
			Expect(q, new int[] { 3, 4 });
		}

		public virtual void TestDescendTwo()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrIntegerTTestCase
				));
			IQuery qElements = q.Descend("intArr");
			qElements.Constrain(17);
			qElements.Constrain(25);
			Expect(q, new int[] { 4 });
		}

		public virtual void TestDescendSmaller()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrIntegerTTestCase
				));
			IQuery qElements = q.Descend("intArr");
			qElements.Constrain(3).Smaller();
			Expect(q, new int[] { 2, 3 });
		}

		public virtual void TestDescendNotSmaller()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrIntegerTTestCase
				));
			IQuery qElements = q.Descend("intArr");
			qElements.Constrain(3).Smaller();
			Expect(q, new int[] { 2, 3 });
		}
	}
}
