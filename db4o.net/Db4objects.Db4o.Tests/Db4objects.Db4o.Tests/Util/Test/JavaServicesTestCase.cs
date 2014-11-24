/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Sharpen.Lang;

namespace Db4objects.Db4o.Tests.Util.Test
{
	public class JavaServicesTestCase : ITestCase
	{
		public class ShortProgram
		{
			public static readonly string Output = "XXshortXX";

			public static void Main(string[] arguments)
			{
				Sharpen.Runtime.Out.WriteLine(Output);
			}
		}

		public class LongProgram
		{
			public static readonly string Output = "XXlongXX";

			public static void Main(string[] arguments)
			{
				Sharpen.Runtime.Out.WriteLine(Output);
				try
				{
					Thread.Sleep(long.MaxValue);
				}
				catch (Exception)
				{
				}
			}
		}
	}
}
