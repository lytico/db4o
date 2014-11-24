/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */

using Db4oUnit;

namespace Db4oTool.Tests.Core
{
	public abstract class AbstractCommandLineInstrumentationTestCase : AbstractInstrumentationTestCase
	{
		protected abstract string CommandLine { get; }

		override protected void InstrumentAssembly(string path)
		{
			CheckInstrumentationOutput(InstrumentationServices.InstrumentAssembly(CommandLine, path));
		}

		protected virtual void CheckInstrumentationOutput(ShellUtilities.ProcessOutput output)
		{
			if (output.ExitCode == 0) return;

			Assert.Fail(output.ToString());
		}
	}
}
