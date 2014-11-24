/* Copyright (C) 2004 Versant Inc.	  http://www.db4o.com */
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using Sharpen.Lang;

namespace Sharpen
{
	public class Runtime 
	{
		private static long DIFFERENCE_IN_TICKS = 62135604000000;
		private static long RATIO = 10000;

		public static TextWriter Out
		{
			get
			{
				return Console.Out;
			}
		}

		public static TextWriter Err
		{
			get
			{
				return Console.Error;
			}
		}

		public static object GetArrayValue(object array, int i)
	    {
	        return ((Array)array).GetValue(i);
	    }
	    
	    public static int GetArrayLength(object array)
	    {
            return ((Array) array).Length;
	    }

	    public static void SetArrayValue(object array, int index, object value)
	    {
	        ((Array)array).SetValue(value, index);
	    }

        private const BindingFlags AllMembers = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        
		private const BindingFlags AllMembersIncludingStatic = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        private const BindingFlags DeclaredMembers = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

		public static FieldInfo GetDeclaredField(Type type, string name)
		{
            return type.GetField(name, AllMembersIncludingStatic);
		}

		public static FieldInfo[] GetDeclaredFields(Type type)
		{
			//return type.GetFields(DeclaredMembers | BindingFlags.Static);

			// Workaound for the following bug:
			//https://connect.microsoft.com/VisualStudio/feedback/details/566678/field-of-generic-class-missing-when-getmembers-called-with-declaredonly-if-it-was-used-in-an-expression 
			List<FieldInfo> declaredFields = new List<FieldInfo>();

			var allFields = type.GetFields(AllMembersIncludingStatic);
			foreach (var field in allFields)
			{
				if (field.DeclaringType == type)
				{
					declaredFields.Add(field);
				}
			}

			return declaredFields.ToArray();
		}

		public static MethodInfo GetDeclaredMethod(Type type, string name, Type[] parameterTypes)
		{
			return type.GetMethod(name, DeclaredMembers, null, parameterTypes, null);
		}

        public static MethodInfo GetMethod(Type type, string name, Type[] parameterTypes)
        {
            return type.GetMethod(name, AllMembers, null, parameterTypes, null);
        }

		public static Type[] GetParameterTypes(MethodBase method)
		{
			ParameterInfo[] parameters = method.GetParameters();
			Type[] types = new Type[parameters.Length];
			for (int i=0; i<types.Length; ++i)
			{
				types[i] = parameters[i].ParameterType;
			}
			return types;
		}

		public static long CurrentTimeMillis() 
		{
			return ToJavaMilliseconds(DateTime.Now.ToUniversalTime());
		}

		public static int FloatToIntBits(float value) 
		{
			return BitConverter.ToInt32(BitConverter.GetBytes(value), 0);
		}

		public static void Gc() 
		{
			GC.Collect();
		}
		
		public static bool EqualsIgnoreCase(string lhs, string rhs) 
		{
#if SILVERLIGHT
			return 0 == string.Compare(lhs, rhs, StringComparison.OrdinalIgnoreCase);
#else
			return 0 == string.Compare(lhs, rhs, true);
#endif
		}

		public static int CompareOrdinal(String a, String b)
		{
			return String.CompareOrdinal(a, b);
		}

		public static string Substring(String s, int startIndex)
		{
			return s.Substring(startIndex);
		}

		public static string Substring(String s, int startIndex, int endIndex)
		{
			return s.Substring(startIndex, endIndex-startIndex);
		}

		public static void GetCharsForString(string str, int start, int end, char[] destination, int destinationStart) 
		{
			str.CopyTo(start, destination, 0, end-start);
		}

		public static byte[] GetBytesForString(string str)
		{
#if SILVERLIGHT
			return System.Text.Encoding.Unicode.GetBytes(str);
#else
			return System.Text.Encoding.Default.GetBytes(str);
#endif
		}

		public static string GetStringForBytes(byte[] bytes, int index, int length)
		{
#if SILVERLIGHT
			return System.Text.Encoding.Unicode.GetString(bytes, index, length);
#else
			return System.Text.Encoding.Default.GetString(bytes, index, length);
#endif
		}

		public static string GetStringValueOf(object value) 
		{
			return null == value
				? "null"
				: value.ToString();
		}

		public static String GetProperty(String key) 
		{
			return GetProperty(key, null);
		}

		public static String GetProperty(String key, String defaultValue)
		{
#if CF
			return key.Equals("line.separator") ? "\n" : defaultValue;
#else
			return key.Equals("line.separator")
				? Environment.NewLine
				: GetEnvironmentVariable(key, defaultValue);
#endif
		}

		public static string GetEnvironmentVariable(string variableName, string defaultValue)
		{
#if CF || SILVERLIGHT
			return defaultValue;
#else
			string value = Environment.GetEnvironmentVariable(variableName);
			if (value == null || value.Length == 0) return defaultValue;
			return value;
#endif
		}

		public static object GetReferenceTarget(WeakReference reference) 
		{
			return reference.Target;
		}

		public static long GetTimeForDate(DateTime dateTime) 
		{
			return ToJavaMilliseconds(dateTime);
		}

		public static int IdentityHashCode(object obj) 
		{
			return IdentityHashCodeProvider.IdentityHashCode(obj);
		}

		public static float IntBitsToFloat(int value) 
		{
			return BitConverter.ToSingle(BitConverter.GetBytes(value), 0);
		}

		public static void Wait(object obj, long timeout) 
		{
#if CF
			throw new NotImplementedException();
#else
			Monitor.Wait(obj, (int) timeout);
#endif
		}

		public static void Wait(object obj) 
		{
#if CF
			throw new NotImplementedException();
#else
			Monitor.Wait(obj);
#endif
		}

		public static void Notify(object obj) 
		{
#if CF
			throw new NotImplementedException();
#else
			Monitor.Pulse(obj);
#endif
		}

		public static void NotifyAll(object obj) 
		{
#if CF
			throw new NotImplementedException();
#else
			Monitor.PulseAll(obj);
#endif
		}

		public static void PrintStackTrace(Exception exception) 
		{
			PrintStackTrace(exception, Err);
		}

		public static void PrintStackTrace(Exception exception, TextWriter writer) 
		{
			writer.WriteLine(exception);
		}

		public static void RunFinalization() 
		{
			GC.WaitForPendingFinalizers();
		}

		public static void RunFinalizersOnExit(bool flag) 
		{
			// do nothing
		}

        public static Type GetType(string typeName)
        {
            return TypeReference.FromString(typeName).Resolve();
        }

		public static long ToJavaMilliseconds(DateTime dateTimeNet)
		{
			return ToJavaMilliseconds(dateTimeNet.Ticks);
		}

		public static long ToJavaMilliseconds(long ticks)
		{
			return ticks / RATIO - DIFFERENCE_IN_TICKS;
		}

		public static long ToNetTicks(long javaMilliseconds)
		{
			return (javaMilliseconds + DIFFERENCE_IN_TICKS) * RATIO;
		}

        public static string Getenv(string name)
        {
#if CF || SILVERLIGHT
			throw new NotImplementedException();
#else
            return Environment.GetEnvironmentVariable(name);
#endif
        }

 
    }
}