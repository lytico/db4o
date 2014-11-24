/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4oUnit;
using Db4oUnit.Mocking;
using Db4objects.Db4o.Foundation;

namespace Db4oUnit.Mocking
{
	public class CodeGenerator
	{
		/// <summary>
		/// Generates an array that can be used with
		/// <see cref="MethodCallRecorder.Verify(MethodCall[])">MethodCallRecorder.Verify(MethodCall[])
		/// 	</see>
		/// .
		/// Example:
		/// MethodCallRecorder recorder = new MethodCallRecorder();
		/// runTest(recorder);
		/// System.out.println(CodeGenerator.generateMethodCallArray(recorder))
		/// </summary>
		/// <param name="calls">MethodCall generator</param>
		/// <returns>array string</returns>
		public static string GenerateMethodCallArray(IEnumerable calls)
		{
			IEnumerable callStrings = Iterators.Map(calls, new _IFunction4_23());
			return Iterators.Join(callStrings.GetEnumerator(), "," + TestPlatform.NewLine);
		}

		private sealed class _IFunction4_23 : IFunction4
		{
			public _IFunction4_23()
			{
			}

			public object Apply(object arg)
			{
				return CodeGenerator.GenerateMethodCall((MethodCall)arg);
			}
		}

		public static string GenerateValue(object value)
		{
			if (value == null)
			{
				return "null";
			}
			if (value is string)
			{
				return "\"" + value + "\"";
			}
			if (value is object[])
			{
				return GenerateArray((object[])value);
			}
			return value.ToString();
		}

		public static string GenerateArray(object[] array)
		{
			IEnumerator values = Iterators.Map(Iterators.Iterate(array), new _IFunction4_45()
				);
			return "new Object[] " + Iterators.Join(values, "{", "}", ", ");
		}

		private sealed class _IFunction4_45 : IFunction4
		{
			public _IFunction4_45()
			{
			}

			public object Apply(object arg)
			{
				return CodeGenerator.GenerateValue(arg);
			}
		}

		public static string GenerateMethodCall(MethodCall call)
		{
			return "new MethodCall(\"" + call.methodName + "\", " + GenerateArray(call.args) 
				+ ")";
		}
	}
}
