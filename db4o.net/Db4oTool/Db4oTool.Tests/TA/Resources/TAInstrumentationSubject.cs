/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Db4objects.Db4o.Activation;
using Db4objects.Db4o.TA;
using Db4oTool.Tests.TA; // MockActivator
using Db4oUnit;

public abstract class Named
{
	protected string _name;

	public Named(string name)
	{
		_name = name;
	}

	public string Name
	{
		get { return _name;  }
	}
}

public class ProjectItem : Named
{
	public ProjectItem(string name) : base(name)
	{
	}

	override public bool Equals(object o)
	{
		ProjectItem other = o as ProjectItem;
		if (other == null) return false;

		// foreign field access
		return _name == other._name;
	}

	override public int GetHashCode()
	{
		return _name.GetHashCode();
	}
}

class Project : ProjectItem
{
	public Project(string name) : base(name)
	{
	}

	public void UseByRef()
	{
		ByRef(ref _name);
	}

	private void ByRef(ref string name)
	{
	}
}

struct AValueType
{	
}

[CompilerGenerated]
class CompilerGeneratedType
{
}

class FilteredOut : IActivatable
{
	public string foo;

	public FilteredOut(string foo_)
	{
		foo = foo_;
	}

	[NonSerialized]
	private IActivator _activator;

	public void Bind(IActivator activator)
	{
		_activator = activator;
	}

	public void Activate(ActivationPurpose purpose)
	{
		if (null == _activator) return;
		_activator.Activate(purpose);
	}
}

class CustomActivatable : IActivatable
{
    [NonSerialized]
    private IActivator _activator;

    [NonSerialized]
    public int _mustNotInstrumentMe = 1;
    public int _mustInstrumentAccess = 2;

    public void Bind(IActivator activator)
    {
        if (_activator != null || activator == null) throw new InvalidOperationException();
        _activator = activator;
    }

    public void Activate(ActivationPurpose purpose)
    {
		if (null == _activator) return;
		_activator.Activate(purpose);
    }
}

class VolatileContainer
{
	private volatile int _foo;

	public int Foo
	{
		get { return _foo; }
		set { _foo = value; }
	}
}

class GenericContainer<T>
{
	private int _foo;

	public int Foo
	{
		get { return _foo; }
		set { _foo = value; }
	}
}

class GenericInherited : GenericContainer<int>
{
	private int _bar;

	public int Bar
	{
		get { return _bar; }
		set { _bar = value; }
	}
}

class GenericContainerN<T1, T2, TN>
{
	private int _foo;

	public int Foo
	{
		get { return _foo; }
		set { _foo = value; }
	}
}

class GenericFieldContainer
{
	private int? _foo;

	public int? Foo
	{
		get { return _foo; }
		set { _foo = value; }
	}
}

class GenericFieldOnGenericType<T>
{
	private T _foo;

	public T Foo
	{
		get { return _foo; }
		set { _foo = value; }
	}
}

class MockActivatable : IActivatable
{
	public void Bind(IActivator activator) { }
	public void Activate(ActivationPurpose purpose) { }
}

class Tagged
{
	public string tags;

	public Tagged(string tags_)
	{
		tags = tags_;
	}
}

class GenericMethods
{
	public static string ConstrainedMethod<T>(T value) where T: Tagged
	{
		return value.tags;
	}
}

// Ensures ta instrumentation plays nice with the 'switch' bytecode
class SwitchSubject
{
	private string _field1;
	private string _field2;

	public void Update(int index, string value)
	{
		switch (index)
		{
			case 1:
				_field1 = value;
				break;
			case 2:
				_field2 = value;
				break;
		}
	}

	public string Get(int index)
	{
		switch (index)
		{
			case 1: return _field1;
			case 2: return _field2;
		}
		throw new ArgumentException();
	}
}

class SubjectHoldingArray
{
	private Tagged[] _taggedArray;

	public Tagged[] Items
	{
		get { return _taggedArray; }
		set { _taggedArray = value; }
	} 
}

static class StaticClass
{
	private static string _name;

	public static string Name
	{
		get
		{
			return _name;
		}

		set
		{
			_name = value;
		}
	}
}

class Activity
{
	private List<string> _checkpoints = new List<string>();

	public string[] Checkpoints
	{
		get { return _checkpoints.ToArray(); }
	}

	public void Checkpoint(string label)
	{
		_checkpoints.Add(label);
	}
}

class TryFinallySubject
{
	private Activity _activity;

	public TryFinallySubject(Activity activity)
	{
		_activity = activity;
	}

	public string TryFinally(object obj)
	{
		string result = "";
		_activity.Checkpoint("BeforeTry");
		try
		{
			result = obj.ToString();
			_activity.Checkpoint("Try");
		}
		finally
		{
			_activity.Checkpoint("Finally");
		}
		return result;
	}

	public object TryCatch(object obj)
	{
		object result = null;
		_activity.Checkpoint("BeforeTry");
		try
		{
			result = obj.ToString();
			_activity.Checkpoint("Try");
		}
		catch (Exception x)
		{
			_activity.Checkpoint("Catch");
			result = x;
		}
		finally
		{
			_activity.Checkpoint("Finally");
		}
		return result;
	}
}

class GotoSubject
{
	private Activity _activity;

	public GotoSubject(Activity activity)
	{
		_activity = activity;
	}

	public void Run(int count)
	{
		_activity.Checkpoint("before");
	search:
		_activity.Checkpoint("search");
		for (int i=0; ; ++i)
		{
			if (i < count)
			{
				if (count > 1)
				{
					--count;
					goto search;
				}
				else
					break;
			}
		}
	}
}

class ActivatableInheritedFromNonActivatable : NonActivatableBaseClass
{
	override public string ToString()
	{
		return base.ToString();
	}

	override public bool Equals(object o)
	{
		NonActivatableBaseClass other = o as NonActivatableBaseClass;
		return name == other.name;
	}
}

class TAInstrumentationSubject : ITestCase
{
	public void _TestActivatableInheritedFromNonActivatableCallingSuper()
	{
		ActivatableInheritedFromNonActivatable subject = new ActivatableInheritedFromNonActivatable();
		MockActivator a = ActivatorFor(subject);
		subject.ToString();
		Assert.AreEqual(1, a.ReadCount);
	}

	public void _TestActivatableInheritedFromNonActivatableAccessingSuperField()
	{
		ActivatableInheritedFromNonActivatable subject = new ActivatableInheritedFromNonActivatable();
		MockActivator a = ActivatorFor(subject);
		Assert.IsTrue(subject.Equals(subject));
		Assert.AreEqual(2, a.ReadCount);
	}

	public void TestGoto()
	{
		Activity activity = new Activity();
		GotoSubject subject = new GotoSubject(activity);
		MockActivator a = ActivatorFor(subject);
		subject.Run(2);
		ArrayAssert.AreEqual(new string[] { "before", "search", "search" }, activity.Checkpoints);
		Assert.AreEqual(3, a.ReadCount);
	}

	public void TestTryFinally()
	{
		Activity activity = new Activity();
		TryFinallySubject subject = new TryFinallySubject(activity);
		MockActivator a = ActivatorFor(subject);
		Assert.AreEqual("foo", subject.TryFinally("foo"));
		ArrayAssert.AreEqual(new string[] { "BeforeTry", "Try", "Finally" }, activity.Checkpoints);
		Assert.AreEqual(3, a.ReadCount);
	}

	public void TestTryCatchWithoutException()
	{
		Activity activity = new Activity();
		TryFinallySubject subject = new TryFinallySubject(activity);
		MockActivator a = ActivatorFor(subject);
		Assert.AreEqual("foo", subject.TryCatch("foo"));
		ArrayAssert.AreEqual(new string[] { "BeforeTry", "Try", "Finally" }, activity.Checkpoints);
		Assert.AreEqual(3, a.ReadCount);
	}

	public void TestTryCatchWithException()
	{
		Activity activity = new Activity();
		TryFinallySubject subject = new TryFinallySubject(activity);
		MockActivator a = ActivatorFor(subject);
		Assert.IsInstanceOf(typeof(NullReferenceException), subject.TryCatch(null));
		ArrayAssert.AreEqual(new string[] { "BeforeTry", "Catch", "Finally" }, activity.Checkpoints);
		Assert.AreEqual(3, a.ReadCount);
	}

	public void TestSubjectHoldingArray()
	{
		SubjectHoldingArray subject = new SubjectHoldingArray();
		MockActivator a = ActivatorFor(subject);
		subject.Items = null;
		Assert.AreEqual(1, a.WriteCount);
		Assert.AreEqual(null, subject.Items);
		Assert.AreEqual(1, a.ReadCount);
	}

	public void TestStaticClassIsNotInstrumented()
	{
		Assert.IsFalse(
			typeof(IActivatable).IsAssignableFrom(typeof(StaticClass)),
			"Static classes should not be instrumented.");
	}

	public void TestSwitch()
	{
		SwitchSubject subject = new SwitchSubject();
		MockActivator a = ActivatorFor(subject);
		subject.Update(1, "foo");
		Assert.AreEqual(1, a.WriteCount);
		Assert.AreEqual("foo", subject.Get(1));
		Assert.AreEqual(1, a.ReadCount);

		subject.Update(2, "bar");
		Assert.AreEqual(2, a.WriteCount);
		Assert.AreEqual("bar", subject.Get(2));
		Assert.AreEqual(2, a.ReadCount);
	}

	public void TestConstrainedGenericMethod()
	{
		Tagged obj = new Tagged("foo, bar");
		MockActivator a = ActivatorFor(obj);
		Assert.AreEqual("foo, bar", GenericMethods.ConstrainedMethod(obj));
		Assert.AreEqual(1, a.ReadCount);
	}

	public void TestGenericFieldOnGenericType()
	{
		GenericFieldOnGenericType<int> obj = new GenericFieldOnGenericType<int>();
		MockActivator a = ActivatorFor(obj);
		Assert.AreEqual(0, obj.Foo);
		Assert.AreEqual(1, a.ReadCount);
	}

	public void TestGenericInherited()
	{
		GenericInherited obj = new GenericInherited();
		MockActivator a = ActivatorFor(obj);
		Assert.AreEqual(0, obj.Foo);
		Assert.AreEqual(1, a.ReadCount);
		Assert.AreEqual(0, obj.Bar);
		Assert.AreEqual(2, a.ReadCount);
	}

	public void TestGenericFieldContainer()
	{
		GenericFieldContainer container = new GenericFieldContainer();
		MockActivator a = ActivatorFor(container);
		Assert.IsNull(container.Foo);
		Assert.AreEqual(1, a.ReadCount);
	}

	public void TestGenericInstrumentation()
	{
		GenericContainer<int> container = new GenericContainer<int>();
		MockActivator a = ActivatorFor(container);
		Assert.AreEqual(0, container.Foo);
		Assert.AreEqual(1, a.ReadCount);
	}

	public void TestArbitraryArityGenericInstrumentation()
	{
		GenericContainerN<int, string, int> container = new GenericContainerN<int, string, int>();
		MockActivator a = ActivatorFor(container);
		Assert.AreEqual(0, container.Foo);
		Assert.AreEqual(1, a.ReadCount);
	}

	public void TestVolatileAccess()
	{
		VolatileContainer container = new VolatileContainer();
		MockActivator a = ActivatorFor(container);
		Assert.AreEqual(0, container.Foo);
		Assert.AreEqual(1, a.ReadCount);
	}

	public void TestIsActivatable()
	{
        Assert.IsTrue(IsActivatable(typeof(Named)));
        Assert.IsTrue(IsActivatable(typeof(ProjectItem)));
		Assert.IsTrue(IsActivatable(typeof(Project)));
	}

	public void TestIActivatableImplementClassIsNotInstrumented()
	{
		Assert.AreEqual(0, typeof(MockActivatable).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Length);
	}

	public void TestPropertyGetter()
	{
		Project p = new Project("test");
		MockActivator activator = ActivatorFor(p);

		Assert.AreEqual(0, activator.ReadCount);
		Assert.AreEqual("test", p.Name);
		Assert.AreEqual(1, activator.ReadCount);
	}

	public void TestForeignFieldAccess()
	{
		Project p1 = new Project("test");
		Project p2 = new Project("test");

		MockActivator a1 = ActivatorFor(p1);
		MockActivator a2 = ActivatorFor(p2);

		Assert.IsTrue(p1.Equals(p2));

		Assert.AreEqual(1, a1.ReadCount);
		Assert.AreEqual(1, a2.ReadCount);
	}

	public void TestFilteredOutFieldAccess()
	{
		FilteredOut subject = new FilteredOut("test");
		MockActivator activator = ActivatorFor(subject);

		// TA instrumentation would normally
		// instrument the field access on the
		// next line EXCEPT that FilteredOut
		// was filtered out
		Assert.AreEqual("test", subject.foo);
		Assert.AreEqual(0, activator.ReadCount);
	}

	public void TestFieldByRef()
	{
		Project p1 = new Project("test");
		MockActivator a1 = ActivatorFor(p1);

		p1.UseByRef();

		Assert.AreEqual(1, a1.ReadCount);
	}

	public void TestValueTypesAreNotInstrumented()
	{
		Assert.IsFalse(IsActivatable(typeof(AValueType)));
	}

	public void TestCompilerGeneratedClassesAreNotInstrumented()
	{
		Assert.IsFalse(IsActivatable(typeof(CompilerGeneratedType)));
	}

    public void TestIsTransient()
    {
        CustomActivatable ca = new CustomActivatable();
        MockActivator ma = ActivatorFor(ca);

        Assert.AreEqual(0, ma.ReadCount);
        
        int n = ca._mustNotInstrumentMe;        
        Assert.AreEqual(0, ma.ReadCount);

        n = ca._mustInstrumentAccess;
        Assert.AreEqual(1, ma.ReadCount);
    }

	public void TestBindToNull()
	{
		Tagged obj = new Tagged("test");
		MockActivator ma = ActivatorFor(obj);
		IActivatable activatable = (IActivatable)obj;
		activatable.Bind(null);

		string tags = obj.tags;
		Assert.AreEqual(0, ma.ReadCount);
	}

	public void TestBindSameActivator()
	{
		Tagged obj = new Tagged("test");
		MockActivator ma = ActivatorFor(obj);
		IActivatable activatable = (IActivatable)obj;
		activatable.Bind(ma);

		string tags = obj.tags;
		Assert.AreEqual(1, ma.ReadCount);
	}

	public void TestBindDifferentActivator()
	{
		Tagged obj = new Tagged("test");
		MockActivator ma = ActivatorFor(obj);
		IActivatable activatable = (IActivatable)obj;

		Assert.Expect(
			typeof (InvalidOperationException),
			delegate
				{
					activatable.Bind(ActivatorFor(obj));
				});
	}


	private MockActivator ActivatorFor(object p)
	{
		return MockActivator.ActivatorFor(p);
	}

	private static bool IsActivatable(Type type)
	{
		return typeof(IActivatable).IsAssignableFrom(type);
	}
}