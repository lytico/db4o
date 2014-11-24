/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */

namespace Db4oTool.Tests.TA
{
	class TACrossAssemblyInstrumentationTestCase : TAInstrumentationTestCaseBase
	{
		protected override string[] Resources
		{
			get
            {
                return new string[]
                {
					"TAInstrumentationSubject",
					"TAAssemblyReferenceSubject",
				};
            }
		}

		protected override string CommandLine
		{
			get
			{
				return "-ta -by-name:FilteredOut -not";
			}
		}
	}
}
