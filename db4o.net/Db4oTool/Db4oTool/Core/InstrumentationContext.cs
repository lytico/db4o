/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
using System;
using System.Diagnostics;
using System.Reflection;
using Mono.Cecil;

namespace Db4oTool.Core
{
	public class InstrumentationContext
	{
		private AssemblyDefinition _assembly;
		private Configuration _configuration;
		private string _alternateAssemblyLocation;

        public InstrumentationContext(Configuration configuration, AssemblyDefinition assembly)
        {
			Init(configuration, assembly);
		}

		public InstrumentationContext(Configuration configuration)
		{
			Init(configuration, LoadAssembly(configuration));
		}

		public string AlternateAssemblyLocation
		{
			get { return _alternateAssemblyLocation; }
		}

		public Configuration Configuration
		{
			get { return _configuration; }
		}

		public TraceSwitch TraceSwitch
		{
			get { return _configuration.TraceSwitch; }
		}

		public AssemblyDefinition Assembly
		{
			get { return _assembly; }
		}

		public string AssemblyLocation
		{
			get { return _assembly.MainModule.FullyQualifiedName; }
		}

		public TypeReference Import(Type type)
		{
			return _assembly.MainModule.Import(type);
		}

		public MethodReference Import(MethodBase method)
		{
			return _assembly.MainModule.Import(method);
		}

		public void SaveAssembly()
		{
			WriterParameters parameters = WriterParametersFor(PreserveDebugInfo());
		    _assembly.Write(AssemblyLocation, parameters);
		}

	    private WriterParameters WriterParametersFor(bool preserveDebugInfo)
	    {
	        WriterParameters parameters = new WriterParameters();
	        parameters.WriteSymbols = preserveDebugInfo;
	        return parameters;
	    }

	    private bool PreserveDebugInfo()
		{
			return _configuration.PreserveDebugInfo;
		}

		public void TraceWarning(string message, params object[] args)
		{
			if (TraceSwitch.TraceWarning)
			{
				Trace.WriteLine(string.Format(message, args));
			}
		}

		public void TraceInfo(string message, params object[] args)
		{
			if (TraceSwitch.TraceInfo)
			{
				Trace.WriteLine(string.Format(message, args));
			}
		}

		public void TraceVerbose(string format, params object[] args)
		{
			if (TraceSwitch.TraceVerbose)
			{
				Trace.WriteLine(string.Format(format, args));
			}
		}

		public bool Accept(TypeDefinition typedef)
		{
			return _configuration.Accept(typedef);
		}

		public bool IsAssemblySigned()
		{
			return _assembly.Name.HasPublicKey;
		}

		private void Init(Configuration configuration, AssemblyDefinition assembly)
		{
			_configuration = configuration;

			ConfigureCompactFrameworkAssemblyPath(assembly);
			SetupAssembly(assembly);
		}

		private void ConfigureCompactFrameworkAssemblyPath(AssemblyDefinition assembly)
		{
			_alternateAssemblyLocation = CompactFrameworkServices.FolderFor(assembly);
		}

		private static AssemblyDefinition LoadAssembly(Configuration configuration)
		{
			return AssemblyDefinition.ReadAssembly(configuration.AssemblyLocation);
		}

		private void SetupAssembly(AssemblyDefinition assembly)
		{
			_assembly = assembly;

			if (PreserveDebugInfo())
			{
				_assembly.MainModule.ReadSymbols();
			}
		}
	}
}