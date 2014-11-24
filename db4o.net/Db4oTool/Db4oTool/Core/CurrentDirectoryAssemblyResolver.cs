using System;

namespace Db4oTool.Core
{
	public class CurrentDirectoryAssemblyResolver : DirectoryAssemblyResolver
	{
		public CurrentDirectoryAssemblyResolver()
			: base(Environment.CurrentDirectory)
		{
		}
	}
}
