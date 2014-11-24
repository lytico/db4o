using System;
using System.Collections.Generic;

namespace Db4oTool
{
	class Factory
	{
		public static IEnumerable<T> Instantiate<T>(IEnumerable<string> typeNames)
		{
			foreach (string typeName in typeNames)
			{
				yield return Instantiate<T>(typeName);
			}
		}

		public static T Instantiate<T>(string typeName)
		{
			return (T)Activator.CreateInstance(Type.GetType(typeName, true));
		}
	}
}
