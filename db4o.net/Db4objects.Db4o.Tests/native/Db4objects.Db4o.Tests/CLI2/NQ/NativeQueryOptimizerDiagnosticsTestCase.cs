using System;
using System.Collections.Generic;
using System.IO;
using Db4objects.Db4o.Tests.Util;
using Db4oUnit;
using Db4oUnit.Extensions.Fixtures;

namespace Db4objects.Db4o.Tests.NativeQueries.Diagnostics
{
    public partial class NativeQueryOptimizerDiagnosticsTestCase : IOptOutSilverlight
	{
#if !CF && !SILVERLIGHT
        public void TesteInlineClosureComplexQueryFails()
        {
            _failed = false;
            IList<Subject> items = Db().Query<Subject>(delegate(Subject sub) { return sub.ComplexName().StartsWith("Test"); });
            Assert.IsTrue(_failed);
        }

        public void TestNQOptimizerLoadFailure()
        {
            string code = @"
using System;
using System.IO;

using Db4objects.Db4o.Foundation.IO;
using Db4objects.Db4o.Diagnostic;
using Db4objects.Db4o.Config;
using Db4objects.Db4o;
using Db4objects.Db4o.Tests.NativeQueries.Diagnostics;

public class Item {

}

public class TestNQOptimizerLoadFailure : MarshalByRefObject, INQTestRunner
{
    public bool Run()
    {
        IConfiguration config = Db4oFactory.NewConfiguration();
        ListenForNQOptimizerLoadFailures listener = new ListenForNQOptimizerLoadFailures();
        config.Diagnostic().AddListener(listener);

		string databaseFile = Path.GetTempFileName();
        using(IObjectContainer db = Db4oFactory.OpenFile(config, databaseFile))
        {
            db.Store(new Item());
            db.Query<Object>(delegate { return true; });
        }

		File4.Delete(databaseFile);

        return listener.UnableToLoadNQOptimizer;
    }
}

internal class ListenForNQOptimizerLoadFailures : IDiagnosticListener
{
    private bool unableToLoadNQOptimizer;

    public void OnDiagnostic(IDiagnostic d)
    {
        if (d is NativeQueryOptimizerNotLoaded)
        {
            unableToLoadNQOptimizer = true;
        }
    }

    public bool UnableToLoadNQOptimizer
    {
        get { return unableToLoadNQOptimizer; }
    }
}";
            
            string typeName = "TestNQOptimizerLoadFailure";
            AppDomain appDomain = PrepareTestEnvironment(BuildAssemblyFileName(typeName), code);
            try
            {
                INQTestRunner test = (INQTestRunner)appDomain.CreateInstanceAndUnwrap(BuildAssemblyFileName(typeName), typeName);
                Assert.IsTrue(test.Run());
            }
            finally
            {
                if (appDomain != null)
                {
                    AppDomain.Unload(appDomain);
                }
            }
        }

        private static string BuildAssemblyFileName(string typeName)
        {
            return typeName + "_assembly";
        }

        private static AppDomain PrepareTestEnvironment(string assemblyFileName, string code)
        {
            AppDomain targetAppDomain = CreateAppDomain();
            string assemblyPath = Path.Combine(targetAppDomain.BaseDirectory, assemblyFileName + ".dll");
            CompilationServices.EmitAssembly(assemblyPath, code);

            return targetAppDomain;
        }

        private static AppDomain CreateAppDomain()
        {
            AppDomainSetup appDomainSetup = new AppDomainSetup();
            appDomainSetup.ApplicationBase = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Db4oNQTest");
            AppDomain newAppDomain = AppDomain.CreateDomain("Db4oNQTest", null, appDomainSetup);

            CopyAssembliesTo(newAppDomain, "Db4oUnit.Extensions.dll", "Db4objects.Db4o.dll", "Db4objects.Db4o.Tests.exe", "Db4oUnit.dll");

            return newAppDomain;
        }

        private static void CopyAssembliesTo(AppDomain domain, params string[] toBeCopied)
        {
            string targetPath = CreateDirectoryIfNecessary(domain.BaseDirectory);
            foreach(string file in toBeCopied)
            {
                File.Copy(
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory,file), 
                    Path.Combine(targetPath, file));
            }
        }

        private static string CreateDirectoryIfNecessary(string directory)
        {
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);
            }
                
            Directory.CreateDirectory(directory);

            return directory;
        }
#endif
	}

    public interface INQTestRunner
    {
        bool Run();   
    }
}
