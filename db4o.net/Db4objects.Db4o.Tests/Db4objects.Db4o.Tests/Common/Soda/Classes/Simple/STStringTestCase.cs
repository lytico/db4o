/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda;
using Db4objects.Db4o.Tests.Common.Soda.Util;

namespace Db4objects.Db4o.Tests.Common.Soda.Classes.Simple
{
	public class STStringTestCase : SodaBaseTestCase, ISTInterface
	{
		public string str;

		public STStringTestCase()
		{
		}

		public STStringTestCase(string str)
		{
			this.str = str;
		}

		/// <summary>needed for STInterface test</summary>
		public virtual object ReturnSomething()
		{
			return str;
		}

		public override object[] CreateData()
		{
			return new object[] { new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase
				(null), new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase("aaa"
				), new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase("bbb"), 
				new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase("dod") };
		}

		public virtual void TestEquals()
		{
			IQuery q = NewQuery();
			q.Constrain(_array[2]);
			SodaTestUtil.ExpectOne(q, _array[2]);
		}

		public virtual void TestNotEquals()
		{
			IQuery q = NewQuery();
			q.Constrain(_array[2]);
			q.Descend("str").Constraints().Not();
			Expect(q, new int[] { 0, 1, 3 });
		}

		public virtual void TestDescendantEquals()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase
				());
			q.Descend("str").Constrain("bbb");
			SodaTestUtil.ExpectOne(q, new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase
				("bbb"));
		}

		public virtual void TestContains()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase
				("od"));
			q.Descend("str").Constraints().Contains();
			SodaTestUtil.ExpectOne(q, new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase
				("dod"));
		}

		public virtual void TestNotContains()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase
				("od"));
			q.Descend("str").Constraints().Contains().Not();
			SodaTestUtil.Expect(q, new object[] { new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase
				(null), new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase("aaa"
				), new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase("bbb") }
				);
		}

		public virtual void TestLike()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase
				("do"));
			q.Descend("str").Constraints().Like();
			SodaTestUtil.ExpectOne(q, new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase
				("dod"));
			q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase
				("od"));
			q.Descend("str").Constraints().Like();
			SodaTestUtil.ExpectOne(q, _array[3]);
		}

		public virtual void TestStartsWith()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase
				("do"));
			q.Descend("str").Constraints().StartsWith(true);
			SodaTestUtil.ExpectOne(q, new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase
				("dod"));
			q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase
				("od"));
			q.Descend("str").Constraints().StartsWith(true);
			Expect(q, new int[] {  });
			q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase
				("dodo"));
			q.Descend("str").Constraints().StartsWith(true);
			Expect(q, new int[] {  });
		}

		public virtual void TestEndsWith()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase
				("do"));
			q.Descend("str").Constraints().EndsWith(true);
			Expect(q, new int[] {  });
			q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase
				("od"));
			q.Descend("str").Constraints().EndsWith(true);
			SodaTestUtil.ExpectOne(q, new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase
				("dod"));
			q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase
				("D"));
			q.Descend("str").Constraints().EndsWith(false);
			SodaTestUtil.ExpectOne(q, new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase
				("dod"));
			q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase
				("dodo"));
			// COR-413
			q.Descend("str").Constraints().EndsWith(false);
			Expect(q, new int[] {  });
		}

		public virtual void TestNotLike()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase
				("aaa"));
			q.Descend("str").Constraints().Like().Not();
			SodaTestUtil.Expect(q, new object[] { new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase
				(null), new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase("bbb"
				), new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase("dod") }
				);
			q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase
				("xxx"));
			q.Descend("str").Constraints().Like();
			Expect(q, new int[] {  });
		}

		public virtual void TestIdentity()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase
				("aaa"));
			IObjectSet set = q.Execute();
			Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase identityConstraint
				 = (Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase)set.Next();
			identityConstraint.str = "hihs";
			q = NewQuery();
			q.Constrain(identityConstraint).Identity();
			identityConstraint.str = "aaa";
			SodaTestUtil.ExpectOne(q, new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase
				("aaa"));
		}

		public virtual void TestNotIdentity()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase
				("aaa"));
			IObjectSet set = q.Execute();
			Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase identityConstraint
				 = (Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase)set.Next();
			identityConstraint.str = null;
			q = NewQuery();
			q.Constrain(identityConstraint).Identity().Not();
			identityConstraint.str = "aaa";
			SodaTestUtil.Expect(q, new object[] { new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase
				(null), new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase("bbb"
				), new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase("dod") }
				);
		}

		public virtual void TestNull()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase
				(null));
			q.Descend("str").Constrain(null);
			SodaTestUtil.ExpectOne(q, new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase
				(null));
		}

		public virtual void TestNotNull()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase
				(null));
			q.Descend("str").Constrain(null).Not();
			SodaTestUtil.Expect(q, new object[] { new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase
				("aaa"), new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase("bbb"
				), new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase("dod") }
				);
		}

		public virtual void TestConstraints()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase
				("aaa"));
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase
				("bbb"));
			IConstraints cs = q.Constraints();
			IConstraint[] csa = cs.ToArray();
			if (csa.Length != 2)
			{
				Assert.Fail("Constraints not returned");
			}
		}

		public virtual void TestEvaluation()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase
				(null));
			q.Constrain(new _IEvaluation_187());
			SodaTestUtil.ExpectOne(q, new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase
				("dod"));
		}

		private sealed class _IEvaluation_187 : IEvaluation
		{
			public _IEvaluation_187()
			{
			}

			public void Evaluate(ICandidate candidate)
			{
				Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase sts = (Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase
					)candidate.GetObject();
				candidate.Include(sts.str.IndexOf("od") == 1);
			}
		}

		public virtual void TestCaseInsenstiveContains()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase
				));
			q.Constrain(new _IEvaluation_199());
			SodaTestUtil.ExpectOne(q, new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase
				("dod"));
		}

		private sealed class _IEvaluation_199 : IEvaluation
		{
			public _IEvaluation_199()
			{
			}

			public void Evaluate(ICandidate candidate)
			{
				Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase sts = (Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STStringTestCase
					)candidate.GetObject();
				candidate.Include(sts.str.ToLower().IndexOf("od") >= 0);
			}
		}
	}
}
