/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.IO;
using Db4objects.Db4o.Foundation.IO;
using Db4objects.Db4o.Internal;
using Sharpen;

namespace Db4oUnit.Extensions.Util
{
	public class CrossPlatformServices
	{
		public static string SimpleName(string typeName)
		{
			int index = typeName.IndexOf(',');
			if (index < 0)
			{
				return typeName;
			}
			return Sharpen.Runtime.Substring(typeName, 0, index);
		}

		public static string FullyQualifiedName(Type klass)
		{
			return ReflectPlatform.FullyQualifiedName(klass);
		}

		public static string DatabasePath(string fileName)
		{
			string path = Runtime.GetProperty("db4ounit.file.path");
			if (path == null || path.Length == 0)
			{
				path = ".";
			}
			else
			{
				System.IO.Directory.CreateDirectory(path);
			}
			return Path.Combine(path, fileName);
		}
	}
}
