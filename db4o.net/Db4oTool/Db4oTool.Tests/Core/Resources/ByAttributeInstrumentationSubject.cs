using Db4oTool.Tests.Core;
using Db4oUnit;

class MyCustomAttribute : System.Attribute
{
}

[MyCustomAttribute]
class Instrumented
{
	public static void Foo()
	{
	}
}

class NotInstrumented
{
	public static void Bar()
	{
	}
}

class ByAttributeInstrumentationSubject : ITestCase
{
	private static void RunFooBar()
	{
		Instrumented.Foo();
		NotInstrumented.Bar();
	}

	public void Test()
	{
		string stdout = ShellUtilities.WithStdout(RunFooBar);

		string expected =
			@"
TRACE: System.Void Instrumented::Foo()
";
		Assert.AreEqual(expected.Trim(), stdout);
	}
}
