/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Defragment;
using Db4objects.Db4o.Foundation.IO;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Api;
using Db4objects.Db4o.Tests.Common.Defragment;
using Sharpen;

namespace Db4objects.Db4o.Tests.Common.Defragment
{
	/// <summary>
	/// #COR-775
	/// Currently this test doesn't work with JDKs that use a
	/// timer file lock because the new logic grabs into the Bin
	/// below the MockBin and reads open times there directly.
	/// </summary>
	/// <remarks>
	/// #COR-775
	/// Currently this test doesn't work with JDKs that use a
	/// timer file lock because the new logic grabs into the Bin
	/// below the MockBin and reads open times there directly.
	/// The times are then inconsistent with the written times.
	/// </remarks>
	public class DefragEncryptedFileTestCase : Db4oTestWithTempFile
	{
		private static string Defgared;

		/// <exception cref="System.Exception"></exception>
		public override void SetUp()
		{
			Defgared = TempFile() + ".bk";
		}

		/// <exception cref="System.Exception"></exception>
		public override void TearDown()
		{
			File4.Delete(Defgared);
			base.TearDown();
		}

		public static void Main(string[] args)
		{
			new ConsoleTestRunner(typeof(DefragEncryptedFileTestCase)).Run();
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestCOR775()
		{
			Prepare();
			VerifyDB();
			DefragmentConfig config = new DefragmentConfig(TempFile(), Defgared);
			config.ForceBackupDelete(true);
			//config.storedClassFilter(new AvailableClassFilter());
			config.Db4oConfig(GetConfiguration());
			Db4objects.Db4o.Defragment.Defragment.Defrag(config);
			VerifyDB();
		}

		private void Prepare()
		{
			Sharpen.IO.File file = new Sharpen.IO.File(TempFile());
			if (file.Exists())
			{
				file.Delete();
			}
			IObjectContainer testDB = OpenDB();
			DefragEncryptedFileTestCase.Item item = new DefragEncryptedFileTestCase.Item("richard"
				, 100);
			testDB.Store(item);
			testDB.Close();
		}

		private void VerifyDB()
		{
			IObjectContainer testDB = OpenDB();
			IObjectSet result = testDB.QueryByExample(typeof(DefragEncryptedFileTestCase.Item
				));
			if (result.HasNext())
			{
				DefragEncryptedFileTestCase.Item retrievedItem = (DefragEncryptedFileTestCase.Item
					)result.Next();
				Assert.AreEqual("richard", retrievedItem.name);
				Assert.AreEqual(100, retrievedItem.value);
			}
			else
			{
				Assert.Fail("Cannot retrieve the expected object.");
			}
			testDB.Close();
		}

		private IObjectContainer OpenDB()
		{
			return Db4oEmbedded.OpenFile(GetConfiguration(), TempFile());
		}

		private IEmbeddedConfiguration GetConfiguration()
		{
			IEmbeddedConfiguration config = NewConfiguration();
			config.Common.ActivationDepth = int.MaxValue;
			config.Common.CallConstructors = true;
			config.File.Storage = new DefragEncryptedFileTestCase.MockStorage(config.File.Storage
				, "db4o");
			config.Common.ReflectWith(Platform4.ReflectorForType(typeof(DefragEncryptedFileTestCase.Item
				)));
			Db4oFactory.Configure().Password("encrypted");
			Db4oFactory.Configure().Encrypt(true);
			//TODO: CHECK ENCRYPTION
			return config;
		}

		public class Item
		{
			public string name;

			public int value;

			public Item(string name, int value)
			{
				this.name = name;
				this.value = value;
			}
		}

		public class MockStorage : StorageDecorator
		{
			private string password;

			public MockStorage(IStorage storage, string password) : base(storage)
			{
				this.password = password;
			}

			protected override IBin Decorate(BinConfiguration config, IBin bin)
			{
				return new DefragEncryptedFileTestCase.MockStorage.MockBin(bin, password);
			}

			internal class MockBin : BinDecorator
			{
				private string _password;

				public MockBin(IBin bin, string password) : base(bin)
				{
					_password = password;
				}

				/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
				public override int Read(long pos, byte[] bytes, int length)
				{
					_bin.Read(pos, bytes, length);
					for (int i = 0; i < length; i++)
					{
						bytes[i] = (byte)(bytes[i] - _password.GetHashCode());
					}
					return length;
				}

				/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
				public override int SyncRead(long pos, byte[] bytes, int length)
				{
					return Read(pos, bytes, length);
				}

				/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
				public override void Write(long pos, byte[] buffer, int length)
				{
					byte[] encryptedBuffer = new byte[buffer.Length];
					System.Array.Copy(buffer, 0, encryptedBuffer, 0, buffer.Length);
					for (int i = 0; i < length; i++)
					{
						encryptedBuffer[i] = (byte)(encryptedBuffer[i] + _password.GetHashCode());
					}
					_bin.Write(pos, encryptedBuffer, length);
				}
			}
		}
	}
}
