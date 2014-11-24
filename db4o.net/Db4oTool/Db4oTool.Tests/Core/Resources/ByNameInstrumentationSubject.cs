using Db4oTool.Tests.Core;
using Db4oUnit;

class Foo
{
	public static void Bar()
	{
	}
}

class Fao
{
	public static void Bar()
	{
	}
}

class Foe
{
	public static void Bar()
	{
	}
}

class ByNameInstrumentationSubject : ITestCase
{
	private static void RunBars()
	{
		Foo.Bar();
		Fao.Bar();
		Foe.Bar();
	}

	public void Test()
	{
		string stdout = ShellUtilities.WithStdout(RunBars);

		string expected =
			@"
TRACE: System.Void Foo::Bar()
TRACE: System.Void Fao::Bar()
";
		Assert.AreEqual(expected.Trim(), stdout);
	}
}
