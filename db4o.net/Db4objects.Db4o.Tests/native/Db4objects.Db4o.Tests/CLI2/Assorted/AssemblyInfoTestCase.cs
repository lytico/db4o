using System;
using System.Reflection;
using Db4oUnit;
using Db4oUnit.Extensions.Fixtures;

namespace Db4objects.Db4o.Tests.CLI2.Assorted
{
	class AssemblyInfoTestCase : ITestCase, IOptOutSilverlight
	{
		public void Test()
		{
			Type[] assemblyReferences = new Type[]
				{
					typeof(Db4oFactory),
#if !SILVERLIGHT
					typeof(Db4objects.Db4o.Instrumentation.Api.ITypeEditor),
					typeof(Db4objects.Db4o.NativeQueries.NQOptimizer),
#endif
				};
			foreach (Type type in assemblyReferences)
			{
				AssemblyName assemblyName = type.Assembly.GetName();
				Assert.AreEqual(ExpectedVersion(), assemblyName.Version, assemblyName.FullName);
				Assert.AreNotEqual(0, assemblyName.GetPublicKeyToken().Length, assemblyName.FullName);
			}
		}

		private static Version ExpectedVersion()
		{
			return new Version(Db4oVersion.Major, Db4oVersion.Minor, Db4oVersion.Iteration, Db4oVersion.Revision);
		}
	}
}
