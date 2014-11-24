/* Copyright (C) 2004 - 2006  Versant Inc.   http://www.db4o.com */

using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using Db4oUnit;

public class Item
{
	private string _name;

	public Item(string name)
	{
		_name = name;
	}

	public string Name
	{
		get { return _name; }
	}
}

class ByUpperNameUnoptimizable : Predicate
{
	public bool Match(Item candidate)
	{
		return candidate.Name.ToUpper() == "FOO";
	}
}

class ByName : Predicate
{
	public bool Match(Item candidate)
	{
		return candidate.Name == "bar";
	}
}

public class UnoptimizablePredicateSubject : Db4oTool.Tests.Core.InstrumentedTestCase
{
	override public void SetUp()
	{
		_container.Store(new Item("foo"));
		_container.Store(new Item("bar"));
	}
	
	public void TestByUpperName()
	{
		IObjectSet result = _container.Query(new ByUpperNameUnoptimizable());
		Assert.AreEqual(1, result.Count);
		Assert.AreEqual("foo", (result[0] as Item).Name);
	}
	
	public void TestByName()
	{
		IObjectSet result = _container.Query(new ByName());
		Assert.AreEqual(1, result.Count);
		Assert.AreEqual("bar", (result[0] as Item).Name);
	}
}