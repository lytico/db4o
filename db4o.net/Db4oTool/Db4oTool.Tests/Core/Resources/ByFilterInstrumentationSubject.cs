using Db4oTool.Tests.Core;
using Db4oUnit;

class NotInstrumented
{
	public static void Bar()
	{
	}
}

class ByFilterInstrumentationSubject : ITestCase
{
	public void Test()
	{
		string stdout = ShellUtilities.WithStdout(NotInstrumented.Bar);

		// our filter doesnt accept any types
		string expected = "";
		Assert.AreEqual(expected.Trim(), stdout);
	}
}
