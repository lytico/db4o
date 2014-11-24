/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Internal.Encoding;
using Db4objects.Db4o.Tests.Common.Config;

namespace Db4objects.Db4o.Tests.Common.Config
{
	/// <exclude></exclude>
	public class UnicodeStringEncodingTestCase : StringEncodingTestCaseBase
	{
		protected override Type StringIoClass()
		{
			return typeof(UnicodeStringIO);
		}
	}
}
