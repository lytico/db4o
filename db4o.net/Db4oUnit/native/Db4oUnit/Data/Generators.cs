using System;
using System.Collections;
using Db4objects.Db4o.Foundation;

namespace Db4oUnit.Data
{
	public partial class Generators
	{
		public static IEnumerable PlatformSpecificArbitraryValuesOf(Type type)
		{
			if (IsNullable(type))
			{
				return Iterators.Append(Generators.ArbitraryValuesOf(type.GetGenericArguments()[0]), null);
			}
			return null;
		}

		private static bool IsNullable(Type type)
		{
			return type.IsGenericType && typeof(Nullable<>) == type.GetGenericTypeDefinition();
		}
	}
}
