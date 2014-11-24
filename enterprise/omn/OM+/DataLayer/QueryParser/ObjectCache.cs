using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace OManager.DataLayer.QueryParser
{
    public class ObjectCache
    {
        //This class help in avoiding garbage collection
        private static Dictionary<long, object> objCache = new Dictionary<long, object>();

        public static void AddObject(long id, object obj)
        {

            if (!objCache.ContainsKey(id))
            {
                objCache.Add(id, obj);
            }
        }

        public static void RemoveObject(long id)
        {

            if (objCache.ContainsKey(id))
            {
                objCache.Remove(id);
            }


        }

        public static void ClearAll()
        {
            objCache.Clear();
        }
    }
}
