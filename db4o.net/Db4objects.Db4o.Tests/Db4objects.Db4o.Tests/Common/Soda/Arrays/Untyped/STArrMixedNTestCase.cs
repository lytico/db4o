/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda.Util;
using Sharpen;

namespace Db4objects.Db4o.Tests.Common.Soda.Arrays.Untyped
{
	public class STArrMixedNTestCase : SodaBaseTestCase
	{
		public object[][][] arr;

		public STArrMixedNTestCase()
		{
		}

		public STArrMixedNTestCase(object[][][] arr)
		{
			this.arr = arr;
		}

		public override object[] CreateData()
		{
			Db4objects.Db4o.Tests.Common.Soda.Arrays.Untyped.STArrMixedNTestCase[] arrMixed = 
				new Db4objects.Db4o.Tests.Common.Soda.Arrays.Untyped.STArrMixedNTestCase[5];
			arrMixed[0] = new Db4objects.Db4o.Tests.Common.Soda.Arrays.Untyped.STArrMixedNTestCase
				();
			object[][][] content = new object[][][] { new object[][] { new object[2] } };
			arrMixed[1] = new Db4objects.Db4o.Tests.Common.Soda.Arrays.Untyped.STArrMixedNTestCase
				(content);
			content = new object[][][] { new object[][] { new object[3], new object[3] }, new 
				object[][] { new object[3], new object[3] } };
			arrMixed[2] = new Db4objects.Db4o.Tests.Common.Soda.Arrays.Untyped.STArrMixedNTestCase
				(content);
			content = new object[][][] { new object[][] { new object[3], new object[3] }, new 
				object[][] { new object[3], new object[3] } };
			content[0][0][1] = "foo";
			content[0][1][0] = "bar";
			content[0][1][2] = "fly";
			content[1][0][0] = false;
			arrMixed[3] = new Db4objects.Db4o.Tests.Common.Soda.Arrays.Untyped.STArrMixedNTestCase
				(content);
			content = new object[][][] { new object[][] { new object[3], new object[3] }, new 
				object[][] { new object[3], new object[3] } };
			content[0][0][0] = "bar";
			content[0][1][0] = "wohay";
			content[0][1][1] = "johy";
			content[1][0][0] = 12;
			arrMixed[4] = new Db4objects.Db4o.Tests.Common.Soda.Arrays.Untyped.STArrMixedNTestCase
				(content);
			object[] ret = new object[arrMixed.Length];
			System.Array.Copy(arrMixed, 0, ret, 0, arrMixed.Length);
			return ret;
		}

		public virtual void TestDefaultContainsString()
		{
			IQuery q = NewQuery();
			object[][][] content = new object[][][] { new object[][] { new object[1] } };
			content[0][0][0] = "bar";
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Arrays.Untyped.STArrMixedNTestCase
				(content));
			Expect(q, new int[] { 3, 4 });
		}

		public virtual void TestDefaultContainsInteger()
		{
			IQuery q = NewQuery();
			object[][][] content = new object[][][] { new object[][] { new object[1] } };
			content[0][0][0] = 12;
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Arrays.Untyped.STArrMixedNTestCase
				(content));
			Expect(q, new int[] { 4 });
		}

		public virtual void TestDefaultContainsBoolean()
		{
			IQuery q = NewQuery();
			object[][][] content = new object[][][] { new object[][] { new object[1] } };
			content[0][0][0] = false;
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Arrays.Untyped.STArrMixedNTestCase
				(content));
			Expect(q, new int[] { 3 });
		}

		public virtual void TestDefaultContainsTwo()
		{
			IQuery q = NewQuery();
			object[][][] content = new object[][][] { new object[][] { new object[1] }, new object
				[][] { new object[1] } };
			content[0][0][0] = "bar";
			content[1][0][0] = 12;
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Arrays.Untyped.STArrMixedNTestCase
				(content));
			Expect(q, new int[] { 4 });
		}

		public virtual void TestDescendOne()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Arrays.Untyped.STArrMixedNTestCase
				));
			q.Descend("arr").Constrain("bar");
			Expect(q, new int[] { 3, 4 });
		}

		public virtual void TestDescendTwo()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Arrays.Untyped.STArrMixedNTestCase
				));
			IQuery qElements = q.Descend("arr");
			qElements.Constrain("foo");
			qElements.Constrain("bar");
			Expect(q, new int[] { 3 });
		}

		public virtual void TestDescendOneNot()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Arrays.Untyped.STArrMixedNTestCase
				));
			q.Descend("arr").Constrain("bar").Not();
			Expect(q, new int[] { 0, 1, 2 });
		}

		public virtual void TestDescendTwoNot()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Arrays.Untyped.STArrMixedNTestCase
				));
			IQuery qElements = q.Descend("arr");
			qElements.Constrain("foo").Not();
			qElements.Constrain("bar").Not();
			Expect(q, new int[] { 0, 1, 2 });
		}
	}
}
