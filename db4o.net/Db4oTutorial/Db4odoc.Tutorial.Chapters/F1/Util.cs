using System;
using System.IO;
using Db4objects.Db4o;

namespace Db4odoc.Tutorial.F1
{
	public class Util
	{
		
		public static void ListResult(IObjectSet result)
		{
			Console.WriteLine(result.Count);
			foreach (object item in result)
			{
				Console.WriteLine(item);
			}
		}

		public static void ListRefreshedResult(IObjectContainer container, IObjectSet items, int depth)
		{
			Console.WriteLine(items.Count);
			foreach (object item in items)
			{	
				container.Ext().Refresh(item, depth);
				Console.WriteLine(item);
			}
		}
		
		public static void RetrieveAll(IObjectContainer db) 
		{
			IObjectSet result = db.QueryByExample(typeof(Object));
			ListResult(result);
		}
		
		public static void DeleteAll(IObjectContainer db) 
		{
			IObjectSet result = db.QueryByExample(typeof(Object));
			foreach (object item in result)
			{
				db.Delete(item);
			}
		}		
	}
}
