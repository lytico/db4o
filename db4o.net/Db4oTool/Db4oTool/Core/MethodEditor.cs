/* Copyright (C) 2010 Versant Inc.   http://www.db4o.com */
using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Db4oTool.Core
{
	class MethodEditor
	{
		private readonly ILProcessor _il;
		private readonly MethodBody _body;

		public MethodEditor(MethodDefinition method)
		{	
			_body = method.Body;
			_il = method.Body.GetILProcessor ();
		}

		public Instruction Create(OpCode opcode, VariableDefinition variable)
		{
			return _il.Create(opcode, variable);
		}

		public void InsertBefore(Instruction target, Instruction instruction)
		{
			_il.InsertBefore(target, instruction);
			UpdateInstructionReferences(target, instruction);
		}

		private void UpdateInstructionReferences(Instruction oldTarget, Instruction newTarget)
		{
			UpdateInstructionReferences(_body.Instructions, oldTarget, newTarget);
			UpdateInstructionReferences(_body.ExceptionHandlers, oldTarget, newTarget);
		}

		private static void UpdateInstructionReferences(IEnumerable<Instruction> collection, Instruction oldTarget, Instruction newTarget)
		{
			foreach (Instruction instr in collection)
			{
				if (instr.OpCode == OpCodes.Switch)
				{
					Instruction[] labels = (Instruction[])instr.Operand;
					ReplaceAll(labels, oldTarget, newTarget);
				}
				else if (instr.Operand == oldTarget)
				{
					instr.Operand = newTarget;
				}
			}
		}

		private static void UpdateInstructionReferences(IEnumerable<ExceptionHandler> handlers, Instruction oldTarget, Instruction newTarget)
		{
			foreach (ExceptionHandler handler in handlers)
			{
				if (handler.TryEnd == oldTarget)
				{
					handler.TryEnd = newTarget;
				}
				if (handler.TryStart == oldTarget)
				{
					handler.TryStart = newTarget;
				}
				if (handler.HandlerStart == oldTarget)
				{
					handler.HandlerStart = newTarget;
				}
				if (handler.FilterStart == oldTarget)
				{
					handler.FilterStart = newTarget;
				}
			}
		}

		private static void ReplaceAll(Instruction[] labels, Instruction oldTarget, Instruction newTarget)
		{
			for (int i = 0; i < labels.Length; ++i)
			{
				if (labels[i] == oldTarget)
				{
					labels[i] = newTarget;
				}
			}
		}

		internal Instruction Create(OpCode opCode, ParameterDefinition parameterDefinition)
		{
			return _il.Create(opCode, parameterDefinition);
		}

		public Instruction Create(OpCode opCode)
		{
			return _il.Create(opCode);
		}

		internal Instruction Create(OpCode opCode, int value)
		{
			return _il.Create(opCode, value);
		}

		public Instruction Create(OpCode opCode, MethodReference reference)
		{
			return _il.Create(opCode, reference);
		}

		public VariableDefinition AddVariable(TypeReference type)
		{
			_body.InitLocals = true;

			VariableDefinition variable = new VariableDefinition(type);
			_body.Variables.Add(variable);

			return variable;
		}

		static ParameterDefinition GetParameter (MethodBody self, int index)
		{
		    MethodDefinition method = self.Method;

		    if (method.HasThis) {
				if (index == 0)
					return self.ThisParameter;

				index--;
			}

			return method.Parameters [index];
		}

		public static void SimplifyMacros (MethodBody self)
		{
			if (self == null) throw new ArgumentNullException ("self");

			foreach (Instruction instruction in self.Instructions)
			{
				if (instruction.OpCode.OpCodeType != OpCodeType.Macro) continue;

				switch (instruction.OpCode.Code)
				{
					case Code.Ldarg_0:
						ExpandMacro (instruction, OpCodes.Ldarg, GetParameter (self, 0));
						break;
					case Code.Ldarg_1:
						ExpandMacro (instruction, OpCodes.Ldarg, GetParameter (self, 1));
						break;
					case Code.Ldarg_2:
						ExpandMacro (instruction, OpCodes.Ldarg, GetParameter (self, 2));
						break;
					case Code.Ldarg_3:
						ExpandMacro (instruction, OpCodes.Ldarg, GetParameter (self, 3));
						break;
					case Code.Ldloc_0:
						ExpandMacro (instruction, OpCodes.Ldloc, self.Variables [0]);
						break;
					case Code.Ldloc_1:
						ExpandMacro (instruction, OpCodes.Ldloc, self.Variables [1]);
						break;
					case Code.Ldloc_2:
						ExpandMacro (instruction, OpCodes.Ldloc, self.Variables [2]);
						break;
					case Code.Ldloc_3:
						ExpandMacro (instruction, OpCodes.Ldloc, self.Variables [3]);
						break;
					case Code.Stloc_0:
						ExpandMacro (instruction, OpCodes.Stloc, self.Variables [0]);
						break;
					case Code.Stloc_1:
						ExpandMacro (instruction, OpCodes.Stloc, self.Variables [1]);
						break;
					case Code.Stloc_2:
						ExpandMacro (instruction, OpCodes.Stloc, self.Variables [2]);
						break;
					case Code.Stloc_3:
						ExpandMacro (instruction, OpCodes.Stloc, self.Variables [3]);
						break;
					case Code.Ldarg_S:
						instruction.OpCode = OpCodes.Ldarg;
						break;
					case Code.Ldarga_S:
						instruction.OpCode = OpCodes.Ldarga;
						break;
					case Code.Starg_S:
						instruction.OpCode = OpCodes.Starg;
						break;
					case Code.Ldloc_S:
						instruction.OpCode = OpCodes.Ldloc;
						break;
					case Code.Ldloca_S:
						instruction.OpCode = OpCodes.Ldloca;
						break;
					case Code.Stloc_S:
						instruction.OpCode = OpCodes.Stloc;
						break;
					case Code.Ldc_I4_M1:
						ExpandMacro (instruction, OpCodes.Ldc_I4, -1);
						break;
					case Code.Ldc_I4_0:
						ExpandMacro (instruction, OpCodes.Ldc_I4, 0);
						break;
					case Code.Ldc_I4_1:
						ExpandMacro (instruction, OpCodes.Ldc_I4, 1);
						break;
					case Code.Ldc_I4_2:
						ExpandMacro (instruction, OpCodes.Ldc_I4, 2);
						break;
					case Code.Ldc_I4_3:
						ExpandMacro (instruction, OpCodes.Ldc_I4, 3);
						break;
					case Code.Ldc_I4_4:
						ExpandMacro (instruction, OpCodes.Ldc_I4, 4);
						break;
					case Code.Ldc_I4_5:
						ExpandMacro (instruction, OpCodes.Ldc_I4, 5);
						break;
					case Code.Ldc_I4_6:
						ExpandMacro (instruction, OpCodes.Ldc_I4, 6);
						break;
					case Code.Ldc_I4_7:
						ExpandMacro (instruction, OpCodes.Ldc_I4, 7);
						break;
					case Code.Ldc_I4_8:
						ExpandMacro (instruction, OpCodes.Ldc_I4, 8);
						break;
					case Code.Ldc_I4_S:
						ExpandMacro (instruction, OpCodes.Ldc_I4, (int) (sbyte) instruction.Operand);
						break;
					case Code.Br_S:
						instruction.OpCode = OpCodes.Br;
						break;
					case Code.Brfalse_S:
						instruction.OpCode = OpCodes.Brfalse;
						break;
					case Code.Brtrue_S:
						instruction.OpCode = OpCodes.Brtrue;
						break;
					case Code.Beq_S:
						instruction.OpCode = OpCodes.Beq;
						break;
					case Code.Bge_S:
						instruction.OpCode = OpCodes.Bge;
						break;
					case Code.Bgt_S:
						instruction.OpCode = OpCodes.Bgt;
						break;
					case Code.Ble_S:
						instruction.OpCode = OpCodes.Ble;
						break;
					case Code.Blt_S:
						instruction.OpCode = OpCodes.Blt;
						break;
					case Code.Bne_Un_S:
						instruction.OpCode = OpCodes.Bne_Un;
						break;
					case Code.Bge_Un_S:
						instruction.OpCode = OpCodes.Bge_Un;
						break;
					case Code.Bgt_Un_S:
						instruction.OpCode = OpCodes.Bgt_Un;
						break;
					case Code.Ble_Un_S:
						instruction.OpCode = OpCodes.Ble_Un;
						break;
					case Code.Blt_Un_S:
						instruction.OpCode = OpCodes.Blt_Un;
						break;
					case Code.Leave_S:
						instruction.OpCode = OpCodes.Leave;
						break;
				}
			}
		}

		static void ExpandMacro(Instruction instruction, OpCode opcode, object operand)
		{
			instruction.OpCode = opcode;
			instruction.Operand = operand;
		}

		static void MakeMacro(Instruction instruction, OpCode opcode)
		{
			instruction.OpCode = opcode;
			instruction.Operand = null;
		}

		public static void OptimizeMacros(MethodBody self)
		{
			if (self == null) throw new ArgumentNullException ("self");

			MethodDefinition method = self.Method;

			foreach (Instruction instruction in self.Instructions)
			{
				int index;
				switch (instruction.OpCode.Code)
				{
					case Code.Ldarg:
						index = ((ParameterDefinition) instruction.Operand).Index;
						if (index == -1 && instruction.Operand == self.ThisParameter)
							index = 0;
						else if (method.HasThis)
							index++;

						switch (index)
						{
							case 0:
								MakeMacro (instruction, OpCodes.Ldarg_0);
								break;
							case 1:
								MakeMacro (instruction, OpCodes.Ldarg_1);
								break;
							case 2:
								MakeMacro (instruction, OpCodes.Ldarg_2);
								break;
							case 3:
								MakeMacro (instruction, OpCodes.Ldarg_3);
								break;
							default:
								if (index < 256)
									ExpandMacro (instruction, OpCodes.Ldarg_S, instruction.Operand);
								break;
						}
						break;
					case Code.Ldloc:
						index = ((VariableDefinition) instruction.Operand).Index;
						switch (index)
						{
							case 0:
								MakeMacro (instruction, OpCodes.Ldloc_0);
								break;
							case 1:
								MakeMacro (instruction, OpCodes.Ldloc_1);
								break;
							case 2:
								MakeMacro (instruction, OpCodes.Ldloc_2);
								break;
							case 3:
								MakeMacro (instruction, OpCodes.Ldloc_3);
								break;
							default:
								if (index < 256)
									ExpandMacro (instruction, OpCodes.Ldloc_S, instruction.Operand);
								break;
						}
						break;
					case Code.Stloc:
						index = ((VariableDefinition) instruction.Operand).Index;
						switch (index)
						{
							case 0:
								MakeMacro (instruction, OpCodes.Stloc_0);
								break;
							case 1:
								MakeMacro (instruction, OpCodes.Stloc_1);
								break;
							case 2:
								MakeMacro (instruction, OpCodes.Stloc_2);
								break;
							case 3:
								MakeMacro (instruction, OpCodes.Stloc_3);
								break;
							default:
								if (index < 256)
									ExpandMacro (instruction, OpCodes.Stloc_S, instruction.Operand);
								break;
						}
						break;
					case Code.Ldarga:
						index = ((ParameterDefinition) instruction.Operand).Index;
						if (index == -1 && instruction.Operand == self.ThisParameter)
							index = 0;
						else if (method.HasThis)
							index++;
						if (index < 256)
							ExpandMacro (instruction, OpCodes.Ldarga_S, instruction.Operand);
						break;
					case Code.Ldloca:
						if (((VariableDefinition) instruction.Operand).Index < 256)
							ExpandMacro (instruction, OpCodes.Ldloca_S, instruction.Operand);
						break;
					case Code.Ldc_I4:
						int i = (int) instruction.Operand;
						switch (i)
						{
							case -1:
								MakeMacro (instruction, OpCodes.Ldc_I4_M1);
								break;
							case 0:
								MakeMacro (instruction, OpCodes.Ldc_I4_0);
								break;
							case 1:
								MakeMacro (instruction, OpCodes.Ldc_I4_1);
								break;
							case 2:
								MakeMacro (instruction, OpCodes.Ldc_I4_2);
								break;
							case 3:
								MakeMacro (instruction, OpCodes.Ldc_I4_3);
								break;
							case 4:
								MakeMacro (instruction, OpCodes.Ldc_I4_4);
								break;
							case 5:
								MakeMacro (instruction, OpCodes.Ldc_I4_5);
								break;
							case 6:
								MakeMacro (instruction, OpCodes.Ldc_I4_6);
								break;
							case 7:
								MakeMacro (instruction, OpCodes.Ldc_I4_7);
								break;
							case 8:
								MakeMacro (instruction, OpCodes.Ldc_I4_8);
								break;
							default:
								if (i >= -128 && i < 128)
									ExpandMacro (instruction, OpCodes.Ldc_I4_S, (sbyte) i);
								break;
						}
						break;
				}
			}

			OptimizeBranches (self);
		}

		static void OptimizeBranches (MethodBody body)
		{
			ComputeOffsets(body);

			foreach (Instruction instruction in body.Instructions)
			{
				if (instruction.OpCode.OperandType != OperandType.InlineBrTarget) continue;
				if (OptimizeBranch(instruction)) ComputeOffsets(body);
			}
		}

		private static bool OptimizeBranch(Instruction instruction)
		{
			int offset = ((Instruction) instruction.Operand).Offset - (instruction.Offset + instruction.OpCode.Size + 4);
			if (!(offset >= -128 && offset <= 127)) return false;

			switch (instruction.OpCode.Code)
			{
				case Code.Br:
					instruction.OpCode = OpCodes.Br_S;
					break;
				case Code.Brfalse:
					instruction.OpCode = OpCodes.Brfalse_S;
					break;
				case Code.Brtrue:
					instruction.OpCode = OpCodes.Brtrue_S;
					break;
				case Code.Beq:
					instruction.OpCode = OpCodes.Beq_S;
					break;
				case Code.Bge:
					instruction.OpCode = OpCodes.Bge_S;
					break;
				case Code.Bgt:
					instruction.OpCode = OpCodes.Bgt_S;
					break;
				case Code.Ble:
					instruction.OpCode = OpCodes.Ble_S;
					break;
				case Code.Blt:
					instruction.OpCode = OpCodes.Blt_S;
					break;
				case Code.Bne_Un:
					instruction.OpCode = OpCodes.Bne_Un_S;
					break;
				case Code.Bge_Un:
					instruction.OpCode = OpCodes.Bge_Un_S;
					break;
				case Code.Bgt_Un:
					instruction.OpCode = OpCodes.Bgt_Un_S;
					break;
				case Code.Ble_Un:
					instruction.OpCode = OpCodes.Ble_Un_S;
					break;
				case Code.Blt_Un:
					instruction.OpCode = OpCodes.Blt_Un_S;
					break;
				case Code.Leave:
					instruction.OpCode = OpCodes.Leave_S;
					break;
			}

			return true;
		}

		private static void ComputeOffsets(MethodBody body)
		{
			int offset = 0;
			foreach (Instruction instruction in body.Instructions)
			{
				instruction.Offset = offset;
				offset += instruction.GetSize();
			}
		}
	}
}
