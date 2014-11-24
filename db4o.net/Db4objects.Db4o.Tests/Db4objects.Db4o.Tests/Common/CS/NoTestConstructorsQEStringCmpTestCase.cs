/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.CS;

namespace Db4objects.Db4o.Tests.Common.CS
{
	public class NoTestConstructorsQEStringCmpTestCase : AbstractDb4oTestCase, IOptOutAllButNetworkingCS
	{
		public class Item
		{
			public string _name;

			public Item(string name)
			{
				_name = name;
			}
		}

		private interface IConstraintModifier
		{
			void Modify(IConstraint constraint);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.CallConstructors(true);
			config.TestConstructors(false);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new NoTestConstructorsQEStringCmpTestCase.Item("abc"));
		}

		public virtual void TestStartsWith()
		{
			AssertSingleItem("a", new _IConstraintModifier_35());
		}

		private sealed class _IConstraintModifier_35 : NoTestConstructorsQEStringCmpTestCase.IConstraintModifier
		{
			public _IConstraintModifier_35()
			{
			}

			public void Modify(IConstraint constraint)
			{
				constraint.StartsWith(false);
			}
		}

		public virtual void TestEndsWith()
		{
			AssertSingleItem("c", new _IConstraintModifier_43());
		}

		private sealed class _IConstraintModifier_43 : NoTestConstructorsQEStringCmpTestCase.IConstraintModifier
		{
			public _IConstraintModifier_43()
			{
			}

			public void Modify(IConstraint constraint)
			{
				constraint.EndsWith(false);
			}
		}

		public virtual void TestContains()
		{
			AssertSingleItem("b", new _IConstraintModifier_51());
		}

		private sealed class _IConstraintModifier_51 : NoTestConstructorsQEStringCmpTestCase.IConstraintModifier
		{
			public _IConstraintModifier_51()
			{
			}

			public void Modify(IConstraint constraint)
			{
				constraint.Contains();
			}
		}

		private void AssertSingleItem(string pattern, NoTestConstructorsQEStringCmpTestCase.IConstraintModifier
			 modifier)
		{
			IQuery query = BaseQuery(pattern, modifier);
			IObjectSet result = query.Execute();
			Assert.AreEqual(1, result.Count);
		}

		private IQuery BaseQuery(string pattern, NoTestConstructorsQEStringCmpTestCase.IConstraintModifier
			 modifier)
		{
			IQuery query = NewQuery();
			query.Constrain(typeof(NoTestConstructorsQEStringCmpTestCase.Item));
			IConstraint constraint = query.Descend("_name").Constrain(pattern);
			modifier.Modify(constraint);
			return query;
		}

		public static void Main(string[] args)
		{
			new NoTestConstructorsQEStringCmpTestCase().RunNetworking();
		}
	}
}
#endif // !SILVERLIGHT
