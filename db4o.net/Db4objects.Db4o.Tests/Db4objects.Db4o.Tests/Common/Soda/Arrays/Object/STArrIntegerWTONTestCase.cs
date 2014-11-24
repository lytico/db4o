/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda.Util;
using Sharpen;

namespace Db4objects.Db4o.Tests.Common.Soda.Arrays.Object
{
	public class STArrIntegerWTONTestCase : SodaBaseTestCase
	{
		public object intArr;

		public STArrIntegerWTONTestCase()
		{
		}

		public STArrIntegerWTONTestCase(int[][][] arr)
		{
			intArr = arr;
		}

		public override object[] CreateData()
		{
			Db4objects.Db4o.Tests.Common.Soda.Arrays.Object.STArrIntegerWTONTestCase[] arr = 
				new Db4objects.Db4o.Tests.Common.Soda.Arrays.Object.STArrIntegerWTONTestCase[5];
			arr[0] = new Db4objects.Db4o.Tests.Common.Soda.Arrays.Object.STArrIntegerWTONTestCase
				();
			int[][][] content = new int[][][] {  };
			arr[1] = new Db4objects.Db4o.Tests.Common.Soda.Arrays.Object.STArrIntegerWTONTestCase
				(content);
			content = new int[][][] { new int[][] { new int[3], new int[3] } };
			content[0][0][1] = 0;
			content[0][1][0] = 0;
			arr[2] = new Db4objects.Db4o.Tests.Common.Soda.Arrays.Object.STArrIntegerWTONTestCase
				(content);
			content = new int[][][] { new int[][] { new int[3], new int[3] } };
			content[0][0][0] = 1;
			content[0][1][0] = 17;
			content[0][1][1] = int.MaxValue - 1;
			arr[3] = new Db4objects.Db4o.Tests.Common.Soda.Arrays.Object.STArrIntegerWTONTestCase
				(content);
			content = new int[][][] { new int[][] { new int[2], new int[2] } };
			content[0][0][0] = 3;
			content[0][0][1] = 17;
			content[0][1][0] = 25;
			content[0][1][1] = int.MaxValue - 2;
			arr[4] = new Db4objects.Db4o.Tests.Common.Soda.Arrays.Object.STArrIntegerWTONTestCase
				(content);
			object[] ret = new object[arr.Length];
			System.Array.Copy(arr, 0, ret, 0, arr.Length);
			return ret;
		}

		public virtual void TestDefaultContainsOne()
		{
			IQuery q = NewQuery();
			int[][][] content = new int[][][] { new int[][] { new int[1] } };
			content[0][0][0] = 17;
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Arrays.Object.STArrIntegerWTONTestCase
				(content));
			Expect(q, new int[] { 3, 4 });
		}

		public virtual void TestDefaultContainsTwo()
		{
			IQuery q = NewQuery();
			int[][][] content = new int[][][] { new int[][] { new int[1] }, new int[][] { new 
				int[1] } };
			content[0][0][0] = 17;
			content[1][0][0] = 25;
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Arrays.Object.STArrIntegerWTONTestCase
				(content));
			Expect(q, new int[] { 4 });
		}

		public virtual void TestDescendOne()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Arrays.Object.STArrIntegerWTONTestCase
				));
			q.Descend("intArr").Constrain(17);
			Expect(q, new int[] { 3, 4 });
		}

		public virtual void TestDescendTwo()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Arrays.Object.STArrIntegerWTONTestCase
				));
			IQuery qElements = q.Descend("intArr");
			qElements.Constrain(17);
			qElements.Constrain(25);
			Expect(q, new int[] { 4 });
		}

		public virtual void TestDescendSmaller()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Arrays.Object.STArrIntegerWTONTestCase
				));
			IQuery qElements = q.Descend("intArr");
			qElements.Constrain(3).Smaller();
			Expect(q, new int[] { 2, 3 });
		}

		public virtual void TestDescendNotSmaller()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Arrays.Object.STArrIntegerWTONTestCase
				));
			IQuery qElements = q.Descend("intArr");
			qElements.Constrain(3).Smaller();
			Expect(q, new int[] { 2, 3 });
		}
	}
}
