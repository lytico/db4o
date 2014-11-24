/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
using Mono.Cecil.Cil;

namespace Db4oTool.Core
{
	public abstract class ILPattern
	{
		public static ILPattern Sequence(params ILPattern[] sequence)
		{
			return new SequencePattern(sequence);
		}
		
		public static ILPattern Sequence(params OpCode[] opcodes)
		{
			ILPattern[] patterns = new ILPattern[opcodes.Length];
			for (int i=0; i<opcodes.Length; ++i)
			{
				patterns[i] = Instruction(opcodes[i]);
			}
			return new SequencePattern(patterns);
		}

		private class SequencePattern : ILPattern
		{
			private ILPattern[] _sequence;

			public SequencePattern(ILPattern[] sequence)
			{
				_sequence = sequence;
			}

			internal override void BackwardsMatch(MatchContext context)
			{
				foreach (ILPattern pattern in _sequence)
				{
					pattern.BackwardsMatch(context);
					if (!context.Success || context.Instruction == null) break;
				}
			}
		}

		public static ILPattern Instruction(OpCode code)
		{
			return new InstructionPattern(code);
		}

		private class InstructionPattern : ILPattern
		{
			private OpCode _code;

			public InstructionPattern(OpCode code)
			{
				_code = code;
			}

			internal override void BackwardsMatch(MatchContext context)
			{
				if (context.Instruction == null)
				{
					context.Success = false;
					return;
				}
				context.Success = context.Instruction.OpCode == _code;
				context.MoveBackwards();
			}
		}

		public static ILPattern Optional(OpCode code)
		{
			return new OptionalPattern(Instruction(code));
		}

		private class OptionalPattern : ILPattern
		{
			private ILPattern _pattern;

			public OptionalPattern(ILPattern pattern)
			{
				_pattern = pattern;
			}

			internal override void BackwardsMatch(MatchContext context)
			{
				_pattern.TryBackwardsMatch(context);
			}
		}

		public static ILPattern Alternation(OpCode a, OpCode b)
		{
			return new AlternationPattern(Instruction(a), Instruction(b));
		}

		private class AlternationPattern : ILPattern
		{
			private ILPattern _a;
			private ILPattern _b;

			public AlternationPattern(ILPattern a, ILPattern b)
			{
				_a = a;
				_b = b;
			}

			internal override void BackwardsMatch(MatchContext context)
			{
				if (!_a.TryBackwardsMatch(context))
				{
					_b.BackwardsMatch(context);
				}
			}
		}
		
		public class MatchContext
		{
			public bool Success;
			public Instruction Instruction;
			
			public MatchContext(Instruction instruction)
			{
				Reset(instruction);
			}

			public void Reset(Instruction instruction)
			{
				Success = true;
				Instruction = instruction;
			}

			public void MoveBackwards()
			{
				this.Instruction = this.Instruction.Previous;
			}
		}

		public bool IsBackwardsMatch(Instruction instruction)
		{	
			return BackwardsMatch(instruction).Success;
		}

		public MatchContext BackwardsMatch(Instruction instruction)
		{
			MatchContext context = new MatchContext(instruction.Previous);
			BackwardsMatch(context);
			return context;
		}

		internal abstract void BackwardsMatch(MatchContext context);

		internal bool TryBackwardsMatch(MatchContext context)
		{
			Instruction checkpoint = context.Instruction;
			BackwardsMatch(context);
			if (context.Success) return true;
			
			context.Reset(checkpoint);
			return false;
		}
	}
}