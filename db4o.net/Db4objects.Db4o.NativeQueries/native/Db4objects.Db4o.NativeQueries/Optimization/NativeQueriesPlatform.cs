using System.Reflection;

namespace Db4objects.Db4o.NativeQueries.Optimization
{
	class NativeQueriesPlatform
	{
		public static bool IsStatic(MethodInfo method)
		{
			return method.IsStatic;
		}

		public static string ToPlatformName(string name)
		{
			return char.ToUpper(name[0]) + name.Substring(1);
		}
	}
}
