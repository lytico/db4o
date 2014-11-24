/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Foundation.IO;
using Db4objects.Db4o.IO;
using Sharpen.Lang;

namespace Db4objects.Db4o.IO
{
	/// <summary>
	/// Storage that allows to save an open database file
	/// as another file while keeping the reference system
	/// in place.
	/// </summary>
	/// <remarks>
	/// Storage that allows to save an open database file
	/// as another file while keeping the reference system
	/// in place. If anything goes wrong during copying the
	/// storage tries to reopen the original file, so commit
	/// operations can still take place against the original
	/// file.
	/// </remarks>
	public class SaveAsStorage : StorageDecorator
	{
		private readonly Hashtable4 _binRecords = new Hashtable4();

		public SaveAsStorage(IStorage storage) : base(storage)
		{
		}

		/// <summary>
		/// call this method to save the content of a currently
		/// open ObjectContainer session to a new file location.
		/// </summary>
		/// <remarks>
		/// call this method to save the content of a currently
		/// open ObjectContainer session to a new file location.
		/// Invocation will close the old file without a commit,
		/// keep the reference system in place and connect it to
		/// the file in the new location. If anything goes wrong
		/// during the copying operation or while opening it will
		/// be attempted to reopen the old file. In this case a
		/// Db4oException will be thrown.
		/// </remarks>
		/// <param name="oldUri">the path to the old open database file</param>
		/// <param name="newUri">the path to the new database file</param>
		public virtual void SaveAs(string oldUri, string newUri)
		{
			if (System.IO.File.Exists(newUri))
			{
				throw new InvalidOperationException(newUri + " already exists");
			}
			SaveAsStorage.BinRecord binRecord = (SaveAsStorage.BinRecord)_binRecords.Get(oldUri
				);
			if (binRecord == null)
			{
				throw new InvalidOperationException(oldUri + " was never opened or was closed.");
			}
			BinConfiguration oldConfiguration = binRecord._binConfiguration;
			SaveAsStorage.SaveAsBin saveAsBin = binRecord._bin;
			IRunnable closure = new _IRunnable_49(this, saveAsBin, oldUri, newUri, oldConfiguration
				);
			saveAsBin.ExchangeUnderlyingBin(closure);
		}

		private sealed class _IRunnable_49 : IRunnable
		{
			public _IRunnable_49(SaveAsStorage _enclosing, SaveAsStorage.SaveAsBin saveAsBin, 
				string oldUri, string newUri, BinConfiguration oldConfiguration)
			{
				this._enclosing = _enclosing;
				this.saveAsBin = saveAsBin;
				this.oldUri = oldUri;
				this.newUri = newUri;
				this.oldConfiguration = oldConfiguration;
			}

			public void Run()
			{
				saveAsBin.Sync();
				saveAsBin.Close();
				try
				{
					File4.Copy(oldUri, newUri);
				}
				catch (Exception e)
				{
					this._enclosing.ReopenOldConfiguration(saveAsBin, oldConfiguration, newUri, e);
				}
				BinConfiguration newConfiguration = this._enclosing.PointToNewUri(oldConfiguration
					, newUri);
				try
				{
					IBin newBin = this._enclosing._storage.Open(newConfiguration);
					saveAsBin.DelegateTo(newBin);
					this._enclosing._binRecords.Remove(oldUri);
					this._enclosing._binRecords.Put(newUri, new SaveAsStorage.BinRecord(newConfiguration
						, saveAsBin));
				}
				catch (Exception e)
				{
					this._enclosing.ReopenOldConfiguration(saveAsBin, oldConfiguration, newUri, e);
				}
			}

			private readonly SaveAsStorage _enclosing;

			private readonly SaveAsStorage.SaveAsBin saveAsBin;

			private readonly string oldUri;

			private readonly string newUri;

			private readonly BinConfiguration oldConfiguration;
		}

		private BinConfiguration PointToNewUri(BinConfiguration oldConfig, string newUri)
		{
			return new BinConfiguration(newUri, oldConfig.LockFile(), oldConfig.InitialLength
				(), oldConfig.ReadOnly());
		}

		private void ReopenOldConfiguration(SaveAsStorage.SaveAsBin saveAsBin, BinConfiguration
			 config, string newUri, Exception e)
		{
			IBin safeBin = _storage.Open(config);
			saveAsBin.DelegateTo(safeBin);
			throw new Db4oException("Copying to " + newUri + " failed. Reopened " + config.Uri
				(), e);
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override IBin Open(BinConfiguration config)
		{
			SaveAsStorage.SaveAsBin openedBin = new SaveAsStorage.SaveAsBin(base.Open(config)
				);
			_binRecords.Put(config.Uri(), new SaveAsStorage.BinRecord(config, openedBin));
			return openedBin;
		}

		private class BinRecord
		{
			internal readonly SaveAsStorage.SaveAsBin _bin;

			internal readonly BinConfiguration _binConfiguration;

			internal BinRecord(BinConfiguration binConfiguration, SaveAsStorage.SaveAsBin bin
				)
			{
				_binConfiguration = binConfiguration;
				_bin = bin;
			}
		}

		/// <summary>
		/// We could have nicely used BinDecorator here, but
		/// BinDecorator doesn't allow exchanging the Bin.
		/// </summary>
		/// <remarks>
		/// We could have nicely used BinDecorator here, but
		/// BinDecorator doesn't allow exchanging the Bin. To
		/// be compatible with released versions we do
		/// </remarks>
		private class SaveAsBin : IBin
		{
			private IBin _bin;

			internal SaveAsBin(IBin delegate_)
			{
				_bin = delegate_;
			}

			public virtual void ExchangeUnderlyingBin(IRunnable closure)
			{
				lock (this)
				{
					closure.Run();
				}
			}

			public virtual void Close()
			{
				lock (this)
				{
					_bin.Close();
				}
			}

			public virtual long Length()
			{
				lock (this)
				{
					return _bin.Length();
				}
			}

			public virtual int Read(long position, byte[] bytes, int bytesToRead)
			{
				lock (this)
				{
					return _bin.Read(position, bytes, bytesToRead);
				}
			}

			public virtual void Sync()
			{
				lock (this)
				{
					_bin.Sync();
				}
			}

			public virtual void Sync(IRunnable runnable)
			{
				lock (this)
				{
					Sync();
					runnable.Run();
					Sync();
				}
			}

			public virtual int SyncRead(long position, byte[] bytes, int bytesToRead)
			{
				lock (this)
				{
					return _bin.SyncRead(position, bytes, bytesToRead);
				}
			}

			public virtual void Write(long position, byte[] bytes, int bytesToWrite)
			{
				lock (this)
				{
					_bin.Write(position, bytes, bytesToWrite);
				}
			}

			public virtual void DelegateTo(IBin bin)
			{
				_bin = bin;
			}
		}
	}
}
