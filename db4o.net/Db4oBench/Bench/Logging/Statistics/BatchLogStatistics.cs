/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Bench.Logging.Statistics;
using Sharpen.IO;

namespace Db4objects.Db4o.Bench.Logging.Statistics
{
	public class BatchLogStatistics
	{
		public static void Main(string[] args)
		{
			if (args.Length < 1)
			{
				Sharpen.Runtime.Out.WriteLine("[BATCH] No path given.");
				throw new Exception("[BATCH] No path given.");
			}
			new BatchLogStatistics().Run(args[0]);
		}

		public virtual void Run(string logDirectoryPath)
		{
			Sharpen.IO.File directory = new Sharpen.IO.File(logDirectoryPath);
			if (directory.IsDirectory())
			{
				Sharpen.Runtime.Out.WriteLine("[BATCH] Creating statistics for logs in " + logDirectoryPath
					);
				IFilenameFilter logFilter = new LogFilter();
				Sharpen.IO.File[] logFiles = directory.ListFiles(logFilter);
				int i;
				for (i = 0; i < logFiles.Length; i++)
				{
					new LogStatistics().Run(logFiles[i].GetPath());
				}
				Sharpen.Runtime.Out.WriteLine("[BATCH] Statistics for " + i + " log files have been created!"
					);
			}
			else
			{
				Sharpen.Runtime.Out.WriteLine("[BATCH] Given path is no directory!");
				Sharpen.Runtime.Out.WriteLine("[BATCH] Path: " + logDirectoryPath);
			}
		}
	}

	internal class LogFilter : IFilenameFilter
	{
		public virtual bool Accept(Sharpen.IO.File dir, string name)
		{
			return (name.ToLower().EndsWith(".log"));
		}
	}
}
