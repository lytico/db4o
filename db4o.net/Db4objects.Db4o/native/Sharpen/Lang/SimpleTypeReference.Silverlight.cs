/* Copyright (C) 2009   Versant Inc.   http://www.db4o.com */

#if SILVERLIGHT

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Resources;

#endif

namespace Sharpen.Lang
{
	public partial class SimpleTypeReference
	{
#if SILVERLIGHT
		private Assembly ResolveAssemblySilverlight()
		{
			if (!_assemblyCache.Contains(_assemblyName.Name))
			{
				Assembly assembly = IsInUIThread() 
				                    	? LoadAssembly(_assemblyName.Name) 
				                    	: LoadAssemblyInUIThread(_assemblyName.Name);
				
				_assemblyCache[_assemblyName.Name] = assembly;
				return assembly;
			}

			return _assemblyCache[_assemblyName.Name];
		}

		private static bool IsInUIThread()
		{
			return Deployment.Current.CheckAccess();
		}

		private static Assembly LoadAssemblyInUIThread(string assemblyName)
		{
			Assembly assembly = null;
			using (EventWaitHandle wait = new ManualResetEvent(false))
			{
				Deployment.Current.Dispatcher.BeginInvoke(delegate
				                                          	{
				                                          		assembly = LoadAssembly(assemblyName);
				                                          		wait.Set();
				                                          	});

				wait.WaitOne();
			}
			return assembly;
		}

		private static Assembly LoadAssembly(string assemblyName)
		{
			StreamResourceInfo resourceInfo = Application.GetResourceStream(AssemblyUriFor(assemblyName));
			return new AssemblyPart().Load(resourceInfo.Stream);
		}

		private static Uri AssemblyUriFor(string assemblyName)
		{
			return new Uri(assemblyName + ".dll", UriKind.Relative);
		}

		private static readonly AssemblyCache _assemblyCache = new AssemblyCache(typeof(Type).Assembly);

	}

	sealed class AssemblyCache
	{
		public AssemblyCache(params Assembly[] preCachedAssemblies)
		{
			foreach (var assembly in preCachedAssemblies)
			{
				_cache[new AssemblyName(assembly.FullName).Name] = assembly;
			}
		}

		public Assembly this[string assemblyName]
		{
			get { return _cache[assemblyName]; }
			set { _cache[assemblyName] = value; }
		}

		public bool Contains(string assemblyName)
		{
			return _cache.ContainsKey(assemblyName);
		}
		
		private IDictionary<string, Assembly> _cache = new Dictionary<string, Assembly>();
#endif
	}
}
