/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Db4oTool.Core
{
	public class Configuration
	{
		private bool _caseSensitive;
		private readonly string _assemblyLocation;
		private readonly TraceSwitch _traceSwitch = new TraceSwitch("Db4oTool", "Db4oTool tracing level");
	    private readonly List<ITypeFilter> _filters = new List<ITypeFilter>();
		private bool _preserveDebugInfo;
		
		public Configuration(string assemblyLocation)
		{
			_assemblyLocation = assemblyLocation;
			_traceSwitch.Level = TraceLevel.Warning;
		}

		public bool CaseSensitive
		{
			get { return _caseSensitive; }
			set { _caseSensitive = value; }
		}

		public string AssemblyLocation
		{
			get { return _assemblyLocation; }
		}

		public TraceSwitch TraceSwitch
		{
			get { return _traceSwitch; }
		}

		public bool PreserveDebugInfo
		{
			get { return _preserveDebugInfo; }
			set { _preserveDebugInfo = value; }
		}

		public void AddFilter(ITypeFilter filter)
	    {
            if (null == filter) throw new ArgumentNullException("filter");
	        _filters.Add(filter);
	    }

        public bool Accept(Mono.Cecil.TypeDefinition typedef)
        {
            if (_filters.Count == 0) return true;
            foreach (ITypeFilter filter in _filters)
            {
                if (filter.Accept(typedef)) return true;
            }
            return false;
        }
    }
}
