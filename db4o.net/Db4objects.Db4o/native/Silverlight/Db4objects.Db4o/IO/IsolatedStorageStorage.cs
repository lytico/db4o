/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

#if SILVERLIGHT

using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.IO;

namespace Db4objects.Db4o.IO
{
	public class IsolatedStorageStorage : IStorage
	{
		private static readonly IsolatedStorageFile _store = IsolatedStorageFile.GetUserStoreForApplication();
		private static readonly IDictionary<string, IsolatedStorageFileBin> _openBins = new Dictionary<string, IsolatedStorageFileBin>();

		#region IStorage Members

		public IBin Open(BinConfiguration config)
		{
			IsolatedStorageFileBin bin = new IsolatedStorageFileBin(config, _store);
			AddToOpenBinsCollection(bin);
			RegisterForOnCloseEvent(bin);
			
			return bin;
		}

		public bool Exists(string uri)
		{
			return _store.FileExists(uri) && FileSize(uri) > 0;
		}

		public void Delete(string uri)
		{
			if (_store.FileExists(uri))
			{
				_store.DeleteFile(uri);
			}
		}

		public void Rename(string oldUri, string newUri)
		{
			if (_store.FileExists(oldUri))
			{
				Copy(oldUri, newUri);
				Delete(oldUri);
			}
		}

		private static void Copy(string from, string to)
		{
			using (IsolatedStorageFileStream fromStream = _store.OpenFile(from, FileMode.Open, FileAccess.Read, FileShare.None))
			{
				using(IsolatedStorageFileStream toStream = _store.OpenFile(to, FileMode.CreateNew, FileAccess.Write, FileShare.None))
				{
					byte []buffer = new byte[1024 * 1024];
					int count = fromStream.Read(buffer, 0, buffer.Length);
					while (count > 0)
					{
						toStream.Write(buffer, 0, count);
						count = fromStream.Read(buffer, 0, buffer.Length);
					}
				}
			}
		}

		#endregion

		public static long FileSize(string uri)
		{
			lock (_openBins)
			{
				if (IsBinAlreadyOpen(uri))
				{
					IsolatedStorageFileBin bin = _openBins[uri];
					return bin.Length();
				}

				using (IsolatedStorageFileStream fileStream = _store.OpenFile(uri, FileMode.Open, FileAccess.Read, FileShare.None))
				{
					return fileStream.Length;
				}
			}
		}

		private static bool IsBinAlreadyOpen(string uri)
		{
			return _openBins.ContainsKey(uri);
		}

		private static void RegisterForOnCloseEvent(IsolatedStorageFileBin bin)
		{
			bin.OnClose += (sender, arg) => RemoveFromOpenBinCollection(((IsolatedStorageFileBin)sender).Path);
		}

		private static void RemoveFromOpenBinCollection(string path)
		{
			lock (_openBins)
			{
				if (_openBins.ContainsKey(path))
				{
					_openBins.Remove(path);
				}
			}
		}

		private static void AddToOpenBinsCollection(IsolatedStorageFileBin bin)
		{
			_openBins[bin.Path] = bin;
		}
	}
}

#endif
