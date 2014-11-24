/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;

namespace Db4objects.Db4o.Internal
{
	public class Renames
	{
		public static Rename ForField(string className, string name, string newName)
		{
			return new Rename(className, name, newName);
		}

		public static Rename ForClass(string name, string newName)
		{
			return new Rename(string.Empty, name, newName);
		}

		public static Rename ForInverseQBE(Rename ren)
		{
			return new Rename(ren.rClass, null, ren.rFrom);
		}
	}
}
