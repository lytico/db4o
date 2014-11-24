/* Copyright (C) 2004 - 2006  Versant Inc.   http://www.db4o.com */
using System;
using System.Reflection;
using Db4oTool.Core;
using Db4objects.Db4o.Internal.Query;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Db4oTool.NQ
{
	class QueryInvocationProcessor
	{
		private InstrumentationContext _context;

		private MethodReference _NativeQueryHandler_ExecuteInstrumentedDelegateQuery;
		private MethodReference _NativeQueryHandler_ExecuteInstrumentedStaticDelegateQuery;

		private ILPattern _staticFieldPattern = CreateStaticFieldPattern();

		private ILPattern _predicateCreationPattern = ILPattern.Sequence(OpCodes.Newobj, OpCodes.Ldftn);

		public QueryInvocationProcessor(InstrumentationContext context)
		{
			_context = context;
			_NativeQueryHandler_ExecuteInstrumentedDelegateQuery = context.Import(typeof(NativeQueryHandler).GetMethod("ExecuteInstrumentedDelegateQuery", BindingFlags.Public | BindingFlags.Static));
			_NativeQueryHandler_ExecuteInstrumentedStaticDelegateQuery = context.Import(typeof(NativeQueryHandler).GetMethod("ExecuteInstrumentedStaticDelegateQuery", BindingFlags.Public | BindingFlags.Static));
		}

		public void Process(MethodDefinition parent, Instruction queryInvocation)
		{
			ILProcessor il = parent.Body.GetILProcessor ();
			if (IsCachedStaticFieldPattern(queryInvocation))
			{	
				_context.TraceVerbose("static delegate field pattern found in {0}", parent.Name);
				ProcessCachedStaticFieldPattern(il, queryInvocation);
			}
			else if (IsPredicateCreationPattern(queryInvocation))
			{
				_context.TraceVerbose("simple delegate pattern found in {0}", parent.Name);
				ProcessPredicateCreationPattern(il, queryInvocation);
			}
			else
			{
				_context.TraceWarning("Unknown query invocation pattern on method: {0}!", parent);
			}
		}

		private void ProcessPredicateCreationPattern(ILProcessor il, Instruction queryInvocation)
		{
			MethodReference predicateMethod = GetMethodReferenceFromInlinePredicatePattern(queryInvocation);

			Instruction ldftn = GetNthPrevious(queryInvocation, 2);
			il.InsertBefore(ldftn, il.Create(OpCodes.Dup));

			il.InsertBefore(queryInvocation, il.Create(OpCodes.Ldtoken, predicateMethod));

			// At this point the stack is like this:
			//     runtime method handle, delegate reference, target object, ObjectContainer
			il.Replace(queryInvocation,
			               il.Create(OpCodes.Call,
			                             InstantiateGenericMethod(
			                             	_NativeQueryHandler_ExecuteInstrumentedDelegateQuery,
			                             	GetQueryCallExtent(queryInvocation))));
		}

		private void ProcessCachedStaticFieldPattern(ILProcessor il, Instruction queryInvocation)
		{
			MethodReference predicateMethod = GetMethodReferenceFromStaticFieldPattern(queryInvocation);
			il.InsertBefore(queryInvocation, il.Create(OpCodes.Ldtoken, predicateMethod));

			// At this point the stack is like this:
			//     runtime method handle, delegate reference, ObjectContainer
			
			il.Replace(queryInvocation,
			               il.Create(OpCodes.Call,
			                             InstantiateGenericMethod(
			                             	_NativeQueryHandler_ExecuteInstrumentedStaticDelegateQuery,
			                             	GetQueryCallExtent(queryInvocation))));
		}

		private MethodReference GetMethodReferenceFromInlinePredicatePattern(Instruction queryInvocation)
		{
			return (MethodReference)GetNthPrevious(queryInvocation, 2).Operand;
		}

		private bool IsPredicateCreationPattern(Instruction queryInvocation)
		{
			return _predicateCreationPattern.IsBackwardsMatch(queryInvocation);
		}

		private MethodReference InstantiateGenericMethod(MethodReference methodReference, TypeReference extent)
		{
			GenericInstanceMethod instance = new GenericInstanceMethod(methodReference);
			instance.GenericArguments.Add(extent);
			return instance;
		}

		private TypeReference GetQueryCallExtent(Instruction queryInvocation)
		{
			GenericInstanceMethod method = (GenericInstanceMethod)queryInvocation.Operand;
			return method.GenericArguments[0];
		}

		private MethodReference GetMethodReferenceFromStaticFieldPattern(Instruction instr)
		{
			return (MethodReference)GetFirstPrevious(instr, OpCodes.Ldftn).Operand;
		}

		private Instruction GetFirstPrevious(Instruction instr, OpCode opcode)
		{
			Instruction previous = instr;
			while (previous != null)
			{
				if (previous.OpCode == opcode) return previous;
				previous = previous.Previous;
			}
			throw new ArgumentException("No previous " + opcode + " instruction found");
		}

		private Instruction GetNthPrevious(Instruction instr, int n)
		{
			Instruction previous = instr;
			for (int i = 0; i < n; ++i)
			{
				previous = previous.Previous;
			}
			return previous;
		}
		
		private static ILPattern CreateStaticFieldPattern()
		{
			// ldsfld (br_s)? stsfld newobj ldftn ldnull (brtrue_s | brtrue) ldsfld
			return ILPattern.Sequence(
				ILPattern.Instruction(OpCodes.Ldsfld),
				ILPattern.Optional(OpCodes.Br_S),
				ILPattern.Instruction(OpCodes.Stsfld),
				ILPattern.Instruction(OpCodes.Newobj),
				ILPattern.Instruction(OpCodes.Ldftn),
				ILPattern.Instruction(OpCodes.Ldnull),
				ILPattern.Alternation(OpCodes.Brtrue, OpCodes.Brtrue_S),
				ILPattern.Instruction(OpCodes.Ldsfld));
		}

		private bool IsCachedStaticFieldPattern(Instruction instr)
		{
			return _staticFieldPattern.IsBackwardsMatch(instr);
		}
	}
}