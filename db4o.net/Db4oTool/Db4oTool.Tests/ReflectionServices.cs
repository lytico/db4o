/* Copyright (C) 2004 - 2010 Versant Inc.  http://www.db4o.com */
using Db4oTool.Core;
using Db4oUnit;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Db4oTool.Tests
{
	class ReflectionServices
	{
		public static Instruction FindInstruction(AssemblyDefinition assembly, string typeName, string testMethodName, OpCode testInstruction)
		{
			MethodDefinition testMethod = FindMethod(assembly, typeName, testMethodName);
			Assert.IsNotNull(testMethod);

			return FindInstruction(testMethod, testInstruction);
		}

		public static Instruction FindInstruction(MethodDefinition method, OpCode opCode)
		{
			Instruction current = method.Body.Instructions[0];

			Instruction instruction = current;
			while (instruction != null && instruction.OpCode != opCode)
			{
				instruction = instruction.Next;
			}

			Assert.IsNotNull(instruction);
			Assert.AreEqual(opCode, instruction.OpCode);
			current = instruction;
			return current;
		}

		public static MethodDefinition FindMethod(AssemblyDefinition assembly, string typeName, string methodName)
		{
			TypeDefinition testType = assembly.MainModule.GetType(typeName);
			return CecilReflector.GetMethod(testType, methodName);
		}
	}
}
