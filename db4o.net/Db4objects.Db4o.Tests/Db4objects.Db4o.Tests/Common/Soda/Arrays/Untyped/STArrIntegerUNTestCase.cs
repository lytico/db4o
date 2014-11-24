/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda.Util;
using Sharpen;

namespace Db4objects.Db4o.Tests.Common.Soda.Arrays.Untyped
{
	public class STArrIntegerUNTestCase : SodaBaseTestCase
	{
		public object[][][] intArr;

		public STArrIntegerUNTestCase()
		{
		}

		public STArrIntegerUNTestCase(object[][][] arr)
		{
			intArr = arr;
		}

		public override object[] CreateData()
		{
			Db4objects.Db4o.Tests.Common.Soda.Arrays.Untyped.STArrIntegerUNTestCase[] arr = new 
				Db4objects.Db4o.Tests.Common.Soda.Arrays.Untyped.STArrIntegerUNTestCase[5];
			arr[0] = new Db4objects.Db4o.Tests.Common.Soda.Arrays.Untyped.STArrIntegerUNTestCase
				();
			object[][][] content = new object[][][] {  };
			arr[1] = new Db4objects.Db4o.Tests.Common.Soda.Arrays.Untyped.STArrIntegerUNTestCase
				(content);
			content = new object[][][] { new object[][] { new object[3], new object[3] } };
			content[0][0][1] = 0;
			content[0][1][0] = 0;
			arr[2] = new Db4objects.Db4o.Tests.Common.Soda.Arrays.Untyped.STArrIntegerUNTestCase
				(content);
			content = new object[][][] { new object[][] { new object[3], new object[3] } };
			content[0][0][0] = 1;
			content[0][1][0] = 17;
			content[0][1][1] = int.MaxValue - 1;
			arr[3] = new Db4objects.Db4o.Tests.Common.Soda.Arrays.Untyped.STArrIntegerUNTestCase
				(content);
			content = new object[][][] { new object[][] { new object[2], new object[2] } };
			content[0][0][0] = 3;
			content[0][0][1] = 17;
			content[0][1][0] = 25;
			content[0][1][1] = int.MaxValue - 2;
			arr[4] = new Db4objects.Db4o.Tests.Common.Soda.Arrays.Untyped.STArrIntegerUNTestCase
				(content);
			object[] ret = new object[arr.Length];
			System.Array.Copy(arr, 0, ret, 0, arr.Length);
			return ret;
		}

		public virtual void TestDefaultContainsOne()
		{
			IQuery q = NewQuery();
			object[][][] content = new object[][][] { new object[][] { new object[1] } };
			content[0][0][0] = 17;
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Arrays.Untyped.STArrIntegerUNTestCase
				(content));
			Expect(q, new int[] { 3, 4 });
		}

		public virtual void TestDefaultContainsTwo()
		{
			IQuery q = NewQuery();
			object[][][] content = new object[][][] { new object[][] { new object[1] }, new object
				[][] { new object[1] } };
			content[0][0][0] = 17;
			content[1][0][0] = 25;
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Arrays.Untyped.STArrIntegerUNTestCase
				(content));
			Expect(q, new int[] { 4 });
		}

		public virtual void TestDescendOne()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Arrays.Untyped.STArrIntegerUNTestCase
				));
			q.Descend("intArr").Constrain(17);
			Expect(q, new int[] { 3, 4 });
		}

		public virtual void TestDescendTwo()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Arrays.Untyped.STArrIntegerUNTestCase
				));
			IQuery qElements = q.Descend("intArr");
			qElements.Constrain(17);
			qElements.Constrain(25);
			Expect(q, new int[] { 4 });
		}

		public virtual void TestDescendSmaller()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Arrays.Untyped.STArrIntegerUNTestCase
				));
			IQuery qElements = q.Descend("intArr");
			qElements.Constrain(3).Smaller();
			Expect(q, new int[] { 2, 3 });
		}

		public virtual void TestDescendNotSmaller()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Arrays.Untyped.STArrIntegerUNTestCase
				));
			IQuery qElements = q.Descend("intArr");
			qElements.Constrain(3).Smaller();
			Expect(q, new int[] { 2, 3 });
		}
	}
}
