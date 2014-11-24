/* Copyright (C) 2010 Versant Inc.   http://www.db4o.com */
using Db4oTool.Core;
using Mono.Cecil;

namespace Db4oTool.TA
{
	internal abstract class TAInstrumentationStepBase : ITAInstrumentationStep
	{
		public abstract void Process(MethodDefinition method);

		public InstrumentationContext Context
		{
			get { return _context; }
			set { _context = value; }
		}
		
		private InstrumentationContext _context;
	}
}
