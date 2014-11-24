/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.IO;
using Db4objects.Db4o.Foundation.IO;

namespace Db4oUnit.Extensions.Util
{
	/// <exclude></exclude>
	public class IOUtil
	{
		/// <summary>Deletes the directory</summary>
		/// <exception cref="System.IO.IOException"></exception>
		public static void DeleteDir(string dir)
		{
			string absolutePath = new Sharpen.IO.File(dir).GetCanonicalPath();
			Sharpen.IO.File fDir = new Sharpen.IO.File(dir);
			if (fDir.IsDirectory())
			{
				string[] files = fDir.List();
				for (int i = 0; i < files.Length; i++)
				{
					DeleteDir(Path.Combine(absolutePath, files[i]));
				}
			}
			File4.Delete(dir);
		}
	}
}
