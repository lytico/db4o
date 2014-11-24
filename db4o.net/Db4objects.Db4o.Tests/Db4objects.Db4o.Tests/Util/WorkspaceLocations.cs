/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Tests.Util;

namespace Db4objects.Db4o.Tests.Util
{
	/// <exclude></exclude>
	public class WorkspaceLocations
	{
		private static string _testFolder = null;

		public static string GetTestFolder()
		{
			if (_testFolder == null)
			{
				_testFolder = WorkspaceServices.WorkspacePath("db4oj.tests/test");
			}
			return _testFolder;
		}
	}
}
