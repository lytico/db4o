/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using System.IO;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Foundation.IO;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Tests.Common.IO;

namespace Db4objects.Db4o.Tests.Common.IO
{
	public class SaveAsStorageTestCase : AbstractDb4oTestCase, IOptOutMultiSession, IOptOutInMemory
		, IOptOutNoFileSystemData, IOptOutSilverlight
	{
		public static void Main(string[] args)
		{
			new SaveAsStorageTestCase().RunSolo();
		}

		private readonly SaveAsStorage _storage = new SaveAsStorage(new CachingStorage(new 
			FileStorage()));

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.Storage = _storage;
		}

		public virtual void TestExistingFileWillNotBeOverWritten()
		{
			Db().Store(new SaveAsStorageTestCase.Item(1));
			string oldFileName = FileSession().FileName();
			ByRef newPath = new ByRef();
			try
			{
				newPath.value = Path.GetTempFileName();
				Assert.IsTrue(System.IO.File.Exists(((string)newPath.value)));
				Assert.Expect(typeof(InvalidOperationException), new _ICodeBlock_34(this, oldFileName
					, newPath));
				AssertItems(Db(), 1);
			}
			finally
			{
				File4.Delete(((string)newPath.value));
			}
		}

		private sealed class _ICodeBlock_34 : ICodeBlock
		{
			public _ICodeBlock_34(SaveAsStorageTestCase _enclosing, string oldFileName, ByRef
				 newPath)
			{
				this._enclosing = _enclosing;
				this.oldFileName = oldFileName;
				this.newPath = newPath;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing._storage.SaveAs(oldFileName, ((string)newPath.value));
			}

			private readonly SaveAsStorageTestCase _enclosing;

			private readonly string oldFileName;

			private readonly ByRef newPath;
		}

		private void AssertItems(string fileName, int count)
		{
			IEmbeddedObjectContainer objectContainer = Db4oEmbedded.OpenFile(fileName);
			AssertItems(objectContainer, count);
			objectContainer.Close();
		}

		private void AssertItems(IObjectContainer objectContainer, int count)
		{
			IObjectSet items = objectContainer.Query(typeof(SaveAsStorageTestCase.Item));
			Assert.AreEqual(count, items.Count);
			Assert.AreEqual(count, items.Count);
			int countCheck = 0;
			for (IEnumerator itemIter = items.GetEnumerator(); itemIter.MoveNext(); )
			{
				SaveAsStorageTestCase.Item item = ((SaveAsStorageTestCase.Item)itemIter.Current);
				Assert.IsGreater(0, item._id);
				countCheck++;
			}
			Assert.AreEqual(count, countCheck);
		}

		public virtual void TestUnknownBin()
		{
			Db().Store(new SaveAsStorageTestCase.Item(1));
			Assert.Expect(typeof(InvalidOperationException), new _ICodeBlock_68(this));
			AssertItems(Db(), 1);
		}

		private sealed class _ICodeBlock_68 : ICodeBlock
		{
			public _ICodeBlock_68(SaveAsStorageTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing._storage.SaveAs("unknown", "unknown");
			}

			private readonly SaveAsStorageTestCase _enclosing;
		}

		public virtual void TestSaveAsTwice()
		{
			Db().Store(new SaveAsStorageTestCase.Item(1));
			Db().Commit();
			string oldFileName = FileSession().FileName();
			string firstNewFileName = SaveOldAs(oldFileName);
			AssertItems(oldFileName, 1);
			Db().Store(new SaveAsStorageTestCase.Item(2));
			Db().Commit();
			string secondNewFileName = SaveOldAs(firstNewFileName);
			AssertItems(firstNewFileName, 2);
			Db().Store(new SaveAsStorageTestCase.Item(3));
			AssertItems(Db(), 3);
			Db().Commit();
			Db().Close();
			AssertItems(secondNewFileName, 3);
		}

		public virtual void TestPartialPersistence()
		{
			string oldFileName = FileSession().FileName();
			Db().Store(new SaveAsStorageTestCase.Item(1));
			Db().Commit();
			Db().Store(new SaveAsStorageTestCase.Item(2));
			string newPath = null;
			try
			{
				newPath = SaveOldAs(oldFileName);
				IObjectSet items = Db().Query(typeof(SaveAsStorageTestCase.Item));
				Assert.AreEqual(2, items.Count);
				Db().Store(new SaveAsStorageTestCase.Item(3));
				Db().Close();
				AssertItems(oldFileName, 1);
				AssertItems(newPath, 3);
			}
			catch (Exception e)
			{
				Sharpen.Runtime.PrintStackTrace(e);
			}
			finally
			{
				File4.Delete(newPath);
			}
		}

		private string SaveOldAs(string oldFileName)
		{
			string newPath;
			newPath = Path.GetTempFileName();
			File4.Delete(newPath);
			_storage.SaveAs(oldFileName, newPath);
			return newPath;
		}

		public class Item
		{
			public int _id;

			public Item(int id)
			{
				_id = id;
			}
		}
	}
}
