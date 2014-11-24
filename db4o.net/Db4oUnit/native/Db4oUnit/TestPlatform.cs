/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
namespace Db4oUnit
{
	using System;
	using System.IO;
	using System.Reflection;

	public class TestPlatform
	{
#if CF
        public static string NewLine = "\n";
#else
	    public static string NewLine = Environment.NewLine;
#endif

		// will be assigned from the outside on CF
		public static TextWriter Out;

        public static TextWriter Error;
        
		static TestPlatform()
		{
			Out = Console.Out;
            Error = Console.Error;
		}
		
		public static void PrintStackTrace(TextWriter writer, Exception e)
		{	
			writer.Write(e);
		}

        public static TextWriter GetNullWriter()
        {
            return new NullTextWriter();
        }
        
        public static TextWriter GetStdErr()
		{
			return Error;
		}
		
		public static void EmitWarning(string warning)
		{
			Out.WriteLine(warning);
		}		

		public static bool IsStatic(MethodInfo method)
		{
			return method.IsStatic;
		}

		public static bool IsPublic(MethodInfo method)
		{
			return method.IsPublic;
		}

		public static bool HasParameters(MethodInfo method)
		{
			return method.GetParameters().Length > 0;
		}

        public static TextWriter OpenTextFile(string fname)
        {
            return new StreamWriter(fname);
        }
	}
}
