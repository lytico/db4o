/* Copyright (C) 2004 - 2010 Versant Inc.  http://www.db4o.com */
using System;
using System.IO;
using Db4oTool.Core;
using Db4oUnit;
using Db4oUnit.Fixtures;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Db4oTool.Tests.Core
{
	class DebugInformationTestSuite : FixtureBasedTestSuite
	{
		public override Type[] TestUnits()
		{
			return new Type[] { typeof(DebugInformationTestUnit) };
		}

		public override IFixtureProvider[] FixtureProviders()
		{
			return new IFixtureProvider[]
			       	{
						DebugInformationTestVariables.SourceFixtureProvider,
						DebugInformationTestVariables.DebugSymbolsFixtureProvider
					};
		}
	}

	class DebugInformationTestUnit : ITestCase
	{
		const string ResourceName = "DebugInformationSubject";
		
		public void TestSimpleSourceLine()
		{
			AssertLineInformation("SimpleSourceLine", "o.MethodCall(10)");
		}

		public void TestSimpleIfBoby()
		{
			AssertLineInformation("SimpleIfBody", "o.MethodCall(10)");
		}

		public void TestIfAndElseBranch()
		{
			AssertLineInformation("IfAndElseBranch", "o.MethodCall(10)");
		}

		public void TestElseBranch()
		{
			AssertLineInformation("ElseBranch", "o.MethodCall(10)");
		}

		public void TestAssignmentExpressionAndComparison()
		{
			AssertLineInformation("AssignmentExpressionAndComparison", "if ( (v = o.MethodCall(10)) > 1 ");
		}

		public void TestTryBody()
		{
			AssertLineInformation("TryBody", "o.MethodCall(10)");
		}

		public void TestCatchBody()
		{
			AssertLineInformation("CatchBody", "o.MethodCall(10)");
		}

		private static void AssertLineInformation(string methodName, string expected)
		{
			AssemblyDefinition assembly = Db4oToolTestServices.AssemblyFromResource(ResourceName, typeof(DebugInformationTestSuite), DebugInformationTestVariables.TestWithDebugSymbolsAvailable(), SourceHandler);

			MethodDefinition method = ReflectionServices.FindMethod(assembly, ResourceName, methodName);
			Instruction instruction = ReflectionServices.FindInstruction(method, OpCodes.Callvirt);

			string actual = DebugInformation.InstructionInformationFor(instruction, method.Body.Instructions);

			string fixedExpectation = FixExpectation(expected);
			Assert.IsTrue(actual.Contains(fixedExpectation), string.Format("Expected: {0}, Actual: {1}", fixedExpectation, actual));
		}

		private static string FixExpectation(string expectation)
		{
			if (!DebugInformationTestVariables.TestWithDebugSymbolsAvailable()) return string.Format("{0}", OpCodes.Callvirt);
			return DebugInformationTestVariables.TestWithSourceAvailable()
					? expectation
					: string.Format("{0}.cs", ResourceName);
		}

		private static Action<string> SourceHandler
		{
			get
			{
				Action<string> deleteFile = delegate(string path) { File.Delete(path); };
				Action<string> doNothing  = delegate{ };

				return DebugInformationTestVariables.TestWithSourceAvailable()
				       						? doNothing
				       						: deleteFile;
			}
		}
	}
}
