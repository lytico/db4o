/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
using System;
using System.Collections.Generic;

namespace Db4oTool.Core
{
	public class InstrumentationPipeline
	{
		readonly InstrumentationContext _context;
		readonly List<IAssemblyInstrumentation> _instrumentations = new List<IAssemblyInstrumentation>();
		
		public InstrumentationPipeline(Configuration configuration)
		{
			_context = new InstrumentationContext(configuration);
		}

		public InstrumentationContext Context
		{
			get { return _context; }
		}

		public void Add(IAssemblyInstrumentation instrumentation)
		{
			if (null == instrumentation) throw new ArgumentNullException("instrumentation");
			_instrumentations.Add(instrumentation);
		}

		public void Run()
		{
			foreach (IAssemblyInstrumentation instrumentation in _instrumentations)
			{
				_context.TraceVerbose("Entering '{0}' instrumentation", instrumentation);
				instrumentation.Run(_context);
				_context.TraceVerbose("Leaving '{0}' instrumentation", instrumentation);
			}
		}
	}
}