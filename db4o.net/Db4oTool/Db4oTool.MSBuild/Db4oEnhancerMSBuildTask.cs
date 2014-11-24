/* Copyright (C) 2004 - 2010 Versant Inc.   http://www.db4o.com */
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Db4oTool.MSBuild
{
    public class Db4oEnhancerMSBuildTask : Task
    {
        private ITaskItem[] assemblies;

        [Required]
        public ITaskItem[] Assemblies
        {
            get { return assemblies; }
            set { assemblies = value; }
        }

        private ITaskItem projectDir;

        public ITaskItem ProjectDir
        {
            get { return projectDir; }
            set { projectDir = value; }
        }

        private string commandLine;

        public string CommandLine
        {
            get { return commandLine; }
            set { commandLine = value; }
        }

        public override bool Execute()
        {
			List<string> list = new List<string>();
			list.Add("-v");
			list.Add("-ta");
			if (commandLine != null)
			{
				list.AddRange(commandLine.Split(' '));
			}

			foreach (ITaskItem assembly in assemblies)
			{
				string assemblyFile = projectDir + assembly.ItemSpec;
				Log.LogWarning(string.Format("Enhancing assembly: {0}", assemblyFile));
				list.Add(assemblyFile);
                               
				int ret = Enhance(list.ToArray());
				if (ret != 0)
				{
					Log.LogError(string.Format("Fail to enhance assembly: {0} with return value {1}", assemblyFile, ret));
					return false;
				}
				string message = string.Format("Assembly {0} enhanced successfully .", assemblyFile);
				Log.LogWarning(message);

				list.Remove(assemblyFile);
			}
			return true;
        }

		private int Enhance(string[] options)
		{
			int ret;
			StringWriter consoleOut = new StringWriter();
			StringWriter consoleErr = new StringWriter();
			try
			{
				Console.SetOut(consoleOut);
				Console.SetError(consoleErr);

				ret = Program.Main(options);

				WriteOutput(consoleOut);
				WriteOutput(consoleErr);
			}
			finally
			{
				using (StreamWriter originalOut = new StreamWriter(Console.OpenStandardOutput()))
				{
					originalOut.AutoFlush = true;
					Console.SetOut(originalOut);
				}

				using (StreamWriter originalErr = new StreamWriter(Console.OpenStandardError()))
				{
					originalErr.AutoFlush = true;
					Console.SetError(originalErr);
				}
			}

			return ret;
		}

		private void WriteOutput(StringWriter output)
		{
			string contents = output.ToString();
			if (contents.Length > 0)
			{
				Log.LogWarning(contents);
			}
		}
	}
}
