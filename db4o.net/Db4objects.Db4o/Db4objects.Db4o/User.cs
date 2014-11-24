/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o;

namespace Db4objects.Db4o
{
	/// <exclude></exclude>
	/// <persistent></persistent>
	public class User : IInternal4
	{
		public string name;

		public string password;

		public User()
		{
		}

		public User(string name_, string password_)
		{
			name = name_;
			password = password_;
		}
	}
}
