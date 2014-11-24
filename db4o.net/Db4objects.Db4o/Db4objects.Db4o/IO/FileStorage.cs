/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.IO;
using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation.IO;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Internal;
using Sharpen.IO;
using Sharpen.Lang;

namespace Db4objects.Db4o.IO
{
	/// <summary>
	/// Storage adapter to store db4o database data to physical
	/// files on hard disc.
	/// </summary>
	/// <remarks>
	/// Storage adapter to store db4o database data to physical
	/// files on hard disc.
	/// </remarks>
	public class FileStorage : IStorage
	{
		/// <summary>
		/// opens a
		/// <see cref="IBin">IBin</see>
		/// on the specified URI (file system path).
		/// </summary>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual IBin Open(BinConfiguration config)
		{
			return new FileStorage.FileBin(config);
		}

		/// <summary>returns true if the specified file system path already exists.</summary>
		/// <remarks>returns true if the specified file system path already exists.</remarks>
		public virtual bool Exists(string uri)
		{
			Sharpen.IO.File file = new Sharpen.IO.File(uri);
			return file.Exists() && file.Length() > 0;
		}

		public class FileBin : IBin
		{
			private readonly string _path;

			private RandomAccessFile _file;

			/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
			public FileBin(BinConfiguration config)
			{
				bool ok = false;
				try
				{
					_path = new Sharpen.IO.File(config.Uri()).GetCanonicalPath();
					_file = RandomAccessFileFactory.NewRandomAccessFile(_path, config.ReadOnly(), config
						.LockFile());
					if (config.InitialLength() > 0)
					{
						Write(config.InitialLength() - 1, new byte[] { 0 }, 1);
					}
					ok = true;
				}
				catch (IOException e)
				{
					throw new Db4oIOException(e);
				}
				finally
				{
					if (!ok)
					{
						Close();
					}
				}
			}

			/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
			public virtual void Close()
			{
				Platform4.UnlockFile(_path, _file);
				try
				{
					if (!IsClosed())
					{
						_file.Close();
					}
				}
				catch (IOException e)
				{
					throw new Db4oIOException(e);
				}
				finally
				{
					_file = null;
				}
			}

			internal virtual bool IsClosed()
			{
				return _file == null;
			}

			/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
			public virtual long Length()
			{
				try
				{
					return _file.Length();
				}
				catch (IOException e)
				{
					throw new Db4oIOException(e);
				}
			}

			/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
			public virtual int Read(long pos, byte[] bytes, int length)
			{
				try
				{
					Seek(pos);
					if (DTrace.enabled)
					{
						DTrace.FileRead.LogLength(pos, length);
					}
					return _file.Read(bytes, 0, length);
				}
				catch (IOException e)
				{
					throw new Db4oIOException(e);
				}
			}

			/// <exception cref="System.IO.IOException"></exception>
			internal virtual void Seek(long pos)
			{
				if (DTrace.enabled)
				{
					DTrace.RegularSeek.Log(pos);
				}
				_file.Seek(pos);
			}

			/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
			public virtual void Sync()
			{
				try
				{
					_file.GetFD().Sync();
				}
				catch (IOException e)
				{
					throw new Db4oIOException(e);
				}
			}

			public virtual int SyncRead(long position, byte[] bytes, int bytesToRead)
			{
				return Read(position, bytes, bytesToRead);
			}

			/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
			public virtual void Write(long pos, byte[] buffer, int length)
			{
				CheckClosed();
				try
				{
					Seek(pos);
					if (DTrace.enabled)
					{
						DTrace.FileWrite.LogLength(pos, length);
					}
					_file.Write(buffer, 0, length);
				}
				catch (IOException e)
				{
					throw new Db4oIOException(e);
				}
			}

			private void CheckClosed()
			{
				if (IsClosed())
				{
					throw new Db4oIOException();
				}
			}

			public virtual void Sync(IRunnable runnable)
			{
				Sync();
				runnable.Run();
				Sync();
			}
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual void Delete(string uri)
		{
			File4.Delete(uri);
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual void Rename(string oldUri, string newUri)
		{
			System.IO.File.Move(oldUri, newUri);
		}
	}
}
