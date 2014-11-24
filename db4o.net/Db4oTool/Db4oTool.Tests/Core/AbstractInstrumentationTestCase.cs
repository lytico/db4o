/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Db4objects.Db4o;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Query;
using Db4oUnit;
using Db4oUnit.Extensions.Util;

namespace Db4oTool.Tests.Core
{
	public abstract class AbstractInstrumentationTestCase : ITestSuiteBuilder
	{
		public const string DatabaseFile = "subject.db4o";
		
		class InstrumentedTestMethod : TestMethod
		{
			private AbstractInstrumentationTestCase _testCase;

			public InstrumentedTestMethod(AbstractInstrumentationTestCase testCase, object subject, MethodInfo method) : base(subject, method)
			{
				_testCase = testCase;
			}

			override protected void SetUp()
			{
				SetUpAssemblyResolver();
				SetUpContainer();
				base.SetUp();
			}
			
			override protected void TearDown()
			{
				try
				{
					base.TearDown();
				}
				finally
				{
					TearDownContainer();
					TearDownAssemblyResolver();
				}
			}

			private void SetUpAssemblyResolver()
			{
				AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
			}

			private void TearDownAssemblyResolver()
			{
				AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
			}

			private void SetUpContainer()
			{
				((InstrumentedTestCase)GetSubject()).Container = _testCase.OpenDatabase();
			}

			private void TearDownContainer()
			{
				InstrumentedTestCase testCase = (InstrumentedTestCase) GetSubject();
				testCase.Container.Close();
				testCase.Container = null;
			}

			private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
			{
				foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
				{
					if (assembly.GetName().Name == args.Name) return assembly;
				}
				return null;
			}
		}
		
		class InstrumentationTestSuiteBuilder : ReflectionTestSuiteBuilder
		{
			private AbstractInstrumentationTestCase _testCase;

			public InstrumentationTestSuiteBuilder(AbstractInstrumentationTestCase testCase, Type clazz)
				: base(clazz)
			{
				_testCase = testCase;
			}

			protected override ITest CreateTest(object instance, MethodInfo method)
			{
				return new InstrumentedTestMethod(_testCase, instance, method);
			}
		}

		public IEnumerator GetEnumerator()
		{
			try
			{
				return BuildFromInstrumentedAssembly();
			}
			catch (Exception x)
			{
				return new ITest[] { new FailingTest(TestSuiteLabel, x) }.GetEnumerator();
			}
		}

		private IEnumerator BuildFromInstrumentedAssembly()
		{
			return ProduceTestCases().GetEnumerator();
		}

        private IEnumerable<ITest> ProduceTestCases()
        {
			Assert.IsTrue(AcceptsDebugMode || AcceptsReleaseMode);

			if (AcceptsDebugMode)
        		foreach (ITest test in ProduceTestCases(true))
        			yield return test;

			if (AcceptsReleaseMode)
				foreach (ITest test in ProduceTestCases(false))
					yield return test;
        }

		protected virtual bool AcceptsDebugMode
		{
			get { return true; }
		}

		protected virtual bool AcceptsReleaseMode
		{
			get { return true; }
		}

		private IEnumerable<ITest> ProduceTestCases(bool debugInfo)
		{
			Exception error = null;
			Assembly[] references = Dependencies;
			foreach (string resource in Resources)
			{
				if (null != error)
				{
					yield return new FailingTest(resource, error);
					continue;
				}

				string assemblyPath = EmitAndInstrumentAssemblyFromResource(resource, references, debugInfo, out error);
				if (null != error)
				{
					yield return new FailingTest(resource, error);
					error = new Exception("The sibling resource '" + resource + "' has errors.", error);
					continue;
				}

				Type type = GetTestCaseType(assemblyPath, resource);
				IEnumerable suite = type.IsSubclassOf(typeof(InstrumentedTestCase))
				                    	? new InstrumentationTestSuiteBuilder(this, type)
				                    	: new ReflectionTestSuiteBuilder(type);

				foreach (Object test in suite)
				{
					yield return (ITest)test;
				}

				if (ShouldVerify(resource))
				{
					yield return new VerifyAssemblyTest(assemblyPath);
				}

				references = ArrayServices.Append(references, type.Assembly);
			}
		}

		private string EmitAndInstrumentAssemblyFromResource(string resource, Assembly[] references, bool debugInfo, out Exception error)
		{
			string assemblyPath = null;
			try
			{
				CompilationServices.Debug.Using(debugInfo, delegate
               	{
               		assemblyPath = EmitAssemblyFromResource(resource, references);
//               		Console.WriteLine("Assembly emitted to: {0}", assemblyPath);
               		Assert.IsTrue(File.Exists(assemblyPath));

               		InstrumentAssembly(assemblyPath);
               		
               	});
				error = null;
			}
			catch (Exception x)
			{
				error = x;
			}
			return assemblyPath;
		}

		protected virtual bool ShouldVerify(string resource)
		{
			return true;
		}

		protected string TestSuiteLabel
		{
			get { return GetType().FullName;  }
		}
		
		protected abstract string[] Resources { get; }

		protected abstract void InstrumentAssembly(string location);

		protected virtual void OnQueryExecution(object sender, QueryExecutionEventArgs args)
		{
			throw new NotImplementedException();
		}

		protected virtual void OnQueryOptimizationFailure(object sender, QueryOptimizationFailureEventArgs args)
		{
			throw new ApplicationException(args.Reason.Message, args.Reason);
		}

        private Type GetTestCaseType(string assemblyName, string resource)
		{
			Assembly assembly = Assembly.LoadFrom(assemblyName);
			return assembly.GetType(resource, true);
		}

		private IObjectContainer OpenDatabase()
		{
			if (File.Exists(DatabaseFile)) File.Delete(DatabaseFile);
			IObjectContainer container = Db4oFactory.OpenFile(DatabaseFile);
			NativeQueryHandler handler = ((ObjectContainerBase)container).GetNativeQueryHandler();
			handler.QueryExecution += OnQueryExecution;
			handler.QueryOptimizationFailure += OnQueryOptimizationFailure;
			return container;
		}

        protected virtual string EmitAssemblyFromResource(string resource, Assembly[] references)
        {
            CopyDependenciesToTemp();
            string resourceName = ResourceServices.CompleteResourceName(GetType(), resource);
            return CompilationServices.EmitAssemblyFromResource(resourceName, references);
        }

		virtual protected void CopyDependenciesToTemp()
		{
			foreach (Assembly dependency in Dependencies)
			{
				ShellUtilities.CopyAssemblyToTemp(dependency);
			}
		}

		protected virtual Assembly[] Dependencies
		{
			get
			{
				return new Assembly[]
					{
						typeof(IObjectContainer).Assembly,
						typeof(Assert).Assembly,
						typeof(DiagnosticCollector<>).Assembly,
						GetType().Assembly
					};
			}
		}
	}
}