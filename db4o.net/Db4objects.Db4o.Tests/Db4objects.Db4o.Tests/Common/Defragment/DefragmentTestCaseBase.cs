/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Tests.Common.Api;

namespace Db4objects.Db4o.Tests.Common.Defragment
{
	public class DefragmentTestCaseBase : Db4oTestWithTempFile
	{
		protected virtual string SourceFile()
		{
			return TempFile();
		}

		protected virtual string BackupFile()
		{
			return BackupFileNameFor(TempFile());
		}

		public static string BackupFileNameFor(string file)
		{
			return file + ".backup";
		}
	}
}
