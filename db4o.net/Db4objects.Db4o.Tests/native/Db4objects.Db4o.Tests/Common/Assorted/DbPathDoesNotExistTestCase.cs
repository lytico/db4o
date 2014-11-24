/* Copyright (C) 2009 Versant Inc.  http://www.db4o.com */
using System.IO;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public partial class DbPathDoesNotExistTestCase
	{
		private string NonExistingFilePath()
		{
#if SILVERLIGHT
			const string tempPath = "silverlightTemp/";
#else
			string tempPath = Path.GetTempPath();
#endif
			return Path.Combine(tempPath, "folderdoesnotexistneverever/filedoesnotexist.db4o");
		}
	}
}
