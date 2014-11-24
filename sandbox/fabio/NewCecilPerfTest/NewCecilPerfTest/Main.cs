using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using Db4objects.Db4o;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Linq;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Diagnostic;
using NewCecilPerfTest.Data;

namespace NewCecilPerfTest
{
	class MainClass
	{
		
		private const int count = 1;

		public static void Main (string[] args)
		{
			for(int i=0;i<1;i++)
			{
				RunTest();
			}
			
			Console.WriteLine("---------------------------------");
			
			DumpEnvInfo();
		}
		
		private static void DumpEnvInfo()
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach(Assembly ass in assemblies)
			{
					AssemblyName assemblyName = ass.GetName();
					Console.WriteLine("{0}: {1}", assemblyName.Name, assemblyName.Version);
			}
		}
		
		public static void RunTest ()
		{
			MemoryStorage storage = new MemoryStorage();
			
			StoreSomething(Prepare(storage)).Close();
			
			/*System.GC.Collect();
			
			for(int i=0;i<count;i++)
			{
				Query(storage);
			}*/

			Stopwatch sw = new Stopwatch();
			
			System.GC.Collect();
			/*sw.Start();
			
			for(int i=0;i<count;i++)
			{
				Prepare(storage).Close();
			}
			
			sw.Stop();
			
			long otherStuff = sw.ElapsedMilliseconds;
			
			sw.Reset();
			System.GC.Collect();*/
			
			sw.Start();
			
			for(int i=0;i<count;i++)
			{
				Query(storage);
			}
			
			sw.Stop();
			
			Console.WriteLine("Elapsed: " + (sw.ElapsedMilliseconds));
		}
		
		private static void Query(IStorage storage)
		{
			IObjectContainer db = Prepare(storage, false);
			
			QueryNQ(db);
			
			db.Close();
		}
		
		class MyDiag : IDiagnosticListener
		{
			public void OnDiagnostic (IDiagnostic d)
			{
				Console.WriteLine("---> " +  d);
			}
		}
		
		private static IObjectContainer Prepare(IStorage storage)
		{
			return Prepare(storage, false);
		}
		
		private static IObjectContainer Prepare(IStorage storage, bool withDiagnostics)
		{
			IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
			config.File.Storage = storage;
			
			if (withDiagnostics)
			{
				config.Common.Diagnostic.AddListener(new MyDiag());
			}
			
			IObjectContainer db = Db4oEmbedded.OpenFile(config, "test.dat");
			
			return db;
		}
		
		private static IEnumerable<Pilot> QueryNQ(IObjectContainer db)
		{
			return db.Query<Pilot>(delegate(Pilot pilot) {
			    return pilot.points == 42;
			});
		}
		
		private static IEnumerable<Pilot> QueryLINQ(IObjectContainer db)
		{
			return from Pilot p in db 
					where p.points == 42
					select p;
		}
		
		private static void PrintAll(IEnumerable<Pilot> pilots)
		{
			foreach(Pilot p in pilots)
			{
				Console.WriteLine("---> "  + p);
			}
		}
		
		private static IObjectContainer StoreSomething(IObjectContainer db)
		{
			Pilot p = new Pilot("Barichello", 42);
			db.Store(p);
			db.Commit();
			return db;
		}
	}
}

