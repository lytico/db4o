using System;
using Db4objects.Db4o.Query;

namespace SimpleCRUD
{
	public static class QueryExtensions
	{
		public static void AddConstraint(this IQuery query, string fieldName, string value)
		{
			if (!String.IsNullOrEmpty(value))
			{
				query.Descend(fieldName).Constrain(value);
			}
		}

	}
}
