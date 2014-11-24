/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;

namespace Db4objects.Db4o
{
	/// <summary>
	/// Renaming actions are stored to the database file to make
	/// sure that they are only performed once.
	/// </summary>
	/// <remarks>
	/// Renaming actions are stored to the database file to make
	/// sure that they are only performed once.
	/// </remarks>
	/// <exclude></exclude>
	/// <persistent></persistent>
	public sealed class Rename : IInternal4
	{
		public string rClass;

		public string rFrom;

		public string rTo;

		public Rename()
		{
		}

		public Rename(string aClass, string aFrom, string aTo)
		{
			rClass = aClass;
			rFrom = aFrom;
			rTo = aTo;
		}

		public bool IsField()
		{
			return rClass.Length != 0;
		}
	}
}
