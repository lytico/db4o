/* Copyright (C) 2004 - 2006  Versant Inc.   http://www.db4o.com */

using System;
using System.IO;
using System.Collections.Generic;
using Db4objects.Db4o;
using Db4oUnit;

public class Item
{
	private string _name;
    private int _value;

    public Item(string name)
        : this(name, 0)
    {
        _value = System.Environment.TickCount;
    }

	public Item(string name, int value)
	{
		_name = name;
        _value = value;
	}

	public string Name
	{
		get { return _name; }
	}

    public int Value
    {
        get { return _value; }
    }
}

// TODO: query invocation with comparator
// TODO: query invocation with comparison
public class DelegateSubject : Db4oTool.Tests.Core.InstrumentedTestCase
{
    //private static int staticValue;

	override public void SetUp()
	{	
		_container.Store(new Item("foo", 1));
		_container.Store(new Item("bar", 2));
	}

	public void TestInlineStaticDelegate()
	{
		IList<Item> items = null;
		try
		{
			items = _container.Query<Item>(delegate(Item candidate)
			{
				return candidate.Name == "foo";
			});
		}
		finally
		{
			CheckResult(items);
		}
	}

	public void TestInlineClosureDelegate()
	{	
		string name = "foo";
		IList<Item> items = _container.Query<Item>(delegate(Item candidate)
		{
			return candidate.Name == name;
		});
		CheckResult(items);
	}

    public void TestInlineClosureDelegateWithStaticField()
    {
        /*
        string name = "foo";
        staticValue = 1;
        IList<Item> items = _container.Query<Item>(delegate(Item candidate)
        {
            return candidate.Name == name && staticValue == candidate.Value;
        });
        CheckResult(items);
         */
    }

    public void TestInlineClosureDelegateWithMultipleFields()
    {
        string name = "foo";
        int value = 1;
        IList<Item> items = _container.Query<Item>(delegate(Item candidate)
        {
            return candidate.Name == name && value == candidate.Value;
        });
        CheckResult(items);
    }

	public void TestStaticMemberDelegate()
	{
		IList<Item> items = _container.Query<Item>(DelegateSubject.MatchFoo);
		CheckResult(items);
	}

	public void TestMultipleQueryInvocations()
	{
		CheckResult(_container.Query<Item>(DelegateSubject.MatchFoo));
		CheckResult(_container.Query<Item>(DelegateSubject.MatchFoo));
		CheckResult(_container.Query<Item>(DelegateSubject.MatchFoo));
	}

	delegate IObjectContainer ObjectContainerAccessor();

	public void TestInlineStaticDelegateInsideExpression()
	{
		ObjectContainerAccessor getter = delegate { return _container; };
		CheckResult(getter().Query<Item>(delegate(Item candidate)
		{
			return candidate.Name == "foo";
		}));
	}

	public void TestInstanceMemberDelegate()
	{
		IList<Item> items = _container.Query<Item>(new QueryItemByName("foo").Match);
		CheckResult(items);
	}

    public void TestIntInstanceMemberDelegate()
    {
        IList<Item> items = _container.Query<Item>(new QueryItemByValue(1).Match);
        CheckResult(items);
    }

	private void CheckResult(IList<Item> items)
	{
		Assert.AreEqual(1, items.Count);
		Assert.AreEqual("foo", items[0].Name);
	}

	static bool MatchFoo(Item candidate)
	{
		return candidate.Name == "foo";
	}

	class QueryItemByName
	{
		string _name;

		public QueryItemByName(string name)
		{
			_name = name;
		}

		public bool Match(Item candidate)
		{
			return candidate.Name == _name;
		}

	}

    class QueryItemByValue
    {
        private int _value;

        public QueryItemByValue(int value)
        {
            _value = value;
        }

        public bool Match(Item candidate)
        {
            return candidate.Value == _value;
        }
    }
}
