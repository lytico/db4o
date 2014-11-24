/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
using System;
using Db4oTool.Core;
using Db4oTool.Tests.Core;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Db4oTool.Tests.Core
{
	/// <summary>
	/// Prepends Console.WriteLine("TRACE: " + method) to every method
	/// in the assembly.
	/// </summary>
	public class TraceInstrumentation : AbstractAssemblyInstrumentation
	{
		override protected void ProcessMethod(MethodDefinition method)
		{
			if (!method.HasBody) return;
			
			MethodBody body = method.Body;
			Instruction firstInstruction = body.Instructions[0];
			ILProcessor il = body.GetILProcessor();
			
			// ldstr "TRACE: " + method
			il.InsertBefore(firstInstruction,
			                    il.Create(OpCodes.Ldstr, "TRACE: " + method));
			
			// call Console.WriteLine(string)
			MethodReference Console_WriteLine = Import(typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }));
			il.InsertBefore(firstInstruction,
			                    il.Create(OpCodes.Call, Console_WriteLine));
		}
	}

    class CustomInstrumentationTestCase : SingleResourceTestCase
	{
		protected override string ResourceName
		{
			get { return "CustomInstrumentationSubject"; }
		}

		protected override string CommandLine
		{
			get { return "-instrumentation:Db4oTool.Tests.Core.TraceInstrumentation,Db4oTool.Tests"; }
		}
	}
}
