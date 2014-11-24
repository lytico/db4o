/* Copyright (C) 2009   Versant Inc.   http://www.db4o.com */
using System;
using System.Collections.Generic;
using System.IO;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Diagnostic;
using Db4oUnit;
using Db4oUnit.Extensions.Util;

class GenericClassTestSubject : Db4oTool.Tests.Core.InstrumentedTestCase
{
	private delegate void Action<T>(T arg);

	public void TestGenericClass()
	{
		AssertQueryIsOptimized(
			delegate(IObjectContainer container)
			{
					new GenericClassHelper<Item>().RunQuery(container);
			}
		);
	}

	public void TestGenericMethod()
	{
		AssertQueryIsOptimized(
			delegate(IObjectContainer container)
			{
				new GenericMethodHelper().RunQuery<Item>(container);
			}
		);
	}

	public void TestPredicateContainingBoxIntructionAreOptimized()
	{
		AssertQueryIsOptimized(
			delegate(IObjectContainer container)
			{
				container.Query<Item>(
					delegate(Item candidate)
					{
						return candidate.id == 1;
					});
			}
		);
	}
	
	private void AssertQueryIsOptimized(Action<IObjectContainer> action)
	{
		DiagnosticCollector<NativeQueryNotOptimized> collector = new DiagnosticCollector<NativeQueryNotOptimized>();
		using (IObjectContainer container = Db4oEmbedded.OpenFile(NewConfiguration(collector), Path.GetTempFileName()))
		{
			action(container);
		}

		Assert.AreEqual(0, collector.Diagnostics.Count);
	}

	private static IEmbeddedConfiguration NewConfiguration(IDiagnosticListener diagnosticCollector)
	{
		IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
		configuration.Common.Diagnostic.AddListener(diagnosticCollector);
		
		return configuration;
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

	private bool QueryMethod<T>(T candidate) where T : Item
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
