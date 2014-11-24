/* Copyright (C) 2011 Versant Inc.  http://www.db4o.com */
using System;
using System.IO;
using System.Reflection;

using Db4oTool;
using Db4oTool.Core;
using Db4oTool.Tests.Core;

namespace Db4objects.Db4o.Linq.Instrumentation.Tests
{
	// TODO: integrate into the build
	class Program
	{
		const string TestAssemblyFileName = "Db4objects.Db4o.Linq.Tests.exe";

		static void Main(string[] args)
		{
			var path = ExecutingAssemblyPath();
			CopyToTemp(Path.Combine(path, TestAssemblyFileName));
			CopyAllToTemp(Directory.GetFiles(path, "*.dll"));

			var testAssemblyFile = Path.Combine(GetTempPath(), TestAssemblyFileName);
			InstrumentAssembly(testAssemblyFile);

			var domain = AppDomain.CreateDomain("LinqTests", null, new AppDomainSetup { ApplicationBase = GetTempPath() });
			domain.ExecuteAssembly(testAssemblyFile);
		}

		private static string GetTempPath()
		{
			return ShellUtilities.GetTempPath();
		}

		private static void InstrumentAssembly(string testAssemblyFile)
		{
			var options = new ProgramOptions()
			{
				Target = testAssemblyFile,
				TransparentPersistence = true,
			};

			const string attribute_type = "Db4objects.Db4o.Linq.Tests.CodeAnalysis.DoNotInstrumentAttribute";

			options.Filters.Add(delegate { return new NotFilter(new ByAttributeFilter(attribute_type)); });
			Db4oTool.Program.Run(options);
		}

		private static void CopyAllToTemp(string[] files)
		{
			foreach (var fname in files)
			{
				CopyToTemp(fname);
			}
		}

		private static void CopyToTemp(string fname)
		{
			ShellUtilities.CopyToTemp(fname);
		}

		private static string ExecutingAssemblyPath()
		{
			return Path.GetDirectoryName(Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName);
		}
	}
}
