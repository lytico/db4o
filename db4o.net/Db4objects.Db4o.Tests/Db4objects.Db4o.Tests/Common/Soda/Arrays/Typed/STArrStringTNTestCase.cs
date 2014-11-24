/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda.Util;
using Sharpen;

namespace Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed
{
	public class STArrStringTNTestCase : SodaBaseTestCase
	{
		public string[][][] strArr;

		public STArrStringTNTestCase()
		{
		}

		public STArrStringTNTestCase(string[][][] arr)
		{
			strArr = arr;
		}

		public override object[] CreateData()
		{
			Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrStringTNTestCase[] arr = new 
				Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrStringTNTestCase[5];
			arr[0] = new Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrStringTNTestCase
				();
			string[][][] content = new string[][][] { new string[][] { new string[2] } };
			arr[1] = new Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrStringTNTestCase
				(content);
			content = new string[][][] { new string[][] { new string[3], new string[3] } };
			arr[2] = new Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrStringTNTestCase
				(content);
			content = new string[][][] { new string[][] { new string[3], new string[3] } };
			content[0][0][1] = "foo";
			content[0][1][0] = "bar";
			content[0][1][2] = "fly";
			arr[3] = new Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrStringTNTestCase
				(content);
			content = new string[][][] { new string[][] { new string[3], new string[3] } };
			content[0][0][0] = "bar";
			content[0][1][0] = "wohay";
			content[0][1][1] = "johy";
			arr[4] = new Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrStringTNTestCase
				(content);
			object[] ret = new object[arr.Length];
			System.Array.Copy(arr, 0, ret, 0, arr.Length);
			return ret;
		}

		public virtual void TestDefaultContainsOne()
		{
			IQuery q = NewQuery();
			string[][][] content = new string[][][] { new string[][] { new string[1] } };
			content[0][0][0] = "bar";
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrStringTNTestCase
				(content));
			Expect(q, new int[] { 3, 4 });
		}

		public virtual void TestDefaultContainsTwo()
		{
			IQuery q = NewQuery();
			string[][][] content = new string[][][] { new string[][] { new string[1] }, new string
				[][] { new string[1] } };
			content[0][0][0] = "bar";
			content[1][0][0] = "foo";
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrStringTNTestCase
				(content));
			Expect(q, new int[] { 3 });
		}

		public virtual void TestDescendOne()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrStringTNTestCase
				));
			q.Descend("strArr").Constrain("bar");
			Expect(q, new int[] { 3, 4 });
		}

		public virtual void TestDescendTwo()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrStringTNTestCase
				));
			IQuery qElements = q.Descend("strArr");
			qElements.Constrain("foo");
			qElements.Constrain("bar");
			Expect(q, new int[] { 3 });
		}

		public virtual void TestDescendOneNot()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrStringTNTestCase
				));
			q.Descend("strArr").Constrain("bar").Not();
			Expect(q, new int[] { 0, 1, 2 });
		}

		public virtual void TestDescendTwoNot()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrStringTNTestCase
				));
			IQuery qElements = q.Descend("strArr");
			qElements.Constrain("foo").Not();
			qElements.Constrain("bar").Not();
			Expect(q, new int[] { 0, 1, 2 });
		}
	}
}
