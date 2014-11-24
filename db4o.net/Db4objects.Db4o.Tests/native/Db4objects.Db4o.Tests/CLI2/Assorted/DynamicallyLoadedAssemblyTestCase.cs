/* Copyright (C) 2011  Versant Inc.   http://www.db4o.com */
#if !SILVERLIGHT && !CF

using System.IO;
using System.Reflection;
using Db4objects.Db4o.Tests.Util;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI2.Assorted
{
	public class DynamicallyLoadedAssemblyTestCase : AbstractDb4oTestCase
	{
		protected override void Store()
		{
			var dynamicAssemblyPath = Path.GetTempFileName() + ".dll";
			CompilationServices.EmitAssembly(	dynamicAssemblyPath,

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

			_dynamicAssembly = Assembly.LoadFrom(dynamicAssemblyPath);
		}

		public void Test()
		{
			var obj = _dynamicAssembly.CreateInstance(TypeName);
			const int value = 42;

			SetField(obj, FieldName, value);
			Store(obj);

			Reopen();

			var retrieved = RetrieveOnlyInstance(obj.GetType());
			
			Assert.AreEqual(value, GetField(retrieved, FieldName));
			Assert.AreEqual(obj.GetType(), retrieved.GetType());
		}

		private static int GetField(object obj, string fieldName)
		{
			var field = obj.GetType().GetField(fieldName);
			return (int) field.GetValue(obj);
		}

		private static void SetField(object obj, string fieldName, int value)
		{
			var field = obj.GetType().GetField(fieldName);
			field.SetValue(obj, value);
		}

		const string TypeName = "Test.Subject";
		const string FieldName = "value";
		private Assembly _dynamicAssembly;
	}
}
#endif