/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy;
using Db4objects.Db4o.Tests.Common.Soda.Util;

namespace Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy
{
	/// <summary>TH: Typed Hierarchy</summary>
	public class STTH1TestCase : SodaBaseTestCase
	{
		public STTH2 h2;

		public string foo1;

		public STTH1TestCase()
		{
		}

		public STTH1TestCase(STTH2 a2)
		{
			h2 = a2;
		}

		public STTH1TestCase(string str)
		{
			foo1 = str;
		}

		public STTH1TestCase(STTH2 a2, string str)
		{
			h2 = a2;
			foo1 = str;
		}

		public override object[] CreateData()
		{
			return new object[] { new Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy.STTH1TestCase
				(), new Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy.STTH1TestCase("str1"
				), new Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy.STTH1TestCase(new 
				STTH2()), new Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy.STTH1TestCase
				(new STTH2("str2")), new Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy.STTH1TestCase
				(new STTH2(new STTH3("str3"))), new Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy.STTH1TestCase
				(new STTH2(new STTH3("str3"), "str2")) };
		}

		public virtual void TestStrNull()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy.STTH1TestCase
				());
			q.Descend("foo1").Constrain(null);
			Expect(q, new int[] { 0, 2, 3, 4, 5 });
		}

		public virtual void TestBothNull()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy.STTH1TestCase
				());
			q.Descend("foo1").Constrain(null);
			q.Descend("h2").Constrain(null);
			SodaTestUtil.ExpectOne(q, _array[0]);
		}

		public virtual void TestDescendantNotNull()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy.STTH1TestCase
				());
			q.Descend("h2").Constrain(null).Not();
			Expect(q, new int[] { 2, 3, 4, 5 });
		}

		public virtual void TestDescendantDescendantNotNull()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy.STTH1TestCase
				());
			q.Descend("h2").Descend("h3").Constrain(null).Not();
			Expect(q, new int[] { 4, 5 });
		}

		public virtual void TestDescendantExists()
		{
			IQuery q = NewQuery();
			q.Constrain(_array[2]);
			Expect(q, new int[] { 2, 3, 4, 5 });
		}

		public virtual void TestDescendantValue()
		{
			IQuery q = NewQuery();
			q.Constrain(_array[3]);
			Expect(q, new int[] { 3, 5 });
		}

		public virtual void TestDescendantDescendantExists()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy.STTH1TestCase
				(new STTH2(new STTH3())));
			Expect(q, new int[] { 4, 5 });
		}

		public virtual void TestDescendantDescendantValue()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy.STTH1TestCase
				(new STTH2(new STTH3("str3"))));
			Expect(q, new int[] { 4, 5 });
		}

		public virtual void TestDescendantDescendantStringPath()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy.STTH1TestCase
				());
			q.Descend("h2").Descend("h3").Descend("foo3").Constrain("str3");
			Expect(q, new int[] { 4, 5 });
		}

		public virtual void TestSequentialAddition()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy.STTH1TestCase
				());
			IQuery cur = q.Descend("h2");
			cur.Constrain(new STTH2());
			cur.Descend("foo2").Constrain("str2");
			cur = cur.Descend("h3");
			cur.Constrain(new STTH3());
			cur.Descend("foo3").Constrain("str3");
			SodaTestUtil.ExpectOne(q, _array[5]);
		}

		public virtual void TestTwoLevelOr()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy.STTH1TestCase
				("str1"));
			q.Descend("foo1").Constraints().Or(q.Descend("h2").Descend("h3").Descend("foo3").
				Constrain("str3"));
			Expect(q, new int[] { 1, 4, 5 });
		}

		public virtual void TestThreeLevelOr()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy.STTH1TestCase
				("str1"));
			q.Descend("foo1").Constraints().Or(q.Descend("h2").Descend("foo2").Constrain("str2"
				)).Or(q.Descend("h2").Descend("h3").Descend("foo3").Constrain("str3"));
			Expect(q, new int[] { 1, 3, 4, 5 });
		}
	}
}
