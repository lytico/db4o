/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */
using System;
using System.Reflection;
using Db4objects.Db4o.Ext;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	public partial class FormatMigrationTestCaseBase
	{
		private static string GetTempPath()
		{
#if !CF && !SILVERLIGHT
			return Environment.GetEnvironmentVariable("TEMP");
#else
			return System.IO.Path.GetTempPath();
#endif
		}
	}
}
