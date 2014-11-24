/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda.Util;

namespace Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped
{
	[System.Serializable]
	public class STIntegerWUTestCase : SodaBaseTestCase
	{
		public object i_int;

		public STIntegerWUTestCase()
		{
		}

		private STIntegerWUTestCase(int a_int)
		{
			i_int = a_int;
		}

		public override object[] CreateData()
		{
			return new object[] { new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STIntegerWUTestCase
				(0), new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STIntegerWUTestCase(1
				), new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STIntegerWUTestCase(99)
				, new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STIntegerWUTestCase(909)
				 };
		}

		public virtual void TestEquals()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STIntegerWUTestCase
				(0));
			// Primitive default values are ignored, so we need an 
			// additional constraint:
			q.Descend("i_int").Constrain(0);
			SodaTestUtil.ExpectOne(q, _array[0]);
		}

		public virtual void TestNotEquals()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STIntegerWUTestCase
				());
			q.Descend("i_int").Constrain(0).Not();
			Expect(q, new int[] { 1, 2, 3 });
		}

		public virtual void TestGreater()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STIntegerWUTestCase
				(9));
			q.Descend("i_int").Constraints().Greater();
			Expect(q, new int[] { 2, 3 });
		}

		public virtual void TestSmaller()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STIntegerWUTestCase
				(1));
			q.Descend("i_int").Constraints().Smaller();
			SodaTestUtil.ExpectOne(q, _array[0]);
		}

		public virtual void TestContains()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STIntegerWUTestCase
				(9));
			q.Descend("i_int").Constraints().Contains();
			Expect(q, new int[] { 2, 3 });
		}

		public virtual void TestNotContains()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STIntegerWUTestCase
				());
			q.Descend("i_int").Constrain(0).Contains().Not();
			Expect(q, new int[] { 1, 2 });
		}

		public virtual void TestLike()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STIntegerWUTestCase
				(90));
			q.Descend("i_int").Constraints().Like();
			SodaTestUtil.ExpectOne(q, new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STIntegerWUTestCase
				(909));
			q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STIntegerWUTestCase
				(10));
			q.Descend("i_int").Constraints().Like();
			Expect(q, new int[] {  });
		}

		public virtual void TestNotLike()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STIntegerWUTestCase
				(1));
			q.Descend("i_int").Constraints().Like().Not();
			Expect(q, new int[] { 0, 2, 3 });
		}

		public virtual void TestIdentity()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STIntegerWUTestCase
				(1));
			IObjectSet set = q.Execute();
			Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STIntegerWUTestCase identityConstraint
				 = (Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STIntegerWUTestCase)set.Next
				();
			identityConstraint.i_int = 9999;
			q = NewQuery();
			q.Constrain(identityConstraint).Identity();
			identityConstraint.i_int = 1;
			SodaTestUtil.ExpectOne(q, _array[1]);
		}

		public virtual void TestNotIdentity()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STIntegerWUTestCase
				(1));
			IObjectSet set = q.Execute();
			Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STIntegerWUTestCase identityConstraint
				 = (Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STIntegerWUTestCase)set.Next
				();
			identityConstraint.i_int = 9080;
			q = NewQuery();
			q.Constrain(identityConstraint).Identity().Not();
			identityConstraint.i_int = 1;
			Expect(q, new int[] { 0, 2, 3 });
		}

		public virtual void TestConstraints()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STIntegerWUTestCase
				(1));
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STIntegerWUTestCase
				(0));
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
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STIntegerWUTestCase
				());
			q.Constrain(new _IEvaluation_137());
			Expect(q, new int[] { 2, 3 });
		}

		private sealed class _IEvaluation_137 : IEvaluation
		{
			public _IEvaluation_137()
			{
			}

			public void Evaluate(ICandidate candidate)
			{
				Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STIntegerWUTestCase sti = (Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STIntegerWUTestCase
					)candidate.GetObject();
				candidate.Include((((int)sti.i_int) + 2) > 100);
			}
		}
	}
}
