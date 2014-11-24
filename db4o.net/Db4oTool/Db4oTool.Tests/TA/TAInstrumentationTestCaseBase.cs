/* Copyright (C) 2009   Versant Inc.   http://www.db4o.com */
using System.Reflection;
using Db4oTool.Tests.Core;

namespace Db4oTool.Tests.TA
{
	abstract class TAInstrumentationTestCaseBase : AbstractCommandLineInstrumentationTestCase
	{
		protected override string CommandLine
		{
			get
			{
				return "-ta";
			}
		}

		protected override abstract string[] Resources { get; }

		protected override Assembly[] Dependencies
		{
			get
			{
				return ArrayServices.Append(base.Dependencies, typeof(Db4objects.Db4o.TA.IActivatable).Assembly);
			}
		}
	}
}
