/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */

using System;
using System.IO;
using Db4oUnit;

namespace Db4objects.Db4o.Tests
{
#if !CF && !SILVERLIGHT
	public class ShutdownMultipleContainer : ITestLifeCycle
	{
		private const string _firstFile = "first.db4o";
		private const string _secondFile = "second.db4o";

		public class Runner : MarshalByRefObject
		{
			public void Run(TextWriter err)
			{
				Console.SetError(err);
				Db4oFactory.OpenFile(_firstFile);
				Db4oFactory.OpenFile(_secondFile);
			}
		}

		public void Test()
		{
			TextWriter stderr = Console.Error;
			StringWriter output = new StringWriter();
			try
			{
				Console.SetError(output);
				RunTestInAnotherDomain();
				CheckOutput(output.ToString());
			}
			finally
			{
				Console.SetError(stderr);
			}
		}

		void CheckOutput(string output)
		{
			Assert.AreNotEqual(-1, output.IndexOf(_firstFile));
			Assert.AreNotEqual(-1, output.IndexOf(_secondFile));
		}

		void RunTestInAnotherDomain()
		{
			CleanFiles();

			AppDomain testDomain = null;
			try
			{
				testDomain = AppDomain.CreateDomain("testDomain");
				Runner r = NewRunnerInAppDomain(testDomain);

				r.Run(Console.Error);
			}
			finally
			{
				if (testDomain != null)
				{
					AppDomain.Unload(testDomain);
				}
			}
		}

		private Runner NewRunnerInAppDomain(AppDomain testDomain)
		{
			return (Runner) testDomain.CreateInstanceAndUnwrap(GetType().Assembly.FullName, typeof (Runner).FullName);
		}

		public void SetUp()
		{
		}

		public void TearDown()
		{
			CleanFiles();
		}

		private static void CleanFiles()
		{
			Clean(_firstFile);
			Clean(_secondFile);
		}

		static void Clean(string file)
		{
			if (File.Exists(file)) File.Delete(file);
		}
	}
#endif
}
