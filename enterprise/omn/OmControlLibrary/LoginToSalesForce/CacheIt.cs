using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Windows;
using System.Windows.Forms;
using System.Threading;
using System.Net;


namespace OMControlLibrary.LoginToSalesForce
{
	class CacheIt
	{
		private static HttpRuntime _httpRuntime;

		public static Cache Cache
		{
			get
			{
				EnsureHttpRuntime();
				return HttpRuntime.Cache;
			}
		}

		private static void EnsureHttpRuntime()
		{
			try
			{
				Monitor.Enter(typeof(CacheIt));
				if (null == _httpRuntime)
				{
					_httpRuntime = new HttpRuntime();

				}
			}
			finally
			{
				Monitor.Exit(typeof(CacheIt));
			}

		}
	}
}
