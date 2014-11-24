using System;
using Db4objects.Db4o.TA;
using Db4oUnit;

unsafe public class PointerContainer
{
	public int* foo;
}

public delegate void FooHandler();

public class DelegateContainer
{
	public FooHandler foo;

	public EventHandler<EventArgs> bar;

	public EventHandler baz;
}

public class IDoHaveSerializableFields : DelegateContainer
{
	public int fooBar;
}

public class BaseClassWithSerializableField
{
	public int foo;
}

public class DerivedClass : BaseClassWithSerializableField
{
	public EventHandler<EventArgs> bar;
}

public class TAUnsafeInstrumentationSubject : ITestCase
{
	public void TestDelegateIsNotInstrumented()
	{
		Assert.IsFalse(typeof(IActivatable).IsAssignableFrom(typeof(FooHandler)));
	}

	public void TestClassWithoutPersistentFieldsAreNotInstrumented()
	{
		Assert.IsFalse(typeof(IActivatable).IsAssignableFrom(typeof(DelegateContainer)));
		Assert.IsFalse(typeof(IActivatable).IsAssignableFrom(typeof(PointerContainer)));
	}

	public void TestOneSerializableFieldTriggersInstrumentation()
	{
		Assert.IsTrue(typeof(IActivatable).IsAssignableFrom(typeof(IDoHaveSerializableFields)));
		Assert.IsTrue(typeof(IActivatable).IsAssignableFrom(typeof(DerivedClass)));
	}
}

