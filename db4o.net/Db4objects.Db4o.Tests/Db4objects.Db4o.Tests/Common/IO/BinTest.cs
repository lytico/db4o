/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Tests.Common.IO;

namespace Db4objects.Db4o.Tests.Common.IO
{
	public class BinTest : StorageTestUnitBase
	{
		public static void Main(string[] args)
		{
			new ConsoleTestRunner(typeof(BinTest)).Run();
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestReadWrite()
		{
			int count = 1024 * 8 + 10;
			byte[] data = new byte[count];
			for (int i = 0; i < count; ++i)
			{
				data[i] = (byte)(i % 256);
			}
			_bin.Write(0, data, data.Length);
			_bin.Sync();
			byte[] readBytes = new byte[count];
			_bin.Read(0, readBytes, readBytes.Length);
			for (int i = 0; i < count; i++)
			{
				Assert.AreEqual(data[i], readBytes[i]);
			}
		}

		public virtual void TestHugeFile()
		{
			int dataSize = 1024 * 2;
			byte[] data = NewDataArray(dataSize);
			for (int i = 0; i < 64; ++i)
			{
				_bin.Write(i * data.Length, data, data.Length);
			}
			byte[] readBuffer = new byte[dataSize];
			for (int i = 0; i < 64; ++i)
			{
				_bin.Read(dataSize * (63 - i), readBuffer, readBuffer.Length);
				ArrayAssert.AreEqual(data, readBuffer);
			}
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestSeek()
		{
			int count = 1024 * 2 + 10;
			byte[] data = NewDataArray(count);
			_bin.Write(0, data, data.Length);
			byte[] readBytes = new byte[count];
			_bin.Read(0, readBytes, readBytes.Length);
			for (int i = 0; i < count; i++)
			{
				Assert.AreEqual(data[i], readBytes[i]);
			}
			_bin.Read(20, readBytes, readBytes.Length);
			for (int i = 0; i < count - 20; i++)
			{
				Assert.AreEqual(data[i + 20], readBytes[i]);
			}
			byte[] writtenData = new byte[10];
			for (int i = 0; i < writtenData.Length; ++i)
			{
				writtenData[i] = (byte)i;
			}
			_bin.Write(1000, writtenData, writtenData.Length);
			int readCount = _bin.Read(1000, readBytes, 10);
			Assert.AreEqual(10, readCount);
			for (int i = 0; i < readCount; ++i)
			{
				Assert.AreEqual(i, readBytes[i]);
			}
		}

		private byte[] NewDataArray(int count)
		{
			byte[] data = new byte[count];
			for (int i = 0; i < data.Length; ++i)
			{
				data[i] = (byte)(i % 256);
			}
			return data;
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestReadWriteBytes()
		{
			string[] strs = new string[] { "short string", "this is a really long string, just to make sure all Storage implementations work correctly. "
				 };
			for (int j = 0; j < strs.Length; j++)
			{
				AssertReadWriteString(_bin, strs[j]);
			}
		}

		/// <exception cref="System.Exception"></exception>
		private void AssertReadWriteString(IBin adapter, string str)
		{
			byte[] data = Sharpen.Runtime.GetBytesForString(str);
			byte[] read = new byte[2048];
			adapter.Write(0, data, data.Length);
			adapter.Read(0, read, read.Length);
			Assert.AreEqual(str, Sharpen.Runtime.GetStringForBytes(read, 0, data.Length));
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void _testReadWriteAheadFileEnd()
		{
			string str = "this is a really long string, just to make sure that all Storage implementations work correctly. ";
			AssertReadWriteAheadFileEnd(_bin, str);
		}

		/// <exception cref="System.Exception"></exception>
		private void AssertReadWriteAheadFileEnd(IBin adapter, string str)
		{
			byte[] data = Sharpen.Runtime.GetBytesForString(str);
			byte[] read = new byte[2048];
			int readBytes = adapter.Read(10, data, data.Length);
			Assert.AreEqual(-1, readBytes);
			Assert.AreEqual(0, adapter.Length());
			readBytes = adapter.Read(0, data, data.Length);
			Assert.AreEqual(-1, readBytes);
			Assert.AreEqual(0, adapter.Length());
			adapter.Write(10, data, data.Length);
			Assert.AreEqual(10 + data.Length, adapter.Length());
			readBytes = adapter.Read(0, read, read.Length);
			Assert.AreEqual(10 + data.Length, readBytes);
			readBytes = adapter.Read(20 + data.Length, read, read.Length);
			Assert.AreEqual(-1, readBytes);
			readBytes = adapter.Read(1024 + data.Length, read, read.Length);
			Assert.AreEqual(-1, readBytes);
			adapter.Write(1200, data, data.Length);
			readBytes = adapter.Read(0, read, read.Length);
			Assert.AreEqual(1200 + data.Length, readBytes);
		}
	}
}
