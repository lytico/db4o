
using System.Collections;
using System.Collections.Generic;

namespace Sharpen
{
	public class Collections
	{
        public static void AddAll(System.Collections.IList list, System.Collections.IEnumerable added)
        {
            foreach (object o in added)
            {
                list.Add(o);
            }
        }

        public static bool AddAll<T>(ICollection<T> list, System.Collections.Generic.IEnumerable<T> added)
        {
            foreach (T o in added)
            {
                list.Add(o);
            }
            return true;
        }

		public static object Remove(IDictionary dictionary, object key)
		{
			object removed = dictionary[key];
			dictionary.Remove(key);
			return removed;
		}

	    public static object[] ToArray(ICollection collection)
	    {
	    	object[] result = new object[collection.Count];
			collection.CopyTo(result, 0);
			return result;
	    }

		public static T[] ToArray<T>(ICollection collection, T[] result)
		{
			collection.CopyTo(result, 0);
			return result;
		}

		public static T[] ToArray<T>(ICollection<T> collection, T[] result)
		{
			collection.CopyTo(result, 0);
			return result;
		}
	}
}
