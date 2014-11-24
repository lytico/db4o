/* Copyright (C) 2011 Versant Inc.  http://www.db4o.com */
using Db4oUnit;

namespace Db4oTool.Tests
{
	class ProgramOptionsTestCase : ITestCase
	{	
		public void TestTpSameAsTA()
		{
			AssertTransparentPersistence("-tp");
			AssertTransparentPersistence("-ta");
		}

		public void TestInstallPerformanceCounters()
		{
			ProgramOptions options = new ProgramOptions();
			Assert.IsFalse(options.InstallPerformanceCounters);
			options.ProcessArgs(new string[] { "--install-performance-counters"});
			Assert.IsTrue(options.InstallPerformanceCounters);
		}

        public void TestInvalidOptionsCombination()
        {
            AssertInvalidOptionsCombination("-nq -fileusage mytarget");
            AssertInvalidOptionsCombination("-nq -check mytarget");
            AssertInvalidOptionsCombination("-ta -fileusage mytarget");
            AssertInvalidOptionsCombination("-ta -check mytarget");
            AssertInvalidOptionsCombination("-tp -fileusage mytarget");
        }

	    private void AssertInvalidOptionsCombination(string arguments)
	    {
            ProgramOptions options = new ProgramOptions();
            options.ProcessArgs(arguments.Split(' '));
	        Assert.IsFalse(options.IsValid);
	    }

	    private static void AssertTransparentPersistence(string arg)
		{
			ProgramOptions options = new ProgramOptions();
			Assert.IsFalse(options.TransparentPersistence);
			options.ProcessArgs(new string[] { arg });
			Assert.IsTrue(options.TransparentPersistence);
		}
	}
}
