/* Copyright (C) 2009 Versant Inc.   http://www.db4o.com */
#if SILVERLIGHT
using System;
using System.IO;
using System.IO.IsolatedStorage;

namespace Db4objects.Db4o.IO
{
	public static class SilverlightIO
	{
		public static bool Exists(string path)
		{
			return ExistsIn(IsolatedStorageFile.GetUserStoreForApplication(), path);
		}

		private static bool ExistsIn(IsolatedStorageFile storage, string path)
		{
			return storage.FileExists(path);
		}

		public static bool Delete(string path)
		{
			IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
			if (ExistsIn(storage, path))
			{
				storage.DeleteFile(path);
				return !ExistsIn(storage, path);
			}
			return false;
		}

		public static long Length(string path)
		{
			IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
			using (IsolatedStorageFileStream fileStream = storage.OpenFile(path, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				return fileStream.Length;
			}
		}
	}
}
#endif