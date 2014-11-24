/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
using System;
using System.IO;
using System.Reflection;
using Db4objects.Db4o.Tests.Util;
using Db4oUnit;
using Db4oUnit.Extensions.Util;

namespace Db4objects.Db4o.Tests.CLI1
{
#if !CF && !SILVERLIGHT
    /// <summary>
    /// This test emits an assembly with a version in one app domain
    /// and then unloads it.
    /// 
    /// Then emits the same assembly with a different version in a second
    /// AppDomain, and then tries to load the classes from it.
    /// </summary>
    public class CsAssemblyVersionChange : ITestCase
    {
        const string TestAssemblyName = "test.dll";

        protected static readonly string DataFile = Path.Combine(Path.GetTempPath(), "test.yap");

        public void Test()
        {
            string version1Code = "[assembly: System.Reflection.AssemblyVersion(\"1.0.0.0\")]";
            string version2Code = "[assembly: System.Reflection.AssemblyVersion(\"2.0.0.0\")]";

            string appDomain1BasePath = Path.Combine(Path.GetTempPath(), "appdomain1");
            string appDomain2BasePath = Path.Combine(Path.GetTempPath(), "appdomain2");

            CompilationServices.EmitAssembly(Path.Combine(appDomain1BasePath, TestAssemblyName), BaseCode, version1Code);
            CompilationServices.EmitAssembly(Path.Combine(appDomain2BasePath, TestAssemblyName), BaseCode, version2Code);

            if (File.Exists(DataFile))
            {
                File.Delete(DataFile);
            }

            try
            {
                ExecuteTestMethodInAppDomain(appDomain1BasePath, "Store");
                ExecuteTestMethodInAppDomain(appDomain2BasePath, "Load");
            }
            catch (Exception e)
            {
                while (e is TargetInvocationException)
                {
                    e = e.InnerException;
                }
                Assert.Fail(e.Message);
            }
        }

        [Serializable]
        class TestMethodRunner
        {
            string _methodName;

            public TestMethodRunner(string methodName)
            {
                _methodName = methodName;
            }

            public void Execute()
            {
                Assembly testAssembly = Assembly.LoadFrom(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestAssemblyName));
                Type type = testAssembly.GetType("Tester", true);
                MethodInfo method = type.GetMethod(_methodName);
                method.Invoke(null, new object[1] { DataFile });
            }
        }

        void ExecuteTestMethodInAppDomain(string basePath, string testMethod)
        {
            CopyNecessaryAssembliesTo(basePath);

            AppDomainSetup setup = new AppDomainSetup();
            setup.ApplicationBase = basePath;

            AppDomain domain = AppDomain.CreateDomain("db4o-assembly-test-domain", null, setup);
            try
            {
                domain.DoCallBack(new CrossAppDomainDelegate(new TestMethodRunner(testMethod).Execute));
            }
            finally
            {
                AppDomain.Unload(domain);
            }
        }

        static void CopyNecessaryAssembliesTo(string basePath)
        {
        	IOServices.CopyEnclosingAssemblyTo(typeof(Db4oFactory), basePath);
			IOServices.CopyEnclosingAssemblyTo(typeof(Db4oUnit.Assert), basePath);
            CopyTo(Assembly.GetExecutingAssembly().Location, basePath);
        }

    	static void CopyTo(string fname, string dir)
        {
            IOServices.CopyTo(fname, dir);
        }

        string BaseCode
        {
            get
            {
                return @"
using System;
using System.IO;
using Db4objects.Db4o;

public class SimpleGenericType<T>
{
	public T value;

	public SimpleGenericType(T value)
	{
		this.value = value;
	}
}

public class Tester
{
	public static void Store(string fname)
	{
		using (IObjectContainer container = Db4oFactory.OpenFile(fname))
		{
			container.Store(new SimpleGenericType<int>(42));
			container.Store(new SimpleGenericType<SimpleGenericType<int>>(new SimpleGenericType<int>(13)));
		}
	}
	
	public static void Load(string fname)
	{
		using (IObjectContainer container = Db4oFactory.OpenFile(fname))
		{
			IObjectSet os = container.QueryByExample(typeof(SimpleGenericType<int>));
			AssertEquals(2, os.Count);
			
			os = container.QueryByExample(typeof(SimpleGenericType<SimpleGenericType<int>>));
			AssertEquals(1, os.Count);
		}
	}
	
	static void AssertEquals(object expected, object actual)
	{
		if (!Object.Equals(expected, actual))
		{
			throw new ApplicationException();
		}
	}
}
            ";

            }
        }
    }
#endif
}
