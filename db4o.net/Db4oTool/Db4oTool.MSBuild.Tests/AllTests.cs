using System;
using Db4oUnit.Extensions;

namespace Db4oTool.MSBuild.Tests
{
    public class AllTests : Db4oTestSuite
    {
        public static int Main(string[] args)
        {
            return new AllTests().RunAll();
        }

        protected override Type[] TestCases()
        {
            return new Type[] { 
                typeof(IntItemTestCase),
                typeof(CommandlineTestCase),
            };
        }
    }
}
