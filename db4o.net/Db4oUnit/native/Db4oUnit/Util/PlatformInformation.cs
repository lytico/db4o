/* Copyright (C) 2004 - 2007  Versant Inc.  http://www.db4o.com */
namespace Db4oUnit.Util
{
	public class PlatformInformation
	{
		public static bool IsJava()
		{
			return false; 
		}

		public static bool IsDotNet()
		{
			return true;
		}
	}
}
