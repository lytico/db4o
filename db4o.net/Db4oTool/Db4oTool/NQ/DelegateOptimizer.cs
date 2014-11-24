/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Db4oTool.NQ
{
	public class DelegateOptimizer : AbstractOptimizer
	{
		private DelegateQueryProcessor _processor;

		override protected void ProcessMethod(MethodDefinition method)
		{
			if (null == method.Body) return;

			// TraceMethodBody(method);

			List<Instruction> instructions = CollectQueryInvocations(method);
			foreach (Instruction instruction in instructions)
			{
				ProcessQueryInvocation(method, instruction);
			}

			//TraceMethodBody(method);
		}

		void ProcessQueryInvocation(MethodDefinition parent, Instruction queryInvocation)
		{
			if (null == _processor) _processor = new DelegateQueryProcessor(_context, this);
			_processedCount++;
			_processor.Process(parent, queryInvocation);
		}

		private List<Instruction> CollectQueryInvocations(MethodDefinition method)
		{
			return new List<Instruction>(EnumerateQueryInvocations(method));
		}

		private IEnumerable<Instruction> EnumerateQueryInvocations(MethodDefinition method)
		{
			foreach (Instruction instruction in method.Body.Instructions)
			{
				if (IsObjectContainerQueryOnPredicateInvocation(instruction))
				{
					yield return instruction;
				}
			}
		}

		private bool IsObjectContainerQueryOnPredicateInvocation(Instruction instruction)
		{
			if (instruction.OpCode.Value != OpCodes.Callvirt.Value) return false;
			GenericInstanceMethod methodRef = instruction.Operand as GenericInstanceMethod;
			if (null == methodRef) return false;
			if (methodRef.Name != "Query") return false;
			if (1 != methodRef.Parameters.Count) return false;
			return IsSystemPredicateInstance(methodRef.Parameters[0].ParameterType);
		}

		private bool IsSystemPredicateInstance(TypeReference type)
		{
			GenericInstanceType genericType = type as GenericInstanceType;
			if (null == genericType) return false;
			return genericType.FullName.StartsWith("System.Predicate");
		}

		protected override string TargetName(int processedCount)
		{
			return processedCount == 1 ? "delegate" : "delegates";
		}
	}
}
