/* Copyright (C) 2004 - 2006  Versant Inc.   http://www.db4o.com */
using System;
using System.Diagnostics;
using Db4objects.Db4o.Consistency;
using Db4objects.Db4o.Filestats;
using Db4objects.Db4o.Monitoring;
using Db4objects.Db4o.Tools;
using Db4oTool.Core;
using Db4oTool.NQ;
using Db4oTool.TA;

namespace Db4oTool
{
	public class Program
	{
		public static int Main(string[] args)
		{
			Trace.Listeners.Add(new TextWriterTraceListener(Console.Error));

			ProgramOptions options = new ProgramOptions();
			try
			{	
				options.ProcessArgs(args);
				if (!options.IsValid)
				{
					options.DoHelp();
					return -1;
				}

				Run(options);
			}
			catch (Exception x)
			{
				ReportError(options, x);
				return -2;
			}
			return 0;
		}

		public static void Run(ProgramOptions options)
		{
            foreach (string fileName in options.StatisticsFileNames)
            {
                new Statistics().Run(fileName);
            }

			if (options.InstallPerformanceCounters)
			{
				Db4oPerformanceCounters.ReInstall();
			}

            if (options.CheckDatabase)
            {
                Console.Write("\r\nChecking '{0}' : ", options.Target);
                ConsistencyChecker.Main(new string[] { options.Target});
            }

            if (options.ShowFileUsageStats)
            {
                FileUsageStatsCollector.Main(new string[] { options.Target, "true" });
            }

            if (NoInstrumentationStep(options))
            {
                return;
            }

			using (new CurrentDirectoryAssemblyResolver())
			{
				RunPipeline(options);
			}
		}

	    private static bool NoInstrumentationStep(ProgramOptions options)
	    {
	        return !options.NQ && !options.TransparentPersistence && options.CustomInstrumentations.Count == 0;
	    }

	    private static void RunPipeline(ProgramOptions options)
		{
			InstrumentationPipeline pipeline = new InstrumentationPipeline(GetConfiguration(options));
			if (options.NQ)
			{
				pipeline.Add(new DelegateOptimizer());
				pipeline.Add(new PredicateOptimizer());
			}
			
            if (options.TransparentPersistence)
			{
				pipeline.Add(new TAInstrumentation(options.Collections));
			}
			
            foreach (IAssemblyInstrumentation instr in Factory.Instantiate<IAssemblyInstrumentation>(options.CustomInstrumentations))
			{
				pipeline.Add(instr);
			}

			if (!options.Fake)
			{
				pipeline.Add(new SaveAssemblyInstrumentation());
				if (pipeline.Context.IsAssemblySigned())
				{
					pipeline.Context.TraceWarning("Warning: Assembly {0} has been signed; once instrumented it will fail strong name validation (you will need to sign it again).", pipeline.Context.Assembly.Name.Name);
				}
			}
			pipeline.Run();
		}

		private static void ReportError(ProgramOptions options, Exception x)
		{
			if (options.Verbose)
			{
				Console.WriteLine(x);
			}
			else
			{
				Console.WriteLine(x.Message);
			}
		}

		private static Configuration GetConfiguration(ProgramOptions options)
		{
			Configuration configuration = new Configuration(options.Target);
			configuration.CaseSensitive = options.CaseSensitive;
			configuration.PreserveDebugInfo = options.Debug;
			if (options.Verbose)
			{
				configuration.TraceSwitch.Level = options.PrettyVerbose ? TraceLevel.Verbose : TraceLevel.Info;
			}
            foreach (TypeFilterFactory factory in options.Filters)
            {
                configuration.AddFilter(factory());
            }
			return configuration;
		}
	}
}