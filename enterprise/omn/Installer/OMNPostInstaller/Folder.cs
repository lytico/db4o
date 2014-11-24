/* Copyright (C) 2010 Versant Inc.  http://www.db4o.com */
using System;
using System.IO;

namespace OMNPostInstaller
{
	internal class Folder
	{
		public static readonly string DB4OHome = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "db4objects");
		public static readonly string OMNHome = Path.Combine(DB4OHome, "ObjectManagerEnterprise");

		public static void Delete(string path)
		{
			if (Directory.Exists(path))
			{
				foreach (string dir in Directory.GetDirectories(path))
				{
					Delete(dir);
				}

				foreach (string file in Directory.GetFiles(path))
				{
					File.Delete(file);
				}

				Directory.Delete(path);
			}
		}
	}
}
