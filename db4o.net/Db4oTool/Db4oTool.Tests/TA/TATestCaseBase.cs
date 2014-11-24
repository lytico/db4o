/* Copyright (C) 2004 - 2010 Versant Inc.  http://www.db4o.com */
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Db4oTool.Core;
using Db4oTool.TA;
using Db4oTool.Tests.Core;
using Db4oUnit;
using Mono.Cecil;

namespace Db4oTool.Tests.TA
{
	public abstract class TATestCaseBase : ITestCase
	{
		protected string InstrumentAssembly(AssemblyDefinition testAssembly)
		{
			return InstrumentAssembly(testAssembly, false);
		}

		protected static AssemblyDefinition GenerateAssembly(string resourceName, params Assembly[] references)
		{
			return Db4oToolTestServices.AssemblyFromResource(resourceName, typeof (TATestCaseBase), true, delegate { }, references);
		}

		private string InstrumentAssembly(AssemblyDefinition assembly, IAssemblyInstrumentation instrumentation)
		{
			StringWriter output = new StringWriter();
			Trace.Listeners.Add(new TextWriterTraceListener(output));

			string assemblyFullPath = assembly.MainModule.FullyQualifiedName;
			InstrumentationContext context = new InstrumentationContext(Configuration(assemblyFullPath), assembly);

			instrumentation.Run(context);
			context.SaveAssembly();

			VerifyAssembly(assemblyFullPath);

			return output.ToString();
		}

		protected string InstrumentAssembly(AssemblyDefinition testAssembly, bool instrumentCollections)
		{
			return InstrumentAssembly(testAssembly, new TAInstrumentation(instrumentCollections));
		}

		protected static void VerifyAssembly(string assemblyPath)
		{
			new VerifyAssemblyTest(assemblyPath).Run();
		}

		protected virtual Configuration Configuration(string assemblyLocation)
		{
			Configuration configuration = new Configuration(assemblyLocation);
			configuration.TraceSwitch.Level = TraceLevel.Info;
 
			return configuration;
		}
	}
}
