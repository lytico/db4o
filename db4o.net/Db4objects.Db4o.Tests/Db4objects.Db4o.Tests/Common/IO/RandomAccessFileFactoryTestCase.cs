/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.IO;
using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Api;
using Sharpen.IO;

namespace Db4objects.Db4o.Tests.Common.IO
{
	public class RandomAccessFileFactoryTestCase : TestWithTempFile
	{
		/// <exception cref="System.IO.IOException"></exception>
		public virtual void TestLockDatabaseFileFalse()
		{
			IObjectContainer container = OpenObjectContainer(false);
			RandomAccessFile raf = RandomAccessFileFactory.NewRandomAccessFile(TempFile(), false
				, false);
			byte[] bytes = new byte[1];
			raf.Read(bytes);
			raf.Close();
			container.Close();
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual void TestLockDatabaseFileTrue()
		{
			IObjectContainer container = OpenObjectContainer(true);
			if (!Platform4.NeedsLockFileThread())
			{
				Assert.Expect(typeof(DatabaseFileLockedException), new _ICodeBlock_31(this));
			}
			container.Close();
		}

		private sealed class _ICodeBlock_31 : ICodeBlock
		{
			public _ICodeBlock_31(RandomAccessFileFactoryTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				RandomAccessFileFactory.NewRandomAccessFile(this._enclosing.TempFile(), false, true
					);
			}

			private readonly RandomAccessFileFactoryTestCase _enclosing;
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual void TestReadOnlyLocked()
		{
			byte[] bytes = new byte[1];
			RandomAccessFile raf = RandomAccessFileFactory.NewRandomAccessFile(TempFile(), true
				, true);
			Assert.Expect(typeof(IOException), new _ICodeBlock_43(raf, bytes));
			raf.Close();
		}

		private sealed class _ICodeBlock_43 : ICodeBlock
		{
			public _ICodeBlock_43(RandomAccessFile raf, byte[] bytes)
			{
				this.raf = raf;
				this.bytes = bytes;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				raf.Write(bytes);
			}

			private readonly RandomAccessFile raf;

			private readonly byte[] bytes;
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual void TestReadOnlyUnLocked()
		{
			byte[] bytes = new byte[1];
			RandomAccessFile raf = RandomAccessFileFactory.NewRandomAccessFile(TempFile(), true
				, false);
			Assert.Expect(typeof(IOException), new _ICodeBlock_54(raf, bytes));
			raf.Close();
		}

		private sealed class _ICodeBlock_54 : ICodeBlock
		{
			public _ICodeBlock_54(RandomAccessFile raf, byte[] bytes)
			{
				this.raf = raf;
				this.bytes = bytes;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				raf.Write(bytes);
			}

			private readonly RandomAccessFile raf;

			private readonly byte[] bytes;
		}

		private IObjectContainer OpenObjectContainer(bool lockDatabaseFile)
		{
			IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
			config.File.LockDatabaseFile = lockDatabaseFile;
			return Db4oEmbedded.OpenFile(config, TempFile());
		}
	}
}
