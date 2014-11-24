/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
using System;
using Db4oTool.Core;
using Db4oUnit;
using Mono.Cecil;
    
namespace Db4oTool.Tests.TA
{
    class TAInstrumentationAppliedMoreThanOnce : TATestCaseBase
    {
        public void Test()
        {
			AssemblyDefinition testAssembly = GenerateAssembly("TADoubleInstrumentationSubject");
			InstrumentAssembly(testAssembly);

            MethodDefinition instrumented = InstrumentedMethod(testAssembly);
			string before = FormatMethodBody(instrumented);

			InstrumentAssembly(testAssembly);

            string after = FormatMethodBody(instrumented);
            Assert.AreEqual(before, after);
		}

        private static MethodDefinition InstrumentedMethod(AssemblyDefinition testAssembly)
        {
            return CecilReflector.GetMethod(testAssembly.MainModule.GetType("InstrumentedType"), "InstrumentedMethod");
        }

        private static string FormatMethodBody(MethodDefinition instrumented)
        {
            return Cecil.FlowAnalysis.Utilities.Formatter.FormatMethodBody(instrumented);
        }
    }
}
