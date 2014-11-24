/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace Db4oTool.Tests.Core
{
	public delegate void Action();

	public class ShellUtilities
	{
		public static string WithStdout(Action code)
		{
			StringWriter writer = new StringWriter();
			TextWriter old = Console.Out;
			try
			{
				Console.SetOut(writer);
				code();
				return writer.ToString().Trim();
			}
			finally
			{
				Console.SetOut(old);
			}
		}

		public static string CopyFileToFolder(string fname, string path)
		{
			string targetFileName = Path.Combine(path, Path.GetFileName(fname));
            Directory.CreateDirectory(path);
			File.Copy(fname, targetFileName, true);
			return targetFileName;
		}

		public class ProcessOutput
		{
			public int ExitCode;
			public string StdOut;
			public string StdErr;

			public ProcessOutput()
			{
			}

			public ProcessOutput(int exitCode, string stdout, string stderr)
			{
				ExitCode = exitCode;
				StdOut = stdout;
				StdErr = stderr;
			}

			public override string ToString()
			{
				return StdOut + StdErr;
			}
		}

		public static ProcessOutput shellm(string fname, params string[] args)
		{
			StringWriter stdout = new System.IO.StringWriter();
			StringWriter stderr = new System.IO.StringWriter();
			TextWriter saved = Console.Out;
			TextWriter savedErr = Console.Error;
			try
			{
				Console.SetOut(stdout);
				Console.SetError(stderr);
				Assembly.LoadFrom(fname).EntryPoint.Invoke(null, new object[] { args });
				return new ProcessOutput(0, stdout.ToString(), stderr.ToString());
			}
			finally
			{
				Console.SetOut(saved);
				Console.SetError(savedErr);
			}
		}

		public static ProcessOutput shell(string fname, params string[] args)
		{
			Process p = StartProcess(fname, args);

			StringWriter stdError = new StringWriter();
			p.ErrorDataReceived += delegate(object sender, DataReceivedEventArgs e)
			{
				stdError.Write(e.Data);
			};
			
			StringWriter stdOut = new StringWriter();
			p.OutputDataReceived += delegate(object sender, DataReceivedEventArgs e)
			{
				stdOut.Write(e.Data);
			};

			p.Start ();

			p.BeginErrorReadLine();
			p.BeginOutputReadLine();

			p.WaitForExit();
			return new ProcessOutput(p.ExitCode, stdOut.ToString(), stdError.ToString());
		}

		static Process StartProcess(string filename, params string[] args)
		{
			Process p = new Process();
			p.StartInfo.CreateNoWindow = true;
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.RedirectStandardOutput = true;
			p.StartInfo.RedirectStandardInput = true;
			p.StartInfo.RedirectStandardError = true;
			p.StartInfo.FileName = filename;
			p.StartInfo.Arguments = string.Join(" ", quote(args));
			return p;
		}

		private static string[] quote(string[] args)
		{
			for (int i = 0; i < args.Length; ++i)
			{
				args[i] = string.Format("\"{0}\"", args[i]);
			}
			return args;
		}

        public static void CopyParentAssemblyToTemp(Type type)
        {
            CopyAssemblyToTemp(type.Assembly);
        }

        public static void CopyAssemblyToTemp(Assembly assembly)
        {
            CopyToTemp(assembly.ManifestModule.FullyQualifiedName);
        }

        public static string CopyToTemp(string fname)
        {
            return ShellUtilities.CopyFileToFolder(fname, GetTempPath());
        }

        public static string GetTempPath()
        {
            //			return Path.GetTempPath();

            // for now, debugging information is only
            // preserved when the directory name does not contain
            // UTF character because of some bug, so
            // let's keep it simple
            string tempPath = Path.Combine(
                Directory.GetDirectoryRoot(Directory.GetCurrentDirectory()),
                "tmp");
            Directory.CreateDirectory(tempPath);
            return tempPath;
        }

        public static void DeleteFile(string fname)
        {
            if (File.Exists(fname)) File.Delete(fname);
        }
	}
}
