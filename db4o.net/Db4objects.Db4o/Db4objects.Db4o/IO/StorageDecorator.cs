/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.IO;

namespace Db4objects.Db4o.IO
{
	/// <summary>Wrapper base class for all classes that wrap Storage.</summary>
	/// <remarks>
	/// Wrapper base class for all classes that wrap Storage.
	/// Each class that adds functionality to a Storage can
	/// extend this class.
	/// </remarks>
	/// <seealso cref="BinDecorator"></seealso>
	public class StorageDecorator : IStorage
	{
		protected readonly IStorage _storage;

		public StorageDecorator(IStorage storage)
		{
			_storage = storage;
		}

		public virtual bool Exists(string uri)
		{
			return _storage.Exists(uri);
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual IBin Open(BinConfiguration config)
		{
			return Decorate(config, _storage.Open(config));
		}

		protected virtual IBin Decorate(BinConfiguration config, IBin bin)
		{
			return bin;
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual void Delete(string uri)
		{
			_storage.Delete(uri);
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual void Rename(string oldUri, string newUri)
		{
			_storage.Rename(oldUri, newUri);
		}
	}
}
