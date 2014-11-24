/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using System.Text;
using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Defragment;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Api;
using Db4objects.Db4o.Tests.Common.Defragment;

namespace Db4objects.Db4o.Tests.Common.Defragment
{
	public class BlockSizeDefragTestCase : Db4oTestWithTempFile
	{
		public class ItemA
		{
			public int _id;

			public ItemA(int id)
			{
				_id = id;
			}

			public override string ToString()
			{
				return "A" + _id;
			}
		}

		public class ItemB
		{
			public string _name;

			public ItemB(string name)
			{
				_name = name;
			}

			public override string ToString()
			{
				return "A" + _name;
			}
		}

		private static readonly int[] BlockSizes = new int[] { 1, 2, 3, 4, 7, 8, 13, 16 };

		private const int NumItemsPerClass = 10;

		private const int DeleteRatio = 3;

		/// <exception cref="System.IO.IOException"></exception>
		public virtual void Test()
		{
			for (int idx = 0; idx < BlockSizes.Length; idx++)
			{
				AssertBlockSizeDefrag(BlockSizes[idx]);
			}
		}

		/// <exception cref="System.IO.IOException"></exception>
		private void AssertBlockSizeDefrag(int blockSize)
		{
			string fileName = TempFile();
			new Sharpen.IO.File(fileName).Delete();
			CreateDatabase(fileName, blockSize);
			Defrag(fileName, blockSize);
			AssertCanRead(fileName, blockSize);
			new Sharpen.IO.File(fileName).Delete();
		}

		private void CreateDatabase(string fileName, int blockSize)
		{
			IObjectContainer db = Db4oEmbedded.OpenFile(Config(blockSize), fileName);
			Collection4 removed = new Collection4();
			for (int idx = 0; idx < NumItemsPerClass; idx++)
			{
				BlockSizeDefragTestCase.ItemA itemA = new BlockSizeDefragTestCase.ItemA(idx);
				BlockSizeDefragTestCase.ItemB itemB = new BlockSizeDefragTestCase.ItemB(FillStr('x'
					, idx));
				db.Store(itemA);
				db.Store(itemB);
				if ((idx % DeleteRatio) == 0)
				{
					removed.Add(itemA);
					removed.Add(itemB);
				}
			}
			db.Commit();
			DeleteAndReadd(db, removed);
			db.Close();
		}

		private void DeleteAndReadd(IObjectContainer db, Collection4 removed)
		{
			IEnumerator removeIter = removed.GetEnumerator();
			while (removeIter.MoveNext())
			{
				db.Delete(removeIter.Current);
			}
			db.Commit();
			IEnumerator readdIter = removed.GetEnumerator();
			while (readdIter.MoveNext())
			{
				db.Store(readdIter.Current);
			}
			db.Commit();
		}

		/// <exception cref="System.IO.IOException"></exception>
		[System.ObsoleteAttribute(@"using deprecated api")]
		private void Defrag(string fileName, int blockSize)
		{
			DefragmentConfig config = new DefragmentConfig(fileName);
			config.Db4oConfig(Config(blockSize));
			config.ForceBackupDelete(true);
			Db4objects.Db4o.Defragment.Defragment.Defrag(config);
		}

		private void AssertCanRead(string fileName, int blockSize)
		{
			IObjectContainer db = Db4oEmbedded.OpenFile(Config(blockSize), fileName);
			AssertResult(db, typeof(BlockSizeDefragTestCase.ItemA));
			AssertResult(db, typeof(BlockSizeDefragTestCase.ItemB));
			db.Close();
		}

		private void AssertResult(IObjectContainer db, Type clazz)
		{
			IObjectSet result = db.Query(clazz);
			Assert.AreEqual(NumItemsPerClass, result.Count);
			while (result.HasNext())
			{
				Assert.IsInstanceOf(clazz, result.Next());
			}
		}

		private IEmbeddedConfiguration Config(int blockSize)
		{
			IEmbeddedConfiguration config = NewConfiguration();
			config.File.BlockSize = blockSize;
			config.Common.ReflectWith(Platform4.ReflectorForType(typeof(BlockSizeDefragTestCase.ItemA
				)));
			return config;
		}

		private string FillStr(char ch, int len)
		{
			StringBuilder buf = new StringBuilder();
			for (int idx = 0; idx < len; idx++)
			{
				buf.Append(ch);
			}
			return buf.ToString();
		}
	}
}
