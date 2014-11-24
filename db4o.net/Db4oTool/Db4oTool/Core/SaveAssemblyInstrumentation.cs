/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
namespace Db4oTool.Core
{
	class SaveAssemblyInstrumentation : IAssemblyInstrumentation
	{
		public void Run(InstrumentationContext context)
		{	
			context.SaveAssembly();
		}
	}
}
