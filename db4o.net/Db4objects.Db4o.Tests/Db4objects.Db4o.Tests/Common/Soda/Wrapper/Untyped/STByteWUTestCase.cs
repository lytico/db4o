/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda.Util;

namespace Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped
{
	[System.Serializable]
	public class STByteWUTestCase : SodaBaseTestCase
	{
		internal static readonly string Descendant = "i_byte";

		public object i_byte;

		public STByteWUTestCase()
		{
		}

		private STByteWUTestCase(byte a_byte)
		{
			i_byte = a_byte;
		}

		public override object[] CreateData()
		{
			return new object[] { new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STByteWUTestCase
				((byte)0), new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STByteWUTestCase
				((byte)1), new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STByteWUTestCase
				((byte)99), new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STByteWUTestCase
				((byte)113) };
		}

		public virtual void TestEquals()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STByteWUTestCase
				((byte)0));
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
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STByteWUTestCase
				((byte)9));
			q.Descend(Descendant).Constraints().Greater();
			Expect(q, new int[] { 2, 3 });
		}

		public virtual void TestSmaller()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STByteWUTestCase
				((byte)1));
			q.Descend(Descendant).Constraints().Smaller();
			SodaTestUtil.ExpectOne(q, _array[0]);
		}

		public virtual void TestContains()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STByteWUTestCase
				((byte)9));
			q.Descend(Descendant).Constraints().Contains();
			Expect(q, new int[] { 2 });
		}

		public virtual void TestNotContains()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STByteWUTestCase
				((byte)0));
			q.Descend(Descendant).Constraints().Contains().Not();
			Expect(q, new int[] { 1, 2, 3 });
		}

		public virtual void TestLike()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STByteWUTestCase
				((byte)11));
			q.Descend(Descendant).Constraints().Like();
			SodaTestUtil.ExpectOne(q, new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STByteWUTestCase
				((byte)113));
			q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STByteWUTestCase
				((byte)10));
			q.Descend(Descendant).Constraints().Like();
			Expect(q, new int[] {  });
		}

		public virtual void TestNotLike()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STByteWUTestCase
				((byte)1));
			q.Descend(Descendant).Constraints().Like().Not();
			Expect(q, new int[] { 0, 2 });
		}

		public virtual void TestIdentity()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STByteWUTestCase
				((byte)1));
			IObjectSet set = q.Execute();
			Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STByteWUTestCase identityConstraint
				 = (Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STByteWUTestCase)set.Next(
				);
			identityConstraint.i_byte = (byte)102;
			q = NewQuery();
			q.Constrain(identityConstraint).Identity();
			identityConstraint.i_byte = (byte)1;
			SodaTestUtil.ExpectOne(q, _array[1]);
		}

		public virtual void TestNotIdentity()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STByteWUTestCase
				((byte)1));
			IObjectSet set = q.Execute();
			Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STByteWUTestCase identityConstraint
				 = (Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STByteWUTestCase)set.Next(
				);
			identityConstraint.i_byte = (byte)102;
			q = NewQuery();
			q.Constrain(identityConstraint).Identity().Not();
			identityConstraint.i_byte = (byte)1;
			Expect(q, new int[] { 0, 2, 3 });
		}

		public virtual void TestConstraints()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STByteWUTestCase
				((byte)1));
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STByteWUTestCase
				((byte)0));
			IConstraints cs = q.Constraints();
			IConstraint[] csa = cs.ToArray();
			if (csa.Length != 2)
			{
				Assert.Fail("Constraints not returned");
			}
		}

		public virtual void TestNull()
		{
		}

		public virtual void TestEvaluation()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STByteWUTestCase
				());
			q.Constrain(new _IEvaluation_139());
			Expect(q, new int[] { 2, 3 });
		}

		private sealed class _IEvaluation_139 : IEvaluation
		{
			public _IEvaluation_139()
			{
			}

			public void Evaluate(ICandidate candidate)
			{
				Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STByteWUTestCase sts = (Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STByteWUTestCase
					)candidate.GetObject();
				candidate.Include((((byte)sts.i_byte) + 2) > 100);
			}
		}
	}
}
