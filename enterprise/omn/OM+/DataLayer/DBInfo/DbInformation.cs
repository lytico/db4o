using System;
using System.Collections.Generic;
using System.Collections;
using Db4objects.Db4o;
using Db4objects.Db4o.Ext ;
using OManager.DataLayer.Connection;
using OME.Logging.Common;

namespace OManager.DataLayer.DBInfo
{
    public static class DbInformation
    {
        public static Hashtable StoredClasses()
        {
			Hashtable storedClassHashtable = new Hashtable();
            try
            {
                IStoredClass[] storedClasses = Db4oClient.Client.Ext().StoredClasses();
                foreach (IStoredClass stored in storedClasses)
                {
                    string className = stored.GetName();
                    if (ExcludeClass(className))
                        continue;
                    storedClassHashtable.Add(className, SimpleName(className));
                }
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
            }
			
			return storedClassHashtable;
		}

    	public static Hashtable StoredClassesByAssembly()
        {
    		try
            {
                IStoredClass[] storedClasses = Db4oClient.Client.Ext().StoredClasses();
                Hashtable storedClassHashtable = new Hashtable();
                foreach (IStoredClass stored in storedClasses)
                {
                    string className = stored.GetName();
                    if (!ExcludeClass(className))
                    {
                        string assemblyName = GetAssemblyName(className);
                    	if (!storedClassHashtable.ContainsKey(assemblyName))
                        {
							storedClassHashtable.Add(assemblyName, new List<string>());
                        }

						List<string> assemblyClasses = storedClassHashtable[assemblyName] as List<string>;
						assemblyClasses.Add(className);
                    }
                }

            	return SortHashtable(storedClassHashtable);
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return null; 
            }
        }

    	public static long GetFreeSizeofDatabase()
        {
            IObjectContainer objectContainer = Db4oClient.Client;
        	long freeSizeOfDB;
            try
            {
                freeSizeOfDB = objectContainer.Ext().SystemInfo().FreespaceSize();
               
            }
            catch (NotImplementedException)
            {
                freeSizeOfDB = - 1;
               
            }
            catch (Exception oEx)
            {
                freeSizeOfDB = -1;
                LoggingHelper.HandleException(oEx);
            }

            return freeSizeOfDB;
        }

       

        public static long getTotalDatabaseSize()
        {
            IObjectContainer objectContainer = Db4oClient.Client;
        	try
            {
                return objectContainer.Ext().SystemInfo().TotalSize();
            }
            catch (NotImplementedException)
            {
                return -1;
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
				return -2;
			}
        }

        public static int GetNumberOfClassesinDB()
        {
            try
            {
                Hashtable countforClasses = StoredClasses();
                return countforClasses.Count;
            }
            catch (NotImplementedException)
            {
                return -1;
            }
            catch (Exception oEx)
            {
				LoggingHelper.HandleException(oEx);
				return -2;
            }
        }

		private static string GetAssemblyName(string className)
		{
			int intIndexof = className.LastIndexOf(',') + 1;
			return className.Substring(intIndexof, className.Length - intIndexof).Trim();
		}

		private static string SimpleName(string className)
		{
			int intIndexof = className.LastIndexOf(',');
			return intIndexof >= 0 ? className.Substring(0, intIndexof) : className;
		}

		private static bool ExcludeClass(string className)
		{

            if (className.EndsWith("mscorlib"))
                return true;
			Type type = Type.GetType(className, false);
			return type != null && typeof(IInternal4).IsAssignableFrom(type);
		}

		private static Hashtable SortHashtable(IDictionary original)
		{
			ArrayList hashKeys = new ArrayList(original.Keys);
			hashKeys.Sort();

			Hashtable sortedHashtable = new Hashtable();
			foreach (string key in hashKeys)
			{
				sortedHashtable.Add(key, original[key]);
			}
			return sortedHashtable;
		}

        public  static long getDatabaseCreationTime()
        {
            IObjectContainer objectContainer = Db4oClient.Client;
            Db4oDatabase db = objectContainer.Ext().Identity();
            return  db.GetCreationTime();
        }
	}
}
