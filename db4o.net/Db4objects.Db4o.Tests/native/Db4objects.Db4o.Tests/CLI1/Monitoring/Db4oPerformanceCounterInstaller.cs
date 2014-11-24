/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
#if !CF && !SILVERLIGHT

using System;
using System.Security.Principal;
using System.Threading;
using Db4objects.Db4o.Monitoring;

namespace Db4objects.Db4o.Tests.CLI1.Monitoring
{
	public class Db4oPerformanceCounterInstaller
	{
		private static bool _installed;

		public static void ReInstall()
		{
			if (_installed)
			{
				return;
			}

			if (IsCurrentUserAnAdministrator())
			{
				Db4oPerformanceCounters.ReInstall();
				_installed = true;
			}
		}

		private static bool IsCurrentUserAnAdministrator()
		{
			AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
			WindowsPrincipal principal = (WindowsPrincipal)Thread.CurrentPrincipal;
			return principal.IsInRole(WindowsBuiltInRole.Administrator);
		}
	}
}

#endif