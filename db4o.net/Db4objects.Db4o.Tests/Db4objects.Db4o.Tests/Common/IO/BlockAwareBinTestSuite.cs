/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Fixtures;
using Db4oUnit.Mocking;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Tests.Common.IO;
using Sharpen.Lang;

namespace Db4objects.Db4o.Tests.Common.IO
{
	public class BlockAwareBinTestSuite : FixtureTestSuiteDescription
	{
		public class BlockAwareBinTest : ITestCase, IEnvironment
		{
			private readonly MockBin _mockBin = new MockBin();

			private sealed class _IBlockSize_20 : IBlockSize
			{
				public _IBlockSize_20(BlockAwareBinTest _enclosing)
				{
					this._enclosing = _enclosing;
				}

				public void Register(IListener4 listener)
				{
					throw new NotImplementedException();
				}

				public void Set(int newValue)
				{
					Assert.AreEqual(this._enclosing.BlockSize(), newValue);
				}

				public int Value()
				{
					return this._enclosing.BlockSize();
				}

				private readonly BlockAwareBinTest _enclosing;
			}

			private readonly IBlockSize _mockBlockSize;

			private BlockAwareBin _subject;

			public BlockAwareBinTest()
			{
				_mockBlockSize = new _IBlockSize_20(this);
				Environments.RunWith(this, new _IRunnable_37(this));
			}

			private sealed class _IRunnable_37 : IRunnable
			{
				public _IRunnable_37(BlockAwareBinTest _enclosing)
				{
					this._enclosing = _enclosing;
				}

				public void Run()
				{
					this._enclosing._subject = new BlockAwareBin(this._enclosing._mockBin);
				}

				private readonly BlockAwareBinTest _enclosing;
			}

			public virtual object Provide(Type service)
			{
				if (service != typeof(IBlockSize))
				{
					throw new ArgumentException();
				}
				return (object)_mockBlockSize;
			}

			public virtual void TestBlockSize()
			{
				Assert.AreEqual(BlockSize(), _subject.BlockSize());
			}

			public virtual void TestClose()
			{
				_subject.Close();
				Verify(new MethodCall[] { new MethodCall("close", new object[] {  }) });
			}

			public virtual void TestSync()
			{
				_subject.Sync();
				Verify(new MethodCall[] { new MethodCall("sync", new object[] {  }) });
			}

			public virtual void TestBlockReadReturnsStorageReturnValue()
			{
				_mockBin.ReturnValueForNextCall(-1);
				Assert.AreEqual(-1, _subject.BlockRead(0, new byte[10]));
			}

			public virtual void TestBlockRead()
			{
				byte[] buffer = new byte[10];
				_subject.BlockRead(0, buffer);
				_subject.BlockRead(1, buffer, 5);
				_subject.BlockRead(42, buffer);
				Verify(new MethodCall[] { new MethodCall("read", new object[] { 0L, buffer, buffer
					.Length }), new MethodCall("read", new object[] { (long)BlockSize(), buffer, 5 }
					), new MethodCall("read", new object[] { 42L * BlockSize(), buffer, buffer.Length
					 }) });
			}

			public virtual void TestBlockReadWithOffset()
			{
				byte[] buffer = new byte[10];
				_subject.BlockRead(0, 1, buffer);
				_subject.BlockRead(1, 3, buffer, 5);
				_subject.BlockRead(42, 5, buffer);
				Verify(new MethodCall[] { new MethodCall("read", new object[] { 1L, buffer, buffer
					.Length }), new MethodCall("read", new object[] { 3 + (long)BlockSize(), buffer, 
					5 }), new MethodCall("read", new object[] { 5 + 42L * BlockSize(), buffer, buffer
					.Length }) });
			}

			public virtual void TestBlockWrite()
			{
				byte[] buffer = new byte[10];
				_subject.BlockWrite(0, buffer);
				_subject.BlockWrite(1, buffer, 5);
				_subject.BlockWrite(42, buffer);
				Verify(new MethodCall[] { new MethodCall("write", new object[] { 0L, buffer, buffer
					.Length }), new MethodCall("write", new object[] { (long)BlockSize(), buffer, 5 }
					), new MethodCall("write", new object[] { 42L * BlockSize(), buffer, buffer.Length
					 }) });
			}

			public virtual void TestBlockWriteWithOffset()
			{
				byte[] buffer = new byte[10];
				_subject.BlockWrite(0, 1, buffer);
				_subject.BlockWrite(1, 3, buffer, 5);
				_subject.BlockWrite(42, 5, buffer);
				Verify(new MethodCall[] { new MethodCall("write", new object[] { 1L, buffer, buffer
					.Length }), new MethodCall("write", new object[] { 3 + (long)BlockSize(), buffer
					, 5 }), new MethodCall("write", new object[] { 5 + 42L * BlockSize(), buffer, buffer
					.Length }) });
			}

			private void Verify(MethodCall[] expectedCalls)
			{
				_mockBin.Verify(expectedCalls);
			}

			private int BlockSize()
			{
				return ((int)SubjectFixtureProvider.Value());
			}
		}

		public BlockAwareBinTestSuite()
		{
			{
				FixtureProviders(new IFixtureProvider[] { new SubjectFixtureProvider(new int[] { 
					2, 3, 17 }) });
				TestUnits(new Type[] { typeof(BlockAwareBinTestSuite.BlockAwareBinTest) });
			}
		}
	}
}
