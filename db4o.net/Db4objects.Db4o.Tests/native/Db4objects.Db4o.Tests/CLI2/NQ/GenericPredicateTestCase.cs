/* Copyright (C) 2009   Versant Inc.   http://www.db4o.com */
using System.Collections.Generic;
using Db4objects.Db4o;
using Db4objects.Db4o.Diagnostic;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4oUnit.Extensions.Util;

namespace Db4objects.Db4o.Tests.CLI2.NQ
{
	public class GenericPredicateTestCase : AbstractDb4oTestCase, IOptOutSilverlight
	{
		protected override void Configure(Config.IConfiguration config)
		{
			config.Diagnostic().AddListener(_collector);
		}

		public void TestGenericClass()
		{
			AssertQueryIsOptimized(
				delegate
				{
					new GenericClassHelper<Item>().RunQuery(Db());
				}
			);
		}

		public void TestGenericMethod()
		{
			AssertQueryIsOptimized(
				delegate
				{
					new GenericMethodHelper().RunQuery<Item>(Db());
				}
			);
		}

		public void TestPredicateContainingBoxIntructionAreOptimized()
		{
			AssertQueryIsOptimized(
				delegate
				{
					Db().Query<Item>(
						delegate(Item candidate)
						{
							return candidate.id == 1;
						});
				}
			);
		}

		private void AssertQueryIsOptimized(CodeBlock action)
		{
			action();
			Assert.AreEqual(0, _collector.Diagnostics.Count);
		}

		private DiagnosticCollector<NativeQueryNotOptimized> _collector = new DiagnosticCollector<NativeQueryNotOptimized>();
	}
}

public class GenericClassHelper<T> where T : Item
{
	public IList<T> RunQuery(IObjectContainer container)
	{
		return container.Query<T>(QueryMethod);
	}

	public bool QueryMethod(T candidate)
	{
		return candidate.name == "doenst matter";
	}
}

public class GenericMethodHelper
{
	public IList<T> RunQuery<T>(IObjectContainer container) where T : Item
	{
		return container.Query<T>(QueryMethod);
	}

	private static bool QueryMethod<T>(T candidate) where T : Item
	{
		return candidate.name == "doenst matter";
	}
}

public class Item
{
	public Item(int id, string name)
	{
		this.id = id;
		this.name = name;
	}

	public int id;
	public string name;
}

