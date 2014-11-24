/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda.Util;

namespace Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped
{
	[System.Serializable]
	public class STCharWUTestCase : SodaBaseTestCase
	{
		internal static readonly string Descendant = "i_char";

		public object i_char;

		public STCharWUTestCase()
		{
		}

		private STCharWUTestCase(char a_char)
		{
			i_char = a_char;
		}

		public override object[] CreateData()
		{
			return new object[] { new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STCharWUTestCase
				((char)0), new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STCharWUTestCase
				((char)1), new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STCharWUTestCase
				((char)99), new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STCharWUTestCase
				((char)909) };
		}

		public virtual void TestEquals()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STCharWUTestCase
				((char)0));
			// Primitive default values are ignored, so we need an 
			// additional constraint:
			q.Descend(Descendant).Constrain((char)0);
			SodaTestUtil.ExpectOne(q, _array[0]);
		}

		public virtual void TestNotEquals()
		{
			IQuery q = NewQuery();
			q.Constrain(_array[0]);
			q.Descend(Descendant).Constraints().Not();
			Expect(q, new int[] { 1, 2, 3 });
		}

		public virtual void TestGreater()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STCharWUTestCase
				((char)9));
			q.Descend(Descendant).Constraints().Greater();
			Expect(q, new int[] { 2, 3 });
		}

		public virtual void TestSmaller()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STCharWUTestCase
				((char)1));
			q.Descend(Descendant).Constraints().Smaller();
			SodaTestUtil.ExpectOne(q, _array[0]);
		}

		public virtual void TestIdentity()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STCharWUTestCase
				((char)1));
			IObjectSet set = q.Execute();
			Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STCharWUTestCase identityConstraint
				 = (Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STCharWUTestCase)set.Next(
				);
			identityConstraint.i_char = (char)9999;
			q = NewQuery();
			q.Constrain(identityConstraint).Identity();
			identityConstraint.i_char = (char)1;
			SodaTestUtil.ExpectOne(q, _array[1]);
		}

		public virtual void TestNotIdentity()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STCharWUTestCase
				((char)1));
			IObjectSet set = q.Execute();
			Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STCharWUTestCase identityConstraint
				 = (Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STCharWUTestCase)set.Next(
				);
			identityConstraint.i_char = (char)9080;
			q = NewQuery();
			q.Constrain(identityConstraint).Identity().Not();
			identityConstraint.i_char = (char)1;
			Expect(q, new int[] { 0, 2, 3 });
		}

		public virtual void TestConstraints()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STCharWUTestCase
				((char)1));
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STCharWUTestCase
				((char)0));
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
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STCharWUTestCase
				());
			q.Constrain(new _IEvaluation_104());
			Expect(q, new int[] { 2, 3 });
		}

		private sealed class _IEvaluation_104 : IEvaluation
		{
			public _IEvaluation_104()
			{
			}

			public void Evaluate(ICandidate candidate)
			{
				Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STCharWUTestCase sts = (Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STCharWUTestCase
					)candidate.GetObject();
				candidate.Include((((char)sts.i_char) + 2) > 100);
			}
		}
	}
}
