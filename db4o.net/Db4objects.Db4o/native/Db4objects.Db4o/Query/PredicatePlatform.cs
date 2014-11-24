using System;

namespace Db4objects.Db4o.Query
{
	using System.Reflection;
	
	public sealed class PredicatePlatform
	{
		public static readonly string PredicatemethodName = "Match";
		
		public static bool IsFilterMethod(MethodInfo method)
		{
			if (method.GetParameters().Length != 1) return false;
			return method.Name == PredicatemethodName;
		}

        public static T GetField<T>(Object obj, string fieldName)
        {
            return (T) obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance).GetValue(obj);
        }
	}
}