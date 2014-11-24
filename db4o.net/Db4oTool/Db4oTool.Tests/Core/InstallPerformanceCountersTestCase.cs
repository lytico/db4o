/* Copyright (C) 2009   Versant Inc.   http://www.db4o.com */
using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Threading;
using Db4objects.Db4o.Monitoring;
using Db4oUnit;

namespace Db4oTool.Tests.Core
{
	class InstallPerformanceCountersTestCase : ITestCase
	{
		public void Test()
		{
			if (!IsCurrentUserAnAdministrator() && IsLenientPerformanceCounterInstallTest())
			{
				Console.Error.WriteLine("WARNING: {0} requires administrator access rights to run.", GetType());
				return;
			}

			if (Db4oCategoryExists())
			{
				PerformanceCounterCategory.Delete(Db4oPerformanceCounters.CategoryName);
			}

			ProgramOptions options = new ProgramOptions();
			options.InstallPerformanceCounters = true;

			Db4oTool.Program.Run(options);

			Assert.IsTrue(Db4oCategoryExists());
		}

		private static bool IsLenientPerformanceCounterInstallTest()
		{
			return !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("LENIENT_PERFCOUNTER_INSTALL_TEST"));
		}

		private static bool Db4oCategoryExists()
		{
			return PerformanceCounterCategory.Exists(Db4oPerformanceCounters.CategoryName);
		}

		private static bool IsCurrentUserAnAdministrator()
		{
			AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
			WindowsPrincipal principal = (WindowsPrincipal)Thread.CurrentPrincipal;
			return principal.IsInRole(WindowsBuiltInRole.Administrator);
		}
	}
}
