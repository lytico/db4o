/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy;
using Db4objects.Db4o.Tests.Common.Soda.Util;

namespace Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy
{
	/// <summary>ETH: Extends Typed Hierarchy</summary>
	public class STETH1TestCase : SodaBaseTestCase
	{
		public string foo1;

		public STETH1TestCase()
		{
		}

		public STETH1TestCase(string str)
		{
			foo1 = str;
		}

		public override object[] CreateData()
		{
			return new object[] { new Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy.STETH1TestCase
				(), new Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy.STETH1TestCase(
				"str1"), new STETH2(), new STETH2("str1", "str2"), new STETH3(), new STETH3("str1a"
				, "str2", "str3"), new STETH3("str1a", "str2a", null), new STETH4(), new STETH4(
				"str1a", "str2", "str4"), new STETH4("str1b", "str2a", "str4") };
		}

		public virtual void TestStrNull()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy.STETH1TestCase
				());
			q.Descend("foo1").Constrain(null);
			Expect(q, new int[] { 0, 2, 4, 7 });
		}

		public virtual void TestTwoNull()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy.STETH1TestCase
				());
			q.Descend("foo1").Constrain(null);
			q.Descend("foo3").Constrain(null);
			Expect(q, new int[] { 0, 2, 4, 7 });
		}

		public virtual void TestClass()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(STETH2));
			Expect(q, new int[] { 2, 3, 4, 5, 6, 7, 8, 9 });
		}

		public virtual void TestOrClass()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(STETH3)).Or(q.Constrain(typeof(STETH4)));
			Expect(q, new int[] { 4, 5, 6, 7, 8, 9 });
		}

		public virtual void TestAndClass()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy.STETH1TestCase
				));
			q.Constrain(typeof(STETH4));
			Expect(q, new int[] { 7, 8, 9 });
		}

		public virtual void TestParalellDescendantPaths()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(STETH3)).Or(q.Constrain(typeof(STETH4)));
			q.Descend("foo3").Constrain("str3").Or(q.Descend("foo4").Constrain("str4"));
			Expect(q, new int[] { 5, 8, 9 });
		}

		public virtual void TestOrObjects()
		{
			IQuery q = NewQuery();
			q.Constrain(_array[3]).Or(q.Constrain(_array[5]));
			Expect(q, new int[] { 3, 5 });
		}
	}
}
