/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Assorted;
using Sharpen.Lang;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class ConcurrentRenameTestCase : ITestLifeCycle
	{
		public static void Main(string[] args)
		{
			new ConsoleTestRunner(typeof(ConcurrentRenameTestCase)).Run();
		}

		private static readonly string DatabaseFileName = string.Empty;

		internal const int NumIterations = 500;

		public class QueryItem
		{
		}

		public class RenameItem
		{
		}

		public abstract class RunnerBase : IRunnable
		{
			private IObjectContainer _db;

			private IList _exceptions;

			protected RunnerBase(IObjectContainer db, IList exceptions)
			{
				_db = db;
				_exceptions = exceptions;
			}

			protected abstract void Exercise(IObjectContainer db);

			public virtual void Run()
			{
				try
				{
					for (int i = 0; i < NumIterations; i++)
					{
						Exercise(_db);
						Runtime4.Sleep(1);
					}
				}
				catch (Exception ex)
				{
					lock (_exceptions)
					{
						_exceptions.Add(ex);
					}
				}
			}
		}

		public class QueryRunner : ConcurrentRenameTestCase.RunnerBase
		{
			public QueryRunner(IObjectContainer db, IList exceptions) : base(db, exceptions)
			{
			}

			protected override void Exercise(IObjectContainer db)
			{
				Assert.AreEqual(1, db.Query(typeof(ConcurrentRenameTestCase.QueryItem)).Count);
				ConcurrentRenameTestCase.QueryItem newItem = new ConcurrentRenameTestCase.QueryItem
					();
				db.Store(newItem);
				db.Commit();
				db.Delete(newItem);
				db.Commit();
			}
		}

		public class RenameRunner : ConcurrentRenameTestCase.RunnerBase
		{
			private static readonly string OriginalName = ReflectPlatform.FullyQualifiedName(
				typeof(ConcurrentRenameTestCase.RenameItem));

			private static readonly string NewName = OriginalName + "X";

			public RenameRunner(IObjectContainer db, IList exceptions) : base(db, exceptions)
			{
			}

			protected override void Exercise(IObjectContainer db)
			{
				RenameClass(db, OriginalName, NewName);
				RenameClass(db, NewName, OriginalName);
			}

			private void RenameClass(IObjectContainer db, string originalName, string newName
				)
			{
				IStoredClass storedClass = db.Ext().StoredClass(originalName);
				storedClass.Rename(newName);
			}
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Test()
		{
			IEmbeddedObjectContainer db = OpenDatabase();
			IList exceptions = new ArrayList();
			Thread[] threads = new Thread[] { new Thread(new ConcurrentRenameTestCase.QueryRunner
				(db, exceptions), "ConcurrentRenameTestCase.test Thread[0]"), new Thread(new ConcurrentRenameTestCase.RenameRunner
				(db, exceptions), "ConcurrentRenameTestCase.test Thread[1]") };
			for (int threadIndex = 0; threadIndex < threads.Length; ++threadIndex)
			{
				Thread thread = threads[threadIndex];
				thread.Start();
			}
			for (int threadIndex = 0; threadIndex < threads.Length; ++threadIndex)
			{
				Thread thread = threads[threadIndex];
				thread.Join();
			}
			db.Close();
			Assert.AreEqual(0, exceptions.Count);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void SetUp()
		{
			IEmbeddedObjectContainer db = OpenDatabase();
			db.Store(new ConcurrentRenameTestCase.QueryItem());
			db.Store(new ConcurrentRenameTestCase.RenameItem());
			db.Close();
		}

		private IEmbeddedObjectContainer OpenDatabase()
		{
			IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
			config.File.Storage = _storage;
			return Db4oEmbedded.OpenFile(config, DatabaseFileName);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TearDown()
		{
		}

		private MemoryStorage _storage = new MemoryStorage();
	}
}
