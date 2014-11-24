/* Copyright (C) 2011 Versant Inc.   http://www.db4o.com */
#if !CF && !SILVERLIGHT

using System.IO;
using System.Reflection;
using Db4objects.Db4o.Tests.Util;
using Db4oUnit;
using Sharpen.Lang;

namespace Db4objects.Db4o.Tests.SharpenLang
{
	public class DynamicallyLoadedAssemblyTestCase : ITestLifeCycle
	{
		public void SetUp()
		{
			var dynamicAssemblyPath = Path.Combine(Path.GetTempPath(), "DynamicAssemblyTest.dll");
			CompilationServices.EmitAssembly(dynamicAssemblyPath,

			                                 new string[0],

			                                 @"
												namespace Test
												{
													public class Subject
													{
														public int value;
													}
												}
												");

			var type = Assembly.LoadFrom(dynamicAssemblyPath).GetTypes()[0];
			typeName = type.FullName + ", " + type.Assembly.GetName().Name;
		}

		public void TearDown()
		{
		}

		public void TestTypeIsResolved()
		{
			var resolvedType = TypeReference.FromString(typeName).Resolve();
			Assert.IsNotNull(resolvedType);
		}
		
		private string typeName;
	}
} 

#endif