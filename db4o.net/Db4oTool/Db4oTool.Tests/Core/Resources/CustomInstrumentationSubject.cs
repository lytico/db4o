/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
using System;
using System.IO;
using Db4oUnit;
using Db4oTool.Tests.Core;

public class CustomInstrumentationSubject : ITestCase
{
	static void Foo()
	{
		Bar();
	}

	static void Bar()
	{
	}

	public void TestInstrumentation()
	{
		string stdout = ShellUtilities.WithStdout(Foo);

		string expected = @"
TRACE: System.Void CustomInstrumentationSubject::Foo()
TRACE: System.Void CustomInstrumentationSubject::Bar()
";
		Assert.AreEqual(expected.Trim(), stdout);
	}
}