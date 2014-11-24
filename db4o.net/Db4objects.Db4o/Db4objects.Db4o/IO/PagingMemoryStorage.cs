/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using System.IO;
using Db4objects.Db4o.IO;

namespace Db4objects.Db4o.IO
{
	/// <summary>
	/// <see cref="IStorage">IStorage</see>
	/// implementation that produces
	/// <see cref="IBin">IBin</see>
	/// instances
	/// that operate in memory.
	/// Use this
	/// <see cref="IStorage">IStorage</see>
	/// to work with db4o as an in-memory database.
	/// </summary>
	public class PagingMemoryStorage : IStorage
	{
		private const int DefaultPagesize = 4096;

		private readonly IDictionary _binsByUri = new Hashtable();

		private readonly int _pageSize;

		public PagingMemoryStorage() : this(DefaultPagesize)
		{
		}

		public PagingMemoryStorage(int pageSize)
		{
			_pageSize = pageSize;
		}

		/// <summary>
		/// returns true if a MemoryBin with the given URI name already exists
		/// in this Storage.
		/// </summary>
		/// <remarks>
		/// returns true if a MemoryBin with the given URI name already exists
		/// in this Storage.
		/// </remarks>
		public virtual bool Exists(string uri)
		{
			return _binsByUri.Contains(uri);
		}

		/// <summary>opens a MemoryBin for the given URI (name can be freely chosen).</summary>
		/// <remarks>opens a MemoryBin for the given URI (name can be freely chosen).</remarks>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual IBin Open(BinConfiguration config)
		{
			IBin bin = AcquireBin(config);
			return config.ReadOnly() ? new ReadOnlyBin(bin) : bin;
		}

		/// <summary>Returns the memory bin for the given URI for external use.</summary>
		/// <remarks>Returns the memory bin for the given URI for external use.</remarks>
		public virtual IBin Bin(string uri)
		{
			return ((IBin)_binsByUri[uri]);
		}

		/// <summary>Registers the given bin for this storage with the given URI.</summary>
		/// <remarks>Registers the given bin for this storage with the given URI.</remarks>
		public virtual void Bin(string uri, IBin bin)
		{
			_binsByUri[uri] = bin;
		}

		private IBin AcquireBin(BinConfiguration config)
		{
			IBin storage = Bin(config.Uri());
			if (null != storage)
			{
				return storage;
			}
			IBin newStorage = ProduceBin(config, _pageSize);
			_binsByUri[config.Uri()] = newStorage;
			return newStorage;
		}

		protected virtual IBin ProduceBin(BinConfiguration config, int pageSize)
		{
			return new PagingMemoryBin(pageSize, config.InitialLength());
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual void Delete(string uri)
		{
			Sharpen.Collections.Remove(_binsByUri, uri);
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual void Rename(string oldUri, string newUri)
		{
			IBin bin = ((IBin)Sharpen.Collections.Remove(_binsByUri, oldUri));
			if (bin == null)
			{
				throw new IOException("Bin not found: " + oldUri);
			}
			_binsByUri[newUri] = bin;
		}
	}
}
