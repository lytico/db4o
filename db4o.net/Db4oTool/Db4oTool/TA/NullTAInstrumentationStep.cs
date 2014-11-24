/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */
using Mono.Cecil;

namespace Db4oTool.TA
{
	internal class NullTAInstrumentationStep : TAInstrumentationStepBase
	{
		public static ITAInstrumentationStep Instance = new NullTAInstrumentationStep();

		public override void Process(MethodDefinition method)
		{
			// Do nothing
		}

		private NullTAInstrumentationStep()
		{
		}
	}
}
