/* Copyright (C) 2009   Versant Inc.   http://www.db4o.com */

using Db4oUnit;
using Db4oTool.Tests.TA; // MockActivator

class CompilerWillGenerateMyConstructor
{
	public object toInitializeThisValue = new object();
}

class CompilerWillGenerateMyConstructorToo : CompilerWillGenerateMyConstructor
{
	public int toInitializeThisValueToo = 0xDB40;
}

class WithConstructorChainAndFieldInitializers
{
	public WithConstructorChainAndFieldInitializers(int v)
	{
		Assert.AreEqual(0x42, initializedField);
		value = v + initializedField;
	}

	public WithConstructorChainAndFieldInitializers() : this(42)
	{
		Assert.AreEqual(0x42 + 42, value);
		value = initializedField;
	}

	public int value;
	private int initializedField = 0x42;
}

class Item
{
	public Item(Item other)
	{
		value = other.value;
		child = other.child;
	}

	public Item(Item other, int value)
	{
		value = value + StaticValue();
		child = other.child;
	}

	public Item(int i)
	{
		value = Constants.Value + i;
	}

	public Item()
	{
		value = Item.StaticValue();
	}

	private static int StaticValue()
	{
		return 42;
	}

	public int value = 0xDB40;
	public Item child;
	public static Item instance = new Item(42);
}

class DerivedItem : Item
{
	public DerivedItem(Item other):base(other)
	{
	}

	public DerivedItem() : base(new Item(), 42)
	{
	}

	public DerivedItem(int i):base(i)
	{
	}

	public int willBeInitializedInConstructor = 0xDB40;
}

class Constants
{
	public static int Value = 42;
}

class ConstructorWithTryCatch : Item
{
	public ConstructorWithTryCatch(Item other, int n) : base(other, n)
	{
		myValue += n;
	}

	public ConstructorWithTryCatch(Item other) : base(other)
	{
		myValue -= 2;
	}

	public ConstructorWithTryCatch() : base(42)
	{
		try
		{
			myValue += value;
		}
		catch
		{
			myValue *= value;
		}
	}

	private int myValue = 0xDB40 + 1;
}

class TAInstrumentedConstructorSubject : ITestCase
{
	public void TestFieldAccessesInConstructors()
	{
		Item template = new Item();
		MockActivator activator = ActivatorFor(template);

		new Item(template);

		Assert.AreEqual(2, activator.ReadCount);
		Assert.AreEqual(0, activator.WriteCount);
	}

	public void TestConstructorChaining()
	{
		Assert.AreEqual(0x42 + 1, new WithConstructorChainAndFieldInitializers(1).value);
		Assert.AreEqual(0x42, new WithConstructorChainAndFieldInitializers().value);
	}

	private MockActivator ActivatorFor(object p)
	{
		return MockActivator.ActivatorFor(p);
	}
}
