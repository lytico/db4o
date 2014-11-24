/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;

namespace Db4objects.Db4o.Tests
{
	public class AllTests : Db4oTestSuite
	{
		public static int Main(string[] args)
		{
#if CF
			return new AllTests().RunSolo();
//            return new AllTests().RunClientServer();
#else
//			return RunInMemory();
//			return new AllTests().RunSolo();
//			return new AllTests().RunClientServer();
//			return new AllTestsConcurrency().RunConcurrencyAll();
//			return new Jre12.Collections.BigSetTestCase().RunAll();
		    return new AllTests().RunAll();
#endif
		}

		private static int RunInMemory()
		{
			return new ConsoleTestRunner(new Db4oTestSuiteBuilder(new Db4oInMemory(), typeof(AllTests))).Run();
		}
		
		protected override Type[] TestCases()
		{
            return new[]
				{	
					typeof(Linq.Tests.AllTests),
                    typeof(Common.Migration.AllTests),
                    typeof(Common.TA.AllTests),
                    typeof(Common.AllTests),
                    typeof(Jre5.Annotation.AllTests),
                    typeof(Jre5.Collections.Typehandler.AllTests),
                    typeof(CLI1.AllTests),
                    typeof(CLI2.AllTests),
                    typeof(SharpenLang.AllTests),
                    typeof(AllTestsConcurrency),
				};
		}
	}
}
