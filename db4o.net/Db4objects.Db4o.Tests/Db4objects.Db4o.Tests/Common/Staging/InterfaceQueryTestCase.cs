/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Staging;

namespace Db4objects.Db4o.Tests.Common.Staging
{
	public class InterfaceQueryTestCase : AbstractDb4oTestCase
	{
		private static readonly string FieldA = "fieldA";

		private static readonly string FieldB = "fieldB";

		public interface IIData
		{
		}

		public class DataA : InterfaceQueryTestCase.IIData
		{
			public int fieldA;

			public int fieldB;

			public DataA(int a, int b)
			{
				fieldA = a;
				fieldB = b;
			}
		}

		public class DataB : InterfaceQueryTestCase.IIData
		{
			public int fieldA;

			public int fieldB;

			public DataB(int a, int b)
			{
				fieldA = a;
				fieldB = b;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected virtual void _configure(IConfiguration config)
		{
			ConfigIndexed(config, typeof(InterfaceQueryTestCase.DataA), FieldA);
			ConfigIndexed(config, typeof(InterfaceQueryTestCase.DataA), FieldB);
			ConfigIndexed(config, typeof(InterfaceQueryTestCase.DataB), FieldA);
			ConfigIndexed(config, typeof(InterfaceQueryTestCase.DataB), FieldB);
		}

		private void ConfigIndexed(IConfiguration config, Type clazz, string fieldName)
		{
			config.ObjectClass(clazz).ObjectField(fieldName).Indexed(true);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new InterfaceQueryTestCase.DataA(10, 10));
			Store(new InterfaceQueryTestCase.DataA(20, 20));
			Store(new InterfaceQueryTestCase.DataB(10, 10));
			Store(new InterfaceQueryTestCase.DataB(30, 30));
		}

		public virtual void TestExplicitNotQuery()
		{
			IQuery query = NewQuery();
			query.Constrain(typeof(InterfaceQueryTestCase.DataA)).And(query.Descend(FieldA).Constrain
				(10).Not()).Or(query.Constrain(typeof(InterfaceQueryTestCase.DataB)).And(query.Descend
				(FieldA).Constrain(10).Not()));
			Assert.AreEqual(2, query.Execute().Count);
		}

		public virtual void TestExplicitNotQuery2()
		{
			IQuery query = NewQuery();
			query.Constrain(typeof(InterfaceQueryTestCase.DataA)).Or(query.Constrain(typeof(InterfaceQueryTestCase.DataB
				)));
			query.Descend(FieldA).Constrain(10).Not();
			Assert.AreEqual(2, query.Execute().Count);
		}

		public virtual void TestQueryAll()
		{
			AssertQueryResult(4, new _IQueryConstrainer_73());
		}

		private sealed class _IQueryConstrainer_73 : InterfaceQueryTestCase.IQueryConstrainer
		{
			public _IQueryConstrainer_73()
			{
			}

			public void Constrain(IQuery query)
			{
			}
		}

		public virtual void TestSingleConstraint()
		{
			AssertQueryResult(2, new _IQueryConstrainer_80());
		}

		private sealed class _IQueryConstrainer_80 : InterfaceQueryTestCase.IQueryConstrainer
		{
			public _IQueryConstrainer_80()
			{
			}

			public void Constrain(IQuery query)
			{
				query.Descend(InterfaceQueryTestCase.FieldA).Constrain(10);
			}
		}

		public virtual void TestAnd()
		{
			AssertQueryResult(2, new _IQueryConstrainer_88());
		}

		private sealed class _IQueryConstrainer_88 : InterfaceQueryTestCase.IQueryConstrainer
		{
			public _IQueryConstrainer_88()
			{
			}

			public void Constrain(IQuery query)
			{
				IConstraint icon1 = query.Descend(InterfaceQueryTestCase.FieldA).Constrain(10);
				IConstraint icon2 = query.Descend(InterfaceQueryTestCase.FieldB).Constrain(10);
				icon1.And(icon2);
			}
		}

		public virtual void TestOr()
		{
			AssertQueryResult(2, new _IQueryConstrainer_98());
		}

		private sealed class _IQueryConstrainer_98 : InterfaceQueryTestCase.IQueryConstrainer
		{
			public _IQueryConstrainer_98()
			{
			}

			public void Constrain(IQuery query)
			{
				IConstraint icon1 = query.Descend(InterfaceQueryTestCase.FieldA).Constrain(10);
				IConstraint icon2 = query.Descend(InterfaceQueryTestCase.FieldB).Constrain(10);
				icon1.Or(icon2);
			}
		}

		public virtual void TestNot()
		{
			AssertQueryResult(2, new _IQueryConstrainer_108());
		}

		private sealed class _IQueryConstrainer_108 : InterfaceQueryTestCase.IQueryConstrainer
		{
			public _IQueryConstrainer_108()
			{
			}

			public void Constrain(IQuery query)
			{
				query.Descend(InterfaceQueryTestCase.FieldA).Constrain(10).Not();
			}
		}

		public virtual void AssertQueryResult(int expected, InterfaceQueryTestCase.IQueryConstrainer
			 constrainer)
		{
			IQuery query = NewQuery(typeof(InterfaceQueryTestCase.IIData));
			constrainer.Constrain(query);
			Assert.AreEqual(expected, query.Execute().Count);
		}

		public interface IQueryConstrainer
		{
			void Constrain(IQuery query);
		}
	}
}
