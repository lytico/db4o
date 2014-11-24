/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy;
using Db4objects.Db4o.Tests.Common.Soda.Util;

namespace Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy
{
	/// <summary>SDFT: Same descendant field typed</summary>
	public class STSDFT1TestCase : SodaBaseTestCase
	{
		public STSDFT1TestCase()
		{
		}

		public override object[] CreateData()
		{
			return new object[] { new Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy.STSDFT1TestCase
				(), new STSDFT2(), new STSDFT2("str1"), new STSDFT2("str2"), new STSDFT3(), new 
				STSDFT3("str1"), new STSDFT3("str3") };
		}

		public virtual void TestStrNull()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy.STSDFT1TestCase
				());
			q.Descend("foo").Constrain(null);
			Expect(q, new int[] { 0, 1, 4 });
		}

		public virtual void TestStrVal()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy.STSDFT1TestCase
				));
			q.Descend("foo").Constrain("str1");
			Expect(q, new int[] { 2, 5 });
		}

		public virtual void TestOrValue()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy.STSDFT1TestCase
				));
			IQuery foo = q.Descend("foo");
			foo.Constrain("str1").Or(foo.Constrain("str2"));
			Expect(q, new int[] { 2, 3, 5 });
		}

		public virtual void TestOrNull()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy.STSDFT1TestCase
				));
			IQuery foo = q.Descend("foo");
			foo.Constrain("str1").Or(foo.Constrain(null));
			Expect(q, new int[] { 0, 1, 2, 4, 5 });
		}

		public virtual void TestTripleOrNull()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy.STSDFT1TestCase
				));
			IQuery foo = q.Descend("foo");
			foo.Constrain("str1").Or(foo.Constrain(null)).Or(foo.Constrain("str2"));
			Expect(q, new int[] { 0, 1, 2, 3, 4, 5 });
		}
		// work in progress
		//	public void testOverConstrainedByClass(){
		//		Query q = SodaTenewQuery();
		//		q.constrain(STSDFT1TestCase.class).or(q.constrain(STSDFT2.class));
		//		Query foo = q.descend("foo");
		//		foo.constrain("str1").or(foo.constrain(null)).or(foo.constrain("str2"));
		//		
		//		SodaTeexpect(q, new int[] {0, 1, 2,3, 4, 5});
		//	}
	}
}
