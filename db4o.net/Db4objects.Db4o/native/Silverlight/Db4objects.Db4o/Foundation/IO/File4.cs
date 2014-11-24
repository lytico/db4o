/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */

#if SILVERLIGHT
using System;
using System.IO;
using System.IO.IsolatedStorage;

namespace Db4objects.Db4o.Foundation.IO
{
	public class File4
	{
		
		public static void Delete(string file)
		{
			IsolatedStorageFile storageFile = IsolatedStorageFile.GetUserStoreForApplication();
			if (storageFile.FileExists(file))
			{
				storageFile.DeleteFile(file);
			}
		}

		public static void Copy(string from, string to)
		{
			throw new NotImplementedException();
		}

		public static bool Exists(string file)
		{
			IsolatedStorageFile storageFile = IsolatedStorageFile.GetUserStoreForApplication();
			return storageFile.FileExists(file);
		}

		public static long Size(string filePath)
		{
			using (IsolatedStorageFile storageFile = IsolatedStorageFile.GetUserStoreForApplication())
			{
				using (IsolatedStorageFileStream fileStream = storageFile.OpenFile(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				{
					return fileStream.Length;
				}
			}
		}
	}
 }
#endif
