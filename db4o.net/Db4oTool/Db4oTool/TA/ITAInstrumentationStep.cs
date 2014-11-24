/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */
using Db4oTool.Core;
using Mono.Cecil;

namespace Db4oTool.TA
{
	interface ITAInstrumentationStep
	{
		void Process(MethodDefinition method);
		InstrumentationContext Context { get; set; }
	}
}
