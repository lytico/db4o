/* Copyright (C) 2010 Versant Inc.   http://www.db4o.com */
using System;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Db4oTool.Core
{
	public struct StackAnalysisResult
	{
		public StackAnalysisResult(Instruction consumer, int offset, bool match, int stackHeight)
		{
			_consumer = consumer;
			_offset = offset;
			_match = match;
			_stackHeight = stackHeight;
		}

		public Instruction Consumer
		{
			get { return _consumer; }
		}

		/// <summary>
		/// # of instructions in between the starting point passed to <see cref="StackAnalyzer.IsConsumedBy"/> and
		/// the instruction that consumed that stack position (<see cref="Consumer"/>).
		/// </summary>
		public int Offset
		{
			get { return _offset; }
		}

		public bool Match
		{
			get { return _match; }
		}

		public int StackHeight
		{
			get { return _stackHeight; }
		}

		public ParameterDefinition AssignedParameter()
		{
			MethodReference callee = _consumer.Operand as MethodReference;
			if (callee == null)
			{
				throw new InvalidOperationException();	
			}

			return callee.Parameters[callee.Parameters.Count - _offset];
		}

		private readonly Instruction _consumer;
		private readonly int _offset;
		private readonly bool _match;
		private readonly int _stackHeight;
	}
}
