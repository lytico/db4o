/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda.Util;

namespace Db4objects.Db4o.Tests.Common.Soda.Classes.Simple
{
	public class STByteTestCase : SodaBaseTestCase
	{
		internal static readonly string Descendant = "i_byte";

		public byte i_byte;

		public STByteTestCase()
		{
		}

		private STByteTestCase(byte a_byte)
		{
			i_byte = a_byte;
		}

		public override object[] CreateData()
		{
			return new object[] { new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STByteTestCase
				((byte)0), new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STByteTestCase((
				byte)1), new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STByteTestCase((byte
				)99), new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STByteTestCase((byte)
				113) };
		}

		public virtual void TestEquals()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STByteTestCase((
				byte)0));
			// Primitive default values are ignored, so we need an 
			// additional constraint:
			q.Descend(Descendant).Constrain((byte)0);
			SodaTestUtil.ExpectOne(q, _array[0]);
		}

		public virtual void TestNotEquals()
		{
			IQuery q = NewQuery();
			q.Constrain(_array[0]);
			q.Descend(Descendant).Constrain((byte)0).Not();
			Expect(q, new int[] { 1, 2, 3 });
		}

		public virtual void TestGreater()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STByteTestCase((
				byte)9));
			q.Descend(Descendant).Constraints().Greater();
			Expect(q, new int[] { 2, 3 });
		}

		public virtual void TestSmaller()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STByteTestCase((
				byte)1));
			q.Descend(Descendant).Constraints().Smaller();
			SodaTestUtil.ExpectOne(q, _array[0]);
		}

		public virtual void TestContains()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STByteTestCase((
				byte)9));
			q.Descend(Descendant).Constraints().Contains();
			Expect(q, new int[] { 2 });
		}

		public virtual void TestNotContains()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STByteTestCase((
				byte)0));
			q.Descend(Descendant).Constrain((byte)0).Contains().Not();
			Expect(q, new int[] { 1, 2, 3 });
		}

		public virtual void TestLike()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STByteTestCase((
				byte)11));
			q.Descend(Descendant).Constraints().Like();
			SodaTestUtil.ExpectOne(q, new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STByteTestCase
				((byte)113));
			q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STByteTestCase((
				byte)10));
			q.Descend(Descendant).Constraints().Like();
			Expect(q, new int[] {  });
		}

		public virtual void TestNotLike()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STByteTestCase((
				byte)1));
			q.Descend(Descendant).Constraints().Like().Not();
			Expect(q, new int[] { 0, 2 });
		}

		public virtual void TestIdentity()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STByteTestCase((
				byte)1));
			IObjectSet set = q.Execute();
			Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STByteTestCase identityConstraint
				 = (Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STByteTestCase)set.Next();
			identityConstraint.i_byte = 102;
			q = NewQuery();
			q.Constrain(identityConstraint).Identity();
			identityConstraint.i_byte = 1;
			SodaTestUtil.ExpectOne(q, _array[1]);
		}

		public virtual void TestNotIdentity()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STByteTestCase((
				byte)1));
			IObjectSet set = q.Execute();
			Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STByteTestCase identityConstraint
				 = (Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STByteTestCase)set.Next();
			identityConstraint.i_byte = 102;
			q = NewQuery();
			q.Constrain(identityConstraint).Identity().Not();
			identityConstraint.i_byte = 1;
			Expect(q, new int[] { 0, 2, 3 });
		}

		public virtual void TestConstraints()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STByteTestCase((
				byte)1));
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STByteTestCase((
				byte)0));
			IConstraints cs = q.Constraints();
			IConstraint[] csa = cs.ToArray();
			if (csa.Length != 2)
			{
				Assert.Fail("Constraints not returned");
			}
		}
	}
}
