using System;
using System.Reflection;
using System.IO;

namespace Db4oTool.Core
{
	public class DirectoryAssemblyResolver : IDisposable
	{
		private readonly string _directory;

		public DirectoryAssemblyResolver(string directory)
		{
			_directory = directory;
			CurrentDomain().AssemblyResolve += AppDomain_AssemblyResolve;
		}

		Assembly AppDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			string baseName = Path.Combine(_directory, SimpleName(args.Name));
			Assembly found = ProbeFile(baseName + ".dll");
			if (found != null) return found;
			return ProbeFile(baseName + ".exe");
		}

		private string SimpleName(string assemblyName)
		{
			return assemblyName.Split(',')[0];
		}

		private Assembly ProbeFile(string fname)
		{
			if (!File.Exists(fname)) return null;
			return Assembly.LoadFile(fname);
		}

		public void Dispose()
		{
			CurrentDomain().AssemblyResolve -= AppDomain_AssemblyResolve;
		}

		private static AppDomain CurrentDomain()
		{
			return AppDomain.CurrentDomain;
		}
	}
}
