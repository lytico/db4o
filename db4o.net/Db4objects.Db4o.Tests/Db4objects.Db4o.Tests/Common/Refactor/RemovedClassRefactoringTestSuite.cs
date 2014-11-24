/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4oUnit.Fixtures;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Tests.Common.Refactor;

namespace Db4objects.Db4o.Tests.Common.Refactor
{
	public class RemovedClassRefactoringTestSuite : FixtureBasedTestSuite, IDb4oTestCase
	{
		private static readonly FixtureVariable DoDefragment = FixtureVariable.NewInstance
			("defrag");

		private static readonly FixtureVariable Indexed = FixtureVariable.NewInstance("indexed"
			);

		private static readonly FixtureVariable ExcludingReflector = FixtureVariable.NewInstance
			("reflector");

		public override IFixtureProvider[] FixtureProviders()
		{
			return new IFixtureProvider[] { new Db4oFixtureProvider(), new SimpleFixtureProvider
				(DoDefragment, new object[] { true, false }), new SimpleFixtureProvider(ExcludingReflector
				, new object[] { new ExcludingReflector(new Type[] { typeof(RemovedClassRefactoringTestSuite.Super
				) }), new ExcludingReflector(new Type[] {  }) }), new SimpleFixtureProvider(Indexed
				, new object[] { true, false }) };
		}

		public override Type[] TestUnits()
		{
			return new Type[] { typeof(RemovedClassRefactoringTestSuite.RemovedClassRefactoringTestUnit
				) };
		}

		public class Super
		{
			public string _superField;

			public Super(string super_)
			{
				_superField = super_;
			}
		}

		public class Sub : RemovedClassRefactoringTestSuite.Super
		{
			public string _subField;

			public Sub(string super_, string sub) : base(super_)
			{
				_subField = sub;
			}
		}

		public class NoSuper
		{
			public NoSuper(string sub)
			{
				_subField = "foo";
			}

			public string _subField;
		}

		public class RemovedClassRefactoringTestUnit : AbstractDb4oTestCase
		{
			/// <exception cref="System.Exception"></exception>
			protected override void Configure(IConfiguration config)
			{
				config.ObjectClass(typeof(RemovedClassRefactoringTestSuite.Sub)).ObjectField("_subField"
					).Indexed((((bool)Indexed.Value)));
			}

			/// <exception cref="System.Exception"></exception>
			protected override void Store()
			{
				RemovedClassRefactoringTestSuite.Sub sub = new RemovedClassRefactoringTestSuite.Sub
					("super", "sub");
				Store(sub);
			}

			/// <exception cref="System.Exception"></exception>
			public virtual void Test()
			{
				Fixture().ResetConfig();
				IConfiguration config = Fixture().Config();
				config.ReflectWith(((IReflector)ExcludingReflector.Value));
				TypeAlias alias = new TypeAlias(typeof(RemovedClassRefactoringTestSuite.Sub), typeof(
					RemovedClassRefactoringTestSuite.NoSuper));
				config.AddAlias(alias);
				if ((((bool)DoDefragment.Value)))
				{
					Defragment();
				}
				else
				{
					Reopen();
				}
				RemovedClassRefactoringTestSuite.NoSuper result = ((RemovedClassRefactoringTestSuite.NoSuper
					)RetrieveOnlyInstance(typeof(RemovedClassRefactoringTestSuite.NoSuper)));
				Assert.AreEqual("sub", result._subField);
				RemovedClassRefactoringTestSuite.NoSuper newSuper = new RemovedClassRefactoringTestSuite.NoSuper
					("foo");
				Store(newSuper);
				IQuery q = NewQuery(typeof(RemovedClassRefactoringTestSuite.NoSuper));
				q.Descend("_subField").Constrain("foo");
				IObjectSet objectSet = q.Execute();
				Assert.AreEqual(1, objectSet.Count);
				result = ((RemovedClassRefactoringTestSuite.NoSuper)objectSet.Next());
				Assert.AreEqual("foo", result._subField);
				Db().Refresh(result, int.MaxValue);
			}
		}
	}
}
