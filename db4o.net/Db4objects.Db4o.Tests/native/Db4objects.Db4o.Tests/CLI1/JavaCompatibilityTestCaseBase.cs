/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */
using System;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Tests.Util;
using Db4oUnit;
using Db4oUnit.Extensions.Fixtures;
using Db4oUnit.Extensions.Util;

namespace Db4objects.Db4o.Tests.CLI1
{
	public class JavaSnippet
	{
		public readonly string MainClassName;

		public readonly string SourceCode;

		public JavaSnippet(string mainClassName, string sourceCode)
		{
			MainClassName = mainClassName;
			SourceCode = sourceCode;
		}

		public string MainClassFile
		{
			get { return MainClassName.Replace('.', '/') + ".java";  }
		}
	}

	public abstract class JavaCompatibilityTestCaseBase : ITestCase, IOptOutSilverlight
	{
		protected abstract JavaSnippet JavaCode();

		protected abstract string ExpectedJavaOutput();

		protected abstract void PopulateContainer(IObjectContainer container);

		protected virtual IConfiguration GetConfiguration()
		{
			return Db4oFactory.NewConfiguration();
		}

		protected void RunTest()
		{
			if (!JavaServices.CanRunJavaCompatibilityTests())
			{
				return;
			}

			GenerateDataFile();
			CompileJavaSnippet();
			string output = RunJavaSnippet();
			AssertJavaOutput(output);
		}

		private void AssertJavaOutput(string output)
		{
//			Console.WriteLine(output);
			string actual = Normalize(output);
			string expected = Normalize(ExpectedJavaOutput());
			if (Contains(actual, expected)) return;

			Assert.Fail(string.Format("Expecting '{0}' got '{1}'", expected, actual));
		}

		private bool Contains(string s, string what)
		{
			return -1 != s.IndexOf(what);
		}

		private string Normalize(string output)
		{
			return output.Trim().Replace("\r\n", "\n");
		}

		private string RunJavaSnippet()
		{
			return JavaServices.java(JavaCode().MainClassName, DataFilePath());
		}

		private void CompileJavaSnippet()
		{
			JavaServices.ResetJavaTempPath();
			JavaSnippet program = JavaCode();
			string stdout = JavaServices.CompileJavaCode(program.MainClassFile, program.SourceCode);
			Console.WriteLine(stdout);
		}

		private void GenerateDataFile()
		{
			System.IO.File.Delete(DataFilePath());
			using (IObjectContainer container = Db4oFactory.OpenFile(GetConfiguration(), DataFilePath()))
			{
				PopulateContainer(container);
				container.Commit();
			}
		}

		private string DataFilePath()
		{
			return IOServices.BuildTempPath(GetType().Name + ".db4o");
		}
	}
}