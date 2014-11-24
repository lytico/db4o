/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */
using System;
using System.Collections.Generic;
using Mono.Cecil.Cil;

namespace Db4oTool.Core
{
	internal static class InstrumentationUtil
	{
		public static IEnumerable<Instruction> Where(MethodBody body, Predicate<Instruction> predicate)
		{
			for(Instruction instruction = body.Instructions[0]; instruction != null; instruction = instruction.Next)
			{
				if (predicate(instruction))
				{
					yield return instruction;
				}
			}
		}


		public static bool IsCallInstruction(Instruction instruction)
		{
			return instruction.OpCode == OpCodes.Call || instruction.OpCode == OpCodes.Callvirt;
		}

		public static bool IsNewObj(this Instruction instruction)
		{
			return instruction.OpCode == OpCodes.Newobj;
		}

		public static bool IsCall(this Instruction instruction)
		{
			return IsCallInstruction(instruction);
		}
	}
}
