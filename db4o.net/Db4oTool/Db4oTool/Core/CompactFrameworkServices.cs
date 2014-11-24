/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */

using System;
using Microsoft.Win32;
using Mono.Cecil;

namespace Db4oTool.Core
{
	public class CompactFrameworkServices
	{
		private static readonly byte[] CompactFrameworkPublicKeyToken = { 0x96, 0x9D, 0xB8, 0x05, 0x3D, 0x33, 0x22, 0xAC };

		public static string FolderFor(AssemblyDefinition assembly)
		{
			string cfVersion = TargetDeviceOf(assembly.MainModule);
			return (cfVersion != null) ? InternalCompactFrameworkFolderFor(cfVersion) : null;
		}

		public static string FolderFor(string version)
		{
			string path = InternalCompactFrameworkFolderFor(version);
			if (path == null)
			{
				throw new ArgumentException("Invalid assembly version.", "version");
			}

			return path;
		}

		private static string TargetDeviceOf(ModuleDefinition module)
		{
			foreach (AssemblyNameReference assemblyRef in module.AssemblyReferences)
			{
				if (assemblyRef.PublicKeyToken == null) continue;

				if (AreEqualAndNotNull(CompactFrameworkPublicKeyToken, assemblyRef.PublicKeyToken))
				{
					return String.Format("{0}.{1}.{2}.{3}",
										 assemblyRef.Version.Major,
										 assemblyRef.Version.Minor,
										 assemblyRef.Version.Revision,
										 assemblyRef.Version.Build);
				}
			}

			return null;
		}

		private static bool AreEqualAndNotNull(byte[] lhs, byte[] rhs)
		{
			if (lhs == null || rhs == null) throw new ArgumentException("Arrays cannot be null.");

			if (lhs.Length != rhs.Length) return false;

			for (int i = 0; i < rhs.Length; i++)
			{
				if (rhs[i] != lhs[i]) return false;
			}

			return true;
		}

		private static string InternalCompactFrameworkFolderFor(string cfVersion)
		{
			RegistryKey assemblyFolderExKey = Registry.LocalMachine.OpenSubKey(String.Format(@"SOFTWARE\Microsoft\.NETCompactFramework\v{0}\WindowsCE\AssemblyFoldersEx", cfVersion));
			if (assemblyFolderExKey == null) return null;

			return (string)assemblyFolderExKey.GetValue("");
		}
	}
}
